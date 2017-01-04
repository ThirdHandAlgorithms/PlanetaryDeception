namespace PlanetaryDeception
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Level 1
    /// </summary>
    public class LevelController_1 : MonoBehaviour
    {
        /// <summary>
        /// Connect to Text gameobject in Unity
        /// </summary>
        public Text AlertText;

        /// <summary>
        /// Update Event handler
        /// </summary>
        public void Update()
        {
            // note: this is test code
            var inventory = PlayerInventory.Instance();

            if (Input.anyKey)
            {
                if (!inventory.ContainsItem(KnownItemsInventory.BubbleGumWrappingPaper))
                {
                    var knownItems = KnownItemsInventory.Instance();
                    knownItems.TransferItem(KnownItemsInventory.BubbleGumWrappingPaper, inventory);
                }
            }

            if (inventory.ContainsItem(KnownItemsInventory.BubbleGumWrappingPaper))
            {
                AlertText.text = "You have BubbleGumWrappingPaper";
            }
        }
    }
}
