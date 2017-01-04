namespace PlanetaryDeception
{
    using System.Collections.Generic;

    /// <summary>
    /// Item Inventory.
    /// </summary>
    public class ItemInventory
    {
        /// <summary>
        /// Inventory items list.
        /// </summary>
        private List<ItemTag> items;

        /// <summary>
        /// Item Inventory.
        /// </summary>
        public ItemInventory()
        {
            items = new List<ItemTag>();
        }

        /// <summary>
        /// Gets or Sets Items.
        /// </summary>
        protected List<ItemTag> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }

        /// <summary>
        /// Checks if an Item is in the Inventory (by Item ID).
        /// </summary>
        /// <param name="itemId">Item ID</param>
        /// <returns>bool</returns>
        public bool ContainsItem(int itemId)
        {
            foreach (var item in items)
            {
                if (item.ItemID == itemId)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Pulls an Item out of the Inventory, returns the retreived Item.
        /// If the Item is not in the Inventory, an exception is thrown.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>ItemTag</returns>
        public ItemTag Pull(int itemId)
        {
            var item = Peek(itemId);
            if (item == null)
            {
                throw new ItemException("Yo, something's wrong", itemId);
            }

            items.Remove(item);

            return item;
        }

        /// <summary>
        /// Transfers an Item from this inventory to another one
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="destinationInventory"></param>
        public void TransferItem(int itemId, ItemInventory destinationInventory)
        {
            var item = Pull(itemId);
            destinationInventory.Add(item);
        }

        /// <summary>
        /// Adds an existing Item to the Inventory
        /// </summary>
        /// <param name="itemTag"></param>
        public void Add(ItemTag itemTag)
        {
            items.Add(itemTag);
        }

        /// <summary>
        /// Gets the ItemTag for a given ItemID if it's in the Inventory, otherwise null is returned
        /// </summary>
        /// <param name="itemId">int</param>
        /// <returns>null|ItemTag</returns>
        protected ItemTag Peek(int itemId)
        {
            foreach (var item in Items)
            {
                if (item.ItemID == itemId)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
