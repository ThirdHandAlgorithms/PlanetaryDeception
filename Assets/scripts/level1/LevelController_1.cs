﻿namespace PlanetaryDeception
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

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

            var inventory = PlayerInventory.Instance();

            if (Input.GetButton("Fire1"))
            {
                if (PlayerIsTouching("Dressoir"))
                {
                    if (!inventory.ContainsItem(KnownItemsInventory.PlayerSecurityAccessCard))
                    {
                        var knownItems = KnownItemsInventory.Instance();
                        knownItems.TransferItem(KnownItemsInventory.PlayerSecurityAccessCard, inventory);
                        AlertText.text = "You picked up " + knownItems.GetName(KnownItemsInventory.PlayerSecurityAccessCard);
                    }
                }
                else if (PlayerIsTouching("Console"))
                {
                    if (!inventory.ContainsItem(KnownItemsInventory.PlayerSecurityAccessCard))
                    {
                        AlertText.text = "You have no access to this terminal";
                    }
                    else
                    {
                        SceneManager.LoadScene("Level_1a");
                    }
                }
                else if (PlayerIsTouching("Door"))
                {
                    if (inventory.ContainsItem(KnownItemsInventory.IOTVenusHouseLockControl))
                    {
                        SceneManager.LoadScene("Level_2");
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
