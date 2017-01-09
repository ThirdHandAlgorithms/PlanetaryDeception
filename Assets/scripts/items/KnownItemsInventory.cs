namespace PlanetaryDeception
{
    /// <summary>
    /// Known Items Inventory.
    /// </summary>
    public class KnownItemsInventory : ItemInventory
    {
        /// <summary>
        /// Personal access card to terminals
        /// </summary>
        public const int PlayerSecurityAccessCard = 1;

        /// <summary>
        /// Hidden lock control
        /// </summary>
        public const int IOTVenusHouseLockControl = 2;

        /// <summary>
        /// Singleton Instance.
        /// </summary>
        private static KnownItemsInventory thisInstance = null;

        /// <summary>
        /// Known Items Inventory.
        /// </summary>
        public KnownItemsInventory()
        {
            Add(new ItemTag(PlayerSecurityAccessCard, ItemClassType.Identification, "Security access card"));
            Add(new ItemTag(IOTVenusHouseLockControl, ItemClassType.HiddenIOTControl, string.Empty));
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
        public override void TransferItem(int itemId, ItemInventory destinationInventory)
        {
            var item = Peek(itemId);
            destinationInventory.Add(item);
        }
    }
}
