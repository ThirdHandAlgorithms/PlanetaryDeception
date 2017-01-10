namespace PlanetaryDeception
{
    /// <summary>
    /// The player's wallet
    /// </summary>
    public class PlayerWallet : SolarbitsWallet
    {
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

                // todo: load from SaveGame or initialize to 500 on NewGame
                thisInstance.Add(500);
            }

            return thisInstance;
        }
    }
}
