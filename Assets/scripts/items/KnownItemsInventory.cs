using System;

namespace PlanetaryDeception
{
    class KnownItemsInventory: ItemInventory
    {
        private static KnownItemsInventory ThisInstance = null;

        public const int BubbleGumWrappingPaper = 1;

        public KnownItemsInventory()
        {
            Add(new ItemTag(BubbleGumWrappingPaper, ItemClassType.Trash, "Bubble gum wrapping paper"));
        }

        public static KnownItemsInventory Instance()
        {
            if (ThisInstance == null)
            {
                ThisInstance = new KnownItemsInventory();
            }

            return ThisInstance;
        }

        /// <summary>
        /// Transfers an Item from this inventory to another one
        /// </summary>
        /// <param name="AItemID"></param>
        /// <param name="ADestinationInventory"></param>
        public void TransferItem(int AItemID, ItemInventory ADestinationInventory)
        {
            var Item = Peek(AItemID);
            ADestinationInventory.Add(Item);
        }
    };
}
