namespace PlanetaryDeception
{
    /// <summary>
    /// Known Items Inventory.
    /// </summary>
    public class KnownItemsInventory : ItemInventory
    {
        /// <summary>
        /// I'm here to kick ass and chew bubblegum, and I got 1 BubbleGumWrappingPaper.
        /// </summary>
        public const int BubbleGumWrappingPaper = 1;

        /// <summary>
        /// Singleton Instance.
        /// </summary>
        private static KnownItemsInventory thisInstance = null;

        /// <summary>
        /// Known Items Inventory.
        /// </summary>
        public KnownItemsInventory()
        {
            Add(new ItemTag(BubbleGumWrappingPaper, ItemClassType.Trash, "Bubble gum wrapping paper"));
        }

        /// <summary>
        /// Singleton.
        /// </summary>
        /// <returns>Instance of KnownItemsInventory</returns>
        public static KnownItemsInventory Instance()
        {
            if (thisInstance == null)
            {
                thisInstance = new KnownItemsInventory();
            }

            return thisInstance;
        }

        /// <summary>
        /// Transfers an Item from this inventory to another one
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="destinationInventory"></param>
        public void TransferItem(int itemId, ItemInventory destinationInventory)
        {
            var item = Peek(itemId);
            destinationInventory.Add(item);
        }
    }
}
