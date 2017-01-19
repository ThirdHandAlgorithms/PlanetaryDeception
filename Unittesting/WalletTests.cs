namespace PlanetaryTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PlanetaryDeception;

    [TestClass]
    public class WalletTests
    {
        [TestMethod]
        public void WalletCreation()
        {
            var wallet = new SolarbitsWallet();
            
            Assert.AreEqual(0, wallet.GetAmount());
        }

        [TestMethod]
        public void AddCurrency()
        {
            var wallet = new SolarbitsWallet();

            wallet.Add(123);
            Assert.AreEqual(123, wallet.GetAmount());

            wallet.Add(456);
            Assert.AreEqual(579, wallet.GetAmount());
        }

        [TestMethod]
        public void Transfer()
        {
            var wallet = new SolarbitsWallet();
            wallet.Add(123);

            var destinationWallet = new SolarbitsWallet();
            wallet.Transfer(23, destinationWallet);

            Assert.AreEqual(100, wallet.GetAmount());
            Assert.AreEqual(23, destinationWallet.GetAmount());

            wallet.Transfer(100, destinationWallet);
            Assert.AreEqual(0, wallet.GetAmount());

            Assert.AreEqual(123, destinationWallet.GetAmount());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Not enough solarbits")]
        public void TransferMoreThanYouHave()
        {
            var wallet = new SolarbitsWallet();
            wallet.Add(123);

            var destinationWallet = new SolarbitsWallet();
            wallet.Transfer(124, destinationWallet);
        }
    }
}
