namespace PlanetaryDeception
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Level 2
    /// </summary>
    public class Level_2_Launch : LevelBase
    {
        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            currentInstance = this;
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                var playerInventory = PlayerInventory.Instance();
                if (PlayerIsTouching("Console"))
                {
                    if (playerInventory.ContainsItem(KnownItem.PlayerSecurityAccessCard))
                    {
                        SceneManager.LoadScene("Level_2_launch_console");
                        return;
                    }
                    else
                    {
                        AlertText.text = "Access denied";
                    }
                }
                else if (PlayerIsTouching("ExitDoor"))
                {
                    SceneManager.LoadScene("Level_2");
                    return;
                }
                else if (PlayerIsTouching("EmptyLauncher"))
                {
                    AlertText.text = "There's no ship docked here";
                }
                else if (PlayerIsTouching("DockedSolarSailShip"))
                {
                    if (playerInventory.ContainsItem(KnownItem.VenusLaunchAssistanceTicket))
                    {
                        SceneManager.LoadScene("Level_EndOfDemo");
                        return;
                    }
                    else
                    {
                        AlertText.text = "You require a Launch Assistance ticket";
                    }
                }
            }
        }
    }
}
