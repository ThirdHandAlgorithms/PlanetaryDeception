namespace PlanetaryDeception
{
    /// <summary>
    /// The player's wallet
    /// </summary>
    public class PlayerWallet : SolarbitsWallet
    {
        /// <summary>
        /// Default starting amount for new games
        /// </summary>
        public const int StartingAmount = 500;

        /// <summary>
        /// singleton instance
        /// </summary>
        private static PlayerWallet thisInstance = null;

        /// <summary>
        /// get/create singleton instance
        /// </summary>
        /// <returns>PlayerWallet</returns>
        public static PlayerWallet Instance()
        {
            if (thisInstance == null)
            {
                thisInstance = new PlayerWallet();
                thisInstance.Add(StartingAmount);
            }

            return thisInstance;
        }
    }
}
