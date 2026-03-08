namespace PlanetaryDeception
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Level 2
    /// </summary>
    public class LevelController_2 : LevelBase
    {
        /// <summary>
        /// Q&A console for interaction
        /// </summary>
        public TinyOS QA;

        /// <summary>
        /// Update Event handler
        /// </summary>
        public void Update()
        {
            currentInstance = this;

            if (Input.GetButton("Fire1"))
            {
                var playerInventory = PlayerInventory.Instance();

                if (PlayerIsTouching("Console"))
                {
                    AlertText.text = "You have no access to this terminal";
                }
                else if (PlayerIsTouching("Door"))
                {
                    if (playerInventory.ContainsItem(KnownItem.PlayerSecurityAccessCard))
                    {
                        var settings = CharacterSettings.Instance();
                        settings.TransitionToNewScene("Level_1", Player);
                        return;
                    }
                    else
                    {
                        AlertText.text = "Access denied, you need your Security Access card";
                    }
                }
                else if (PlayerIsTouching("LaunchEntrance"))
                {
                    if (playerInventory.ContainsItem(KnownItem.VoasisWebsiteCredentialsUsage) && !playerInventory.ContainsItem(KnownItem.VenrefInterrogated))
                    {
                        var settings = CharacterSettings.Instance();
                        settings.TransitionToNewScene("Level_2_interrogation", Player);
                    }
                    else
                    {
                        var settings = CharacterSettings.Instance();
                        settings.TransitionToNewScene("Level_2_launch", Player);
                    }
                    return;
                }
                else if (PlayerIsTouching("ViteEntrance"))
                {
                    AlertText.text = "The office is currently closed";
                }
                else if (PlayerIsTouching("NotSoUndercoverAgent"))
                {
                    AlertText.text = "Someone pretending to read a newspaper.";
                }
                else if (PlayerIsTouching("Flowershop"))
                {
                    if (playerInventory.ContainsItem(KnownItem.PinkRoses) || playerInventory.ContainsItem(KnownItem.RedRoses))
                    {
                        AlertText.text = "I see you're enjoying the roses! Come back anytime.";
                    }
                    else if (playerInventory.ContainsItem(KnownItem.FlowershopVisited))
                    {
                        AlertText.text = "Good to see you again, have a nice day!";
                    }
                    else
                    {
                        var group = QA.GetComponent<CanvasGroup>();
                        if (group.alpha == 0)
                        {
                            group.alpha = 1;
                            FlowershopQA1(QA, () => { QA.Clear(); group.alpha = 0; });
                        }
                    }
                }
            }
        }
        private void FlowershopExit(CanvasGroup group)
        {
            FlowershopExit((Action)(() =>
            {
                QA.Clear();
                group.alpha = 0;
            }));
        }

        public void FlowershopExit(Action onExit)
        {
            KnownItemsInventory.Instance().TransferItem(KnownItem.FlowershopVisited, PlayerInventory.Instance());
            onExit();
        }

        public void FlowershopQA1(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion("Hello, what can I help you with today?");
            dialog.AddPossibleAnswer(
                "I'm not sure.",
                () =>
                {
                    dialog.NewQuestion("Feel free to look around.");
                    dialog.AddPossibleAnswer("Thanks.", () => { FlowershopExit(onExit); });
                });
            dialog.AddPossibleAnswer(
                "Just looking for something to cheer up my quarters.",
                () => { FlowershopQA1_2(dialog, onExit); });
        }

        private void FlowershopQA1_2(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion("How wonderful! I'm sure I have something to your liking. Are you ok, though? You seem worried?");
            dialog.AddPossibleAnswer(
                "I am, I'm not sure, something is wrong, but I'm not sure what.",
                () => { FlowershopQA1_2_branch(dialog, onExit); });
            dialog.AddPossibleAnswer(
                "No, you're right. I'm unsure of anything right now.",
                () => { FlowershopQA1_2_branch(dialog, onExit); });
        }

        private void FlowershopQA1_2_branch(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion(
                "Is it about the bombings? Things are scary nowadays with the Venusian Independence movement, " +
                "but that's only more reason to fill your life with more joy.");
            dialog.AddPossibleAnswer(
                "True, these radicals ruin everything good about our way of living.",
                () => { FlowershopQA2_1(dialog, onExit); });
            dialog.AddPossibleAnswer(
                "True, but maybe they have a good reason to be angry.",
                () => { FlowershopQA2_1(dialog, onExit); });
        }

        private void FlowershopBuy(IDialogConsole dialog, Action onExit, KnownItem roses)
        {
            var playerWallet = PlayerWallet.Instance();
            var price = 5;
            playerWallet.Transfer(price, new SolarbitsWallet());
            KnownItemsInventory.Instance().TransferItem(roses, PlayerInventory.Instance());
            FlowershopExit(onExit);
        }

        private void FlowershopQA2_1(IDialogConsole dialog, Action onExit)
        {
            var price = 5;
            dialog.NewQuestion(
                "That's why I love this Pink Rose, it gives me hope things will get better. " +
                "Red roses only trick me into thinking things are already perfect, while they're not.");
            dialog.AddPossibleAnswer(
                "I think I would love some pink roses in my quarters, some positivity is always welcome. (" + price + " solarbits)",
                () =>
                {
                    dialog.NewQuestion("Ok, I think that would be lovely.");
                    dialog.AddPossibleAnswer("Thanks!", () => { FlowershopBuy(dialog, onExit, KnownItem.PinkRoses); });
                });
            dialog.AddPossibleAnswer(
                "Maybe I'd prefer the ignorance. I want red ones. (" + price + " solarbits)",
                () =>
                {
                    dialog.NewQuestion("Oh... well, sometimes we all need a little comfort. Here you go.");
                    dialog.AddPossibleAnswer("Thanks!", () => { FlowershopBuy(dialog, onExit, KnownItem.RedRoses); });
                });
        }
    }
}