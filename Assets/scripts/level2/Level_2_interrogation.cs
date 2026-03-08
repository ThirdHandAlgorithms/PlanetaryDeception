namespace PlanetaryDeception
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// Level 2 interrogation part
    /// </summary>
    public class Level_2_interrogation : LevelBase
    {
        /// <summary>
        /// Console output
        /// </summary>
        public Text ConsoleOutput;

        /// <summary>
        /// Put the link to the TinyOS here
        /// </summary>
        public TinyOS OS;

        /// <summary>
        /// are the questions set
        /// </summary>
        private bool questionsInitialized = false;

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            // Order of executing Start() on TinyOS seems to be After the Level is Started for some reason...
            if (OS.IsStarted && !questionsInitialized)
            {
                InitQuestions();
                questionsInitialized = true;
            }
        }

        /// <summary>
        /// When you're allowed to leave, and maybe get a free launch assistance ticket
        /// </summary>
        /// <param name="freeTicket"></param>
        private void LeaveInterrogation(bool freeTicket)
        {
            LeaveInterrogation(freeTicket, () =>
            {
                var settings = CharacterSettings.Instance();
                settings.TransitionToNewScene("Level_2", Player);
            });
        }

        /// <summary>
        /// When you're allowed to leave, and maybe get a free launch assistance ticket
        /// </summary>
        /// <param name="freeTicket"></param>
        /// <param name="onExit"></param>
        public void LeaveInterrogation(bool freeTicket, Action onExit)
        {
            var known = KnownItemsInventory.Instance();
            var inv = PlayerInventory.Instance();

            known.TransferItem(KnownItem.VenrefInterrogated, inv);

            if (freeTicket)
            {
                known.TransferItem(KnownItem.VenusLaunchAssistanceTicket, inv);
            }

            onExit();
        }

        /// <summary>
        /// Init Questions
        /// </summary>
        private void InitQuestions()
        {
            InitQuestions(OS, () => { });
        }

        /// <summary>
        /// Init Questions with dialog console and exit callback
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="onExit"></param>
        public void InitQuestions(IDialogConsole dialog, Action onExit)
        {
            var settings = CharacterSettings.Instance();
            dialog.NewQuestion(
                "Agent 1: " + settings.Name + ", right? We know everything, you better 'fess up.\n" +
                "Agent 2: Don't mind my colleague here, you're not in any trouble, we're just gathering some information, that's all.");
            dialog.AddPossibleAnswer("Ok", () => { QA2(dialog, onExit); });
        }

        private void QA2(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion(
                "Agent 2: You work for Vite, right? They make software for our organisation. Can you describe your experiences concerning their cooperation?\n" +
                "Agent 1: I bet you're a supporter of those savages from \"Venusian Independence\" who think we're the enemy while they're the ones destroying the domes.");
            dialog.AddPossibleAnswer("I'm not sure, I just write code. Everything else is handled by management. And I'm definitely not part of VI.", () => { QA3(dialog, onExit); });
            dialog.AddPossibleAnswer("I think we're on good terms. Maybe we can do more to help eachother out.", () => { QA4(dialog, onExit); });
        }

        private void QA3(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion(
               "Agent 1: Right\n" +
               "Agent 2: What about the work and code itself, is there any reason to suspect any of your colleagues of working with VI?");
            dialog.AddPossibleAnswer("Not that I'm aware of.", () => { QA5(dialog, onExit); });
            dialog.AddPossibleAnswer("I'd rather not speculate, maybe you should ask them instead.", () => { QA6(dialog, onExit); });
        }

        private void QA4(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion(
                "Agent 2: We can definitely talk about that.\n" +
                "Agent 1: How about giving us some actual answers first, then maybe we'll talk. Like where you were yesterday? Planting explosives in Dome 1?");
            dialog.AddPossibleAnswer("I was here all day at work and at home, you can check my transport logs.", () => { QA7(dialog, onExit); });
            dialog.AddPossibleAnswer("Was it explosives? That's horrible, I had no idea.", () => { QA8(dialog, onExit); });
        }

        private void QA5(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion(
                "Agent 1: Or you're just defending your VI friends.\n" +
                "Agent 2: I think we have enough for now. Maybe for your troubles we can arrange a free ticket offworld, terrible sorry for the inconvenience.");
            dialog.AddPossibleAnswer(
                "I assure you I have no knowledge of VI. And I'll gladly accept your offer.",
                () => { LeaveInterrogation(true, onExit); });
        }

        private void QA6(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion(
                "Agent 1: You can be assured that we do, I'm sure that they can confirm our suspicions of you.\n" +
                "Agent 2: Thank you for your help, we have no further questions.");
            dialog.AddPossibleAnswer(
                "Alright",
                () => { LeaveInterrogation(false, onExit); });
        }

        private void QA7(IDialogConsole dialog, Action onExit)
        {
            var settings = CharacterSettings.Instance();

            dialog.NewQuestion(
                "Agent 1: So you already erased them from our systems.\n" +
                "Agent 2: I'm sure " + settings.Name + " is telling us the truth. I think we have no further questions.");
            dialog.AddPossibleAnswer(
                "Ok",
                () => { LeaveInterrogation(false, onExit); });
        }

        private void QA8(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion(
                "Agent 2: Quite terrible, it's hard to imagine such violence against your own fellow inhabitants.\n" +
                "Agent 1: A delusional bunch. I'm thinking of leaving this place, maybe resettle on the Moon. Have you ever been there?");
            dialog.AddPossibleAnswer("No, I haven't. I like it here.", () => { QA9(dialog, onExit); });
            dialog.AddPossibleAnswer("No, I haven't. But I'm thinking about visiting it", () => { QA10(dialog, onExit); });
        }

        private void QA9(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion(
                "Agent 2: I hear Ceres is pretty interesting to visit. If only for a small holiday.\n" +
                "Agent 1: Definitely. Some might find it suspicious if someone doesn't get out of their home once in awhile.");
            dialog.AddPossibleAnswer(
                "Of course, perhaps I should.",
                () => { LeaveInterrogation(false, onExit); });
        }

        private void QA10(IDialogConsole dialog, Action onExit)
        {
            dialog.NewQuestion("Agent 2: Oh you definitely should, maybe we can give you a free launch ticket. We know you have some days off next week.");
            dialog.AddPossibleAnswer(
                "Thank you.",
                () => { LeaveInterrogation(true, onExit); });
        }
    }
}
