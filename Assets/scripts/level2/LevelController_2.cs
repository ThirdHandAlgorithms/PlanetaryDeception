namespace PlanetaryDeception
{
    using UnityEngine;

    /// <summary>
    /// Level 2
    /// </summary>
    public class LevelController_2 : LevelBase
    {
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
                else if (PlayerIsTouching("Flowershop"))
                {
                    AlertText.text = "The shopkeeper is not here";
                }
            }
        }
    }
}