namespace PlanetaryDeception
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public bool ContainsItem(KnownItem itemId)
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
        /// ContainsOneItem
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns>bool</returns>
        public bool ContainsOneItem(KnownItem[] itemIds)
        {
            var queryItems =
                from item in items
               where itemIds.Contains(item.ItemID)
              select item;

            return queryItems.Count<ItemTag>() > 0;
        }

        /// <summary>
        /// Pulls an Item out of the Inventory, returns the retreived Item.
        /// If the Item is not in the Inventory, an exception is thrown.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>ItemTag</returns>
        public ItemTag Pull(KnownItem itemId)
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
        public virtual void TransferItem(KnownItem itemId, ItemInventory destinationInventory)
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
            if (itemTag != null)
            {
                items.Add(itemTag);
            }
            else
            {
                throw new Exception("Hey, you're trying to add [null] to an inventory");
            }
        }

        /// <summary>
        /// Gets name of item
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>string</returns>
        public string GetName(KnownItem itemId)
        {
            var item = Peek(itemId);
            if (item == null)
            {
                throw new ItemException("Yo, something's wrong", itemId);
            }

            return item.ItemName;
        }

        /// <summary>
        /// Gets the ItemTag for a given ItemID if it's in the Inventory, otherwise null is returned
        /// </summary>
        /// <param name="itemId">int</param>
        /// <returns>null|ItemTag</returns>
        protected ItemTag Peek(KnownItem itemId)
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
