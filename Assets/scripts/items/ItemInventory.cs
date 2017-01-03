using System;
using System.Collections.Generic;

namespace PlanetaryDeception
{
    public class ItemInventory
    {
        protected List<ItemTag> Items;

        public ItemInventory()
        {
            Items = new List<ItemTag>();
        }

        /// <summary>
        /// Checks if an Item is in the Inventory (by Item ID)
        /// </summary>
        /// <param name="AItemID"></param>
        /// <returns>bool</returns>
        public bool ContainsItem(int AItemID)
        {
            foreach (var Item in Items)
            {
                if (Item.ItemID == AItemID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the ItemTag for a given ItemID if it's in the Inventory, otherwise null is returned
        /// </summary>
        /// <param name="AItemID"></param>
        /// <returns>null|ItemTag</returns>
        protected ItemTag Peek(int AItemID)
        {
            foreach (var Item in Items)
            {
                if (Item.ItemID == AItemID)
                {
                    return Item;
                }
            }

            return null;
        }

        /// <summary>
        /// Pulls an Item out of the Inventory, returns the retreived Item.
        /// If the Item is not in the Inventory, an exception is thrown.
        /// </summary>
        /// <param name="AItemID"></param>
        /// <returns>ItemTag</returns>
        public ItemTag Pull(int AItemID)
        {
            var Item = Peek(AItemID);
            if (Item == null)
            {
                throw new ItemException("Yo, something's wrong", AItemID);
            }

            Items.Remove(Item);

            return Item;
        }

        /// <summary>
        /// Transfers an Item from this inventory to another one
        /// </summary>
        /// <param name="AItemID"></param>
        /// <param name="ADestinationInventory"></param>
        public void TransferItem(int AItemID, ItemInventory ADestinationInventory)
        {
            var Item = Pull(AItemID);
            ADestinationInventory.Add(Item);
        }

        /// <summary>
        /// Adds an existing Item to the Inventory
        /// </summary>
        /// <param name="AItem"></param>
        public void Add(ItemTag AItem)
        {
            Items.Add(AItem);
        }
    };
}
