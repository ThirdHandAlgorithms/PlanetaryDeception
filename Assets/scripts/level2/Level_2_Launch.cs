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
                        KnownItemsInventory.Instance().TransferItem(KnownItem.Chapter2, playerInventory);

                        SceneManager.LoadScene("Level_EndOfDemo");
                        return;
                    }
                    else
                    {
                        AlertText.text = "You require a Launch Assistance ticket";
                    }
                }
                else if (PlayerIsTouching("StationTransport"))
                {
                    if (playerInventory.ContainsItem(KnownItem.StationTransportTicketDome1))
                    {
                        SceneManager.LoadScene("Level_3");
                        return;
                    }
                    else if (playerInventory.ContainsItem(KnownItem.StationTransportTicketDome2))
                    {
                        AlertText.text = "You're already on Station Dome 2";
                    }
                    else if (playerInventory.ContainsOneItem(new KnownItem[]
                    {
                        KnownItem.StationTransportTicketDome3,
                        KnownItem.StationTransportTicketDome4,
                        KnownItem.StationTransportTicketDome5,
                        KnownItem.StationTransportTicketDome6,
                        KnownItem.StationTransportTicketDome7,
                        KnownItem.StationTransportTicketDome8
                    }))
                    {
                        AlertText.text = "Travel temporarily suspended";
                    }
                    else
                    {
                        AlertText.text = "You require a Station Transport ticket";
                    }
                }
            }
        }
    }
}
