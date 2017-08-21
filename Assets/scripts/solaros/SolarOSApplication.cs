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
        protected ISolarOS parentOS;

        public int CursorRow { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="os"></param>
        public SolarOSApplication(ISolarOS os)
        {
            parentOS = os;
            CursorRow = 0;
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
            int rowIdx = 0;

            foreach (var menuItem in parentOS.MenuItems)
            {
                if (menuItem == parentOS.SelectedMenuItem)
                {
                    menuOptions += "[x]";
                    CursorRow = rowIdx;
                }
                else
                {
                    menuOptions += "[ ]";
                }

                menuOptions += " " + menuItem.Description + "\n";

                rowIdx++;
            }

            return menuOptions;
        }
    }
}
