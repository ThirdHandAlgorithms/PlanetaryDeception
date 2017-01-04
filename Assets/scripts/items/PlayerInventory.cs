namespace PlanetaryDeception
{
    /// <summary>
    /// Player Inventory.
    /// </summary>
    class PlayerInventory : ItemInventory
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static PlayerInventory thisInstance = null;

        /// <summary>
        /// Singleton Instance.
        /// </summary>
        /// <returns>Singleton of the Instance</returns>
        public static PlayerInventory Instance()
        {
            if (thisInstance == null)
            {
                thisInstance = new PlayerInventory();
            }

            return thisInstance;
        }
    }
}
