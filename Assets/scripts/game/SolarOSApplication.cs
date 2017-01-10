namespace PlanetaryDeception
{
    /// <summary>
    /// Base class for Applications
    /// </summary>
    public abstract class SolarOSApplication
    {
        /// <summary>
        /// Initialized by caller SolarOS
        /// </summary>
        protected SolarOS parentOS;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="os"></param>
        public SolarOSApplication(SolarOS os)
        {
            parentOS = os;
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public abstract void RefreshDisplay();

        /// <summary>
        /// Run
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Return menu header
        /// </summary>
        /// <returns>string</returns>
        protected virtual string MenuTxt()
        {
            return "Available functions:\n";
        }

        /// <summary>
        /// Returns MenuOptions as a string
        /// </summary>
        /// <returns>string</returns>
        protected virtual string MenuOptionsTxt()
        {
            string menuOptions = string.Empty;

            foreach (var menuItem in parentOS.MenuItems)
            {
                if (menuItem == parentOS.SelectedMenuItem)
                {
                    menuOptions += "[x]";
                }
                else
                {
                    menuOptions += "[ ]";
                }

                menuOptions += " " + menuItem.Description + "\n";
            }

            return menuOptions;
        }
    }
}
