namespace PlanetaryTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PlanetaryDeception;

    [TestClass]
    public class InventoryTests
    {
        [TestMethod]
        public void InventoryCreation()
        {
            var inv = new ItemInventory();
        }

        [TestMethod]
        public void InventoryAdd()
        {
            var inv = new ItemInventory();
            var known = KnownItemsInventory.Instance();

            known.TransferItem(KnownItem.BubblegumWrappingPaper, inv);

            Assert.IsTrue(inv.ContainsItem(KnownItem.BubblegumWrappingPaper));
            Assert.IsTrue(known.ContainsItem(KnownItem.BubblegumWrappingPaper));
        }

        [TestMethod]
        public void InventoryNotAdded()
        {
            var inv = new ItemInventory();

            Assert.IsFalse(inv.ContainsItem(KnownItem.BubblegumWrappingPaper));
        }

        [TestMethod]
        public void InventoryRemove()
        {
            var inv = new ItemInventory();
            var known = KnownItemsInventory.Instance();

            known.TransferItem(KnownItem.BubblegumWrappingPaper, inv);

            inv.TransferItem(KnownItem.BubblegumWrappingPaper, new ItemInventory());

            Assert.IsFalse(inv.ContainsItem(KnownItem.BubblegumWrappingPaper));
        }

        [TestMethod]
        public void InventoryContainsOne()
        {
            var inv = new ItemInventory();
            var known = KnownItemsInventory.Instance();

            known.TransferItem(KnownItem.BubblegumWrappingPaper, inv);
            known.TransferItem(KnownItem.StationTransportTicketDome7, inv);

            Assert.IsTrue(inv.ContainsOneItem(new KnownItem []{ KnownItem.StationTransportTicketDome7}));

            Assert.IsTrue(inv.ContainsOneItem(new KnownItem[] { KnownItem.StationTransportTicketDome7, KnownItem.BubblegumWrappingPaper }));

            Assert.IsTrue(inv.ContainsOneItem(new KnownItem[] { KnownItem.StationTransportTicketDome6, KnownItem.BubblegumWrappingPaper }));

            Assert.IsFalse(inv.ContainsOneItem(new KnownItem[] { KnownItem.StationTransportTicketDome6, KnownItem.StationTransportTicketDome1 }));

            Assert.IsFalse(inv.ContainsOneItem(new KnownItem[] { KnownItem.StationTransportTicketDome6 }));
        }
    }
}
