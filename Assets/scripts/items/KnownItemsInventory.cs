﻿namespace PlanetaryDeception
{
    /// <summary>
    /// Enum of KnownItem id's in the game
    /// </summary>
    public enum KnownItem
    {
        BubblegumWrappingPaper,
        PlayerSecurityAccessCard,
        IOTVenusHouseLockControl,
        VoasisCredentials,
        VoasisWebsiteCredentialsUsage,
        VenrefInterrogated,
        CeresInvitation,
        VenusLaunchAssistanceTicket,
        Chapter2
    }

    /// <summary>
    /// Known Items Inventory.
    /// </summary>
    public class KnownItemsInventory : ItemInventory
    {
        /// <summary>
        /// Singleton Instance.
        /// </summary>
        private static KnownItemsInventory thisInstance = null;

        /// <summary>
        /// Known Items Inventory.
        /// </summary>
        public KnownItemsInventory()
        {
            Add(new ItemTag(KnownItem.BubblegumWrappingPaper, ItemClassType.Trash, "Bubblegum wrapping paper"));
            Add(new ItemTag(KnownItem.PlayerSecurityAccessCard, ItemClassType.Identification, "Security access card"));
            Add(new ItemTag(KnownItem.IOTVenusHouseLockControl, ItemClassType.Hidden, string.Empty));
            Add(new ItemTag(KnownItem.VoasisCredentials, ItemClassType.Hidden, string.Empty));
            Add(new ItemTag(KnownItem.VoasisWebsiteCredentialsUsage, ItemClassType.Hidden, string.Empty));
            Add(new ItemTag(KnownItem.VenrefInterrogated, ItemClassType.Hidden, string.Empty));
            Add(new ItemTag(KnownItem.CeresInvitation, ItemClassType.Hidden, string.Empty));
            Add(new ItemTag(KnownItem.VenusLaunchAssistanceTicket, ItemClassType.Ticket, "Ticket: Venus Launch Assistance"));
            Add(new ItemTag(KnownItem.Chapter2, ItemClassType.Hidden, string.Empty));
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
        public override void TransferItem(KnownItem itemId, ItemInventory destinationInventory)
        {
            var item = Peek(itemId);
            destinationInventory.Add(item);
        }
    }
}
