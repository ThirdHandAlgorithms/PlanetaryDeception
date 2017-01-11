namespace PlanetaryDeception
{
    using System;

    /// <summary>
    /// Virtual Wallet of Solarbits
    /// </summary>
    public class SolarbitsWallet
    {
        /// <summary>
        /// Total amount of Solarbits
        /// </summary>
        protected int amount;

        /// <summary>
        /// Constructor
        /// </summary>
        public SolarbitsWallet()
        {
            amount = 0;
        }

        /// <summary>
        /// Transfer an amount to the destination wallet. Note: if amount is not available - an exception is raised
        /// </summary>
        /// <param name="amountToTransfer"></param>
        /// <param name="destinationWallet"></param>
        public void Transfer(int amountToTransfer, SolarbitsWallet destinationWallet)
        {
            if (amount < amountToTransfer)
            {
                throw new Exception("Not enough solarbits");
            }

            amount -= amountToTransfer;
            destinationWallet.Add(amountToTransfer);
        }

        /// <summary>
        /// Add an amount to this wallet
        /// </summary>
        /// <param name="amountToAdd"></param>
        public void Add(int amountToAdd)
        {
            amount += amountToAdd;
        }

        /// <summary>
        /// Returns the total amount of bits in this wallet
        /// </summary>
        /// <returns>int</returns>
        public int GetAmount()
        {
            return amount;
        }
    }
}
