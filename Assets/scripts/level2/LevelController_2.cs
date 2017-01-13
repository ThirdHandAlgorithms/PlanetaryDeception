namespace PlanetaryDeception
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

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
                        SceneManager.LoadScene("Level_1");
                        return;
                    }
                    else
                    {
                        AlertText.text = "Access denied, you need your Security Access card";
                    }
                }
                else if (PlayerIsTouching("LaunchEntrance"))
                {
                    SceneManager.LoadScene("Level_2_launch");
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