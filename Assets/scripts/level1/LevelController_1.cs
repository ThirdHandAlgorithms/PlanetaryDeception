namespace PlanetaryDeception
{
    using UnityEngine;

    /// <summary>
    /// Level 1
    /// </summary>
    public class LevelController_1 : LevelBase
    {
        /// <summary>
        /// Update Event handler
        /// </summary>
        public void Update()
        {
            currentInstance = this;

            var playerInventory = PlayerInventory.Instance();

            if (Input.GetButton("Fire1"))
            {
                if (PlayerIsTouching("Dressoir"))
                {
                    if (!playerInventory.ContainsItem(KnownItem.PlayerSecurityAccessCard))
                    {
                        var knownItems = KnownItemsInventory.Instance();
                        knownItems.TransferItem(KnownItem.PlayerSecurityAccessCard, playerInventory);
                        AlertText.text = "You picked up " + knownItems.GetName(KnownItem.PlayerSecurityAccessCard);
                    }
                }
                else if (PlayerIsTouching("Console"))
                {
                    if (!playerInventory.ContainsItem(KnownItem.PlayerSecurityAccessCard))
                    {
                        AlertText.text = "You have no access to this terminal";
                    }
                    else
                    {
                        var settings = CharacterSettings.Instance();
                        settings.TransitionToNewScene("Level_1_console", Player);
                        return;
                    }
                }
                else if (PlayerIsTouching("Door"))
                {
                    if (playerInventory.ContainsItem(KnownItem.IOTVenusHouseLockControl))
                    {
                        if (playerInventory.ContainsItem(KnownItem.VoasisWebsiteCredentialsUsage) && !playerInventory.ContainsItem(KnownItem.VenrefInterrogated))
                        {
                            var settings = CharacterSettings.Instance();
                            settings.TransitionToNewScene("Level_2_interrogation", Player);
                        }
                        else
                        {
                            var settings = CharacterSettings.Instance();
                            settings.TransitionToNewScene("Level_2", Player);
                        }
                        
                        return;
                    }
                    else
                    {
                        AlertText.text = "Door is locked";
                    }
                }
            }
        }
    }
}
