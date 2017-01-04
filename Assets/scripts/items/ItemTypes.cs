namespace PlanetaryDeception
{
    /// <summary>
    /// Types of items we're supporting
    /// </summary>
    public enum ItemClassType
    {
        Trash = 0,
        Identification = 1,
        ProjectileWeapon = 2,
        ParticleWeapon = 3
    }

    /// <summary>
    /// Information about an Item
    /// </summary>
    public class ItemTag
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemClass"></param>
        /// <param name="itemName"></param>
        public ItemTag(int itemId, ItemClassType itemClass, string itemName)
        {
            ItemID = itemId;
            ItemClass = itemClass;
            ItemName = itemName;
        }

        /// <summary>
        /// Copies settings from another Item
        /// </summary>
        /// <param name="itemTag"></param>
        public ItemTag(ItemTag itemTag)
        {
            ItemID = itemTag.ItemID;
            ItemClass = itemTag.ItemClass;
            ItemName = itemTag.ItemName;
        }
        
        /// <summary>
        /// Unique identifier for the Item
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// The type of Item
        /// </summary>
        public ItemClassType ItemClass { get; set; }

        /// <summary>
        /// Name for the Item
        /// </summary>
        public string ItemName { get; set; }
    }
}
