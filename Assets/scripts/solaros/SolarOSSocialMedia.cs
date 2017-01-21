namespace PlanetaryDeception
{
    using System.Collections.Generic;

    /// <summary>
    /// Social Media App
    /// </summary>
    public class SolarOSSocialMedia : SolarOSApplication
    {
        /// <summary>
        /// List of DM's as menu items
        /// </summary>
        private List<SolarOSMenuItem> directMessages;

        /// <summary>
        /// List of public messages as menu items
        /// </summary>
        private List<SolarOSMenuItem> publicMessages;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="os"></param>
        public SolarOSSocialMedia(ISolarOS os) : base(os)
        {
            InitDMs();
            InitPublic();
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public override void RefreshDisplay()
        {
            InitSelection();

            parentOS.SetConsoleText(
                parentOS.OSTxt("social media") +
                MenuOptionsTxt()
            );

            if (IsLookingAtDMs())
            {
                var playerInventory = PlayerInventory.Instance();
                if (playerInventory.ContainsItem(KnownItem.VenrefInterrogated))
                {
                    KnownItemsInventory.Instance().TransferItem(KnownItem.CeresInvitation, playerInventory);
                }
            }
        }

        /// <summary>
        /// Initializes menu itemss
        /// </summary>
        public override void Run()
        {
            if (IsLookingAtDMs())
            {
                parentOS.MenuItems = directMessages;
            }
            else
            {
                parentOS.MenuItems = publicMessages;
            }
        }

        /// <summary>
        /// Returns list of menuitems as a string
        /// </summary>
        /// <returns>string</returns>
        protected override string MenuOptionsTxt()
        {
            string menuOptions = string.Empty;

            foreach (var menuItem in parentOS.MenuItems)
            {
                if (menuItem == publicMessages[0])
                {
                    menuOptions += "[x] ";
                }

                menuOptions += menuItem.Description + "\n\n";
            }

            return menuOptions;
        }

        /// <summary>
        /// CurrentApp = dms?
        /// </summary>
        /// <returns>bool</returns>
        private bool IsLookingAtDMs()
        {
            return parentOS.CurrentApplication == publicMessages[0];
        }

        /// <summary>
        /// Init DM's
        /// </summary>
        private void InitDMs()
        {
            directMessages = new List<SolarOSMenuItem>();

            if ((parentOS.Network & SolarOSNetwork.Venus) > 0)
            {
                InitVenusDMs();
            }
        }

        /// <summary>
        /// DM's available from Venus consoles
        /// </summary>
        private void InitVenusDMs()
        {
            var playerInventory = PlayerInventory.Instance();

            directMessages.Add(new SolarOSMenuItem(
                "@drudith\n" +
                "if you ever go to mars, you should contact @messanor\n" +
                "they have some interesting info on that thing we talked about last week."));

            if (playerInventory.ContainsItem(KnownItem.VenrefInterrogated))
            {
                directMessages.Add(new SolarOSMenuItem(
                    "@cyranocereal [relayed]\n" +
                    "hey, didn't you have some days off this week?\n" +
                    "you should visit me on ceres,\n" +
                    "managed to get my hands on some disturbing docs."));
            }
        }

        /// <summary>
        /// Init public messages
        /// </summary>
        private void InitPublic()
        {
            publicMessages = new List<SolarOSMenuItem>();

            publicMessages.Add(new SolarOSMenuItem("Direct messages", Run, RefreshDisplay));

            if ((parentOS.Network & SolarOSNetwork.Venus) > 0)
            {
                InitVenusPublic();
            }
        }

        /// <summary>
        /// Public shares on Venus
        /// </summary>
        private void InitVenusPublic()
        {
            publicMessages.Add(new SolarOSMenuItem(
                "@drudith\n" +
                "found out from friend on moon there's an update to git, but hasn't been cached yet by venus downlink, fml"));
            publicMessages.Add(new SolarOSMenuItem(
                "@helawyr\n" +
                "yet another day with more bad news from @voasis - o2 supply bottleneck caused by unknown malfunction"));
            publicMessages.Add(new SolarOSMenuItem(
                "@lexcant\n" +
                "sometimes i think all i need is wine and catnip, and then it hits me, this is indeed all i need"));
            publicMessages.Add(new SolarOSMenuItem(
                "@officialvnn\n" +
                "earth-moon sources say yearly progress report on earth climate recovery process will be as expected - no recovery within 10yrs. more at news.venus/climate"));
            publicMessages.Add(new SolarOSMenuItem(
                "@4thhand\n" +
                "check out the demo for our new game \"the perils of enceladus\" at fourth.venus/demo"));
            publicMessages.Add(new SolarOSMenuItem(
                "@redressavoir\n" +
                "apparently all the newly installed wind turbines on mars are malfunctioning? #typicalmartiantechnology"));
        }

        /// <summary>
        /// Init menuitem selection
        /// </summary>
        private void InitSelection()
        {
            if (parentOS.MenuItems.Contains(publicMessages[0]))
            {
                parentOS.SelectedMenuItem = publicMessages[0];
            }
            else
            {
                parentOS.SelectedMenuItem = null;
            }
        }
    }
}
