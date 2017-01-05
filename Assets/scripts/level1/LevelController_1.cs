namespace PlanetaryDeception
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

            // note: this is test code
            var inventory = PlayerInventory.Instance();

            if (Input.GetButton("Fire1"))
            {
                if (PlayerIsTouching("Dressoir"))
                {
                    if (!inventory.ContainsItem(KnownItemsInventory.BubbleGumWrappingPaper))
                    {
                        var knownItems = KnownItemsInventory.Instance();
                        knownItems.TransferItem(KnownItemsInventory.BubbleGumWrappingPaper, inventory);
                        AlertText.text = "You picked up BubbleGumWrappingPaper";
                    }
                }
                else if (PlayerIsTouching("Console"))
                {
                    AlertText.text = "You have no access to this terminal";
                }
                else if (PlayerIsTouching("Door"))
                {
                    SceneManager.LoadScene("Level_2");
                }
            }
        }
    }
}
