namespace PlanetaryDeception
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// web browser app
    /// </summary>
    public class SolarOSWeb : SolarOSApplication
    {
        /// <summary>
        /// bookmarks
        /// </summary>
        private List<SolarOSMenuItem> bookmarks;

        /// <summary>
        /// login page
        /// </summary>
        private List<SolarOSMenuItem> loginpage;

        /// <summary>
        /// Current website
        /// </summary>
        private string currentWebsite;

        /// <summary>
        /// Current Webpage text
        /// </summary>
        private string currentWebContent;

        /// <summary>
        /// Login Webpage text
        /// </summary>
        private string loginPageContent;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="os"></param>
        public SolarOSWeb(SolarOS os) : base(os)
        {
            currentWebContent = string.Empty;

            InitBookmarks();
            InitLogin();
        }

        /// <summary>
        /// display
        /// </summary>
        public override void RefreshDisplay()
        {
            InitSelection();

            string description = string.Empty;
            if (IsInMainMenu())
            {
                description = "bookmarks:\n\n";
            }

            string appTitle = "secure web";
            if (currentWebsite != string.Empty)
            {
                appTitle += " - " + currentWebsite;
            }

            parentOS.SetConsoleText(
                parentOS.OSTxt(appTitle) +
                description +
                currentWebContent +
                MenuOptionsTxt()
            );
        }

        /// <summary>
        /// init
        /// </summary>
        public override void Run()
        {
            if (parentOS.CurrentApplication.Description == "api.voasis.venus")
            {
                currentWebsite = parentOS.CurrentApplication.Description;
                currentWebContent = loginPageContent;

                parentOS.MenuItems = loginpage;
            }
            else if (bookmarks.Contains(parentOS.CurrentApplication))
            {
                parentOS.MenuItems = new List<SolarOSMenuItem>();

                LoadTextFileAsWebPage(parentOS.CurrentApplication.Description);

                if (currentWebsite == "tickets.venus")
                {
                    var playerInventory = PlayerInventory.Instance();

                    if (!playerInventory.ContainsItem(KnownItem.StationTransportTicketDome1))
                    {
                        StationTransportTicketOption(playerInventory);
                    }

                    if (!playerInventory.ContainsItem(KnownItem.VenusLaunchAssistanceTicket))
                    {
                        LaunchAssistanceTicketOption(playerInventory);
                    }
                }
            }
            else
            {
                currentWebsite = string.Empty;
                currentWebContent = string.Empty;
                parentOS.MenuItems = bookmarks;
            }
        }

        /// <summary>
        /// Returns MenuOptions as a string
        /// </summary>
        /// <returns>string</returns>
        protected override string MenuOptionsTxt()
        {
            if (IsInMainMenu())
            {
                return base.MenuOptionsTxt();
            }
            else
            {
                string menuOptions = string.Empty;

                foreach (var menuItem in parentOS.MenuItems)
                {
                    if (!menuItem.IsEnabled)
                    {
                        menuOptions += "[-] " + menuItem.Description + "\n";
                    }
                    else if (parentOS.SelectedMenuItem == menuItem)
                    {
                        menuOptions += "[x] " + menuItem.Description + "\n";
                    }
                    else
                    {
                        menuOptions += "[ ] " + menuItem.Description + "\n";
                    }
                }

                return menuOptions;
            }
        }

        /// <summary>
        /// Adds option for a Station Transport ticket
        /// </summary>
        /// <param name="playerInventory"></param>
        private void StationTransportTicketOption(PlayerInventory playerInventory)
        {
            var playerWallet = PlayerWallet.Instance();
            var ticketprice = 10;
            if (playerInventory.ContainsItem(KnownItem.StationTransportUnlocked) && (playerWallet.GetAmount() >= ticketprice))
            {
                parentOS.MenuItems.Add(
                    new SolarOSMenuItem(
                        "buy station transport ticket to dome 1 (" + ticketprice + " solarbits)",
                        () =>
                        {
                            playerWallet.Transfer(ticketprice, new SolarbitsWallet());
                            KnownItemsInventory.Instance().TransferItem(KnownItem.StationTransportTicketDome1, playerInventory);

                            parentOS.CurrentApplication = parentOS.PopPreviousApplicationBreadcrumb();

                            Run();
                        },
                        RefreshDisplay));
            }
            else
            {
                parentOS.MenuItems.Add(new SolarOSMenuItem("buy station transport ticket (" + ticketprice + " solarbits)\n", false));
            }
        }

        /// <summary>
        /// Adds option for a Launch Assistance ticket
        /// </summary>
        /// <param name="playerInventory"></param>
        private void LaunchAssistanceTicketOption(PlayerInventory playerInventory)
        {
            var playerWallet = PlayerWallet.Instance();
            var ticketprice = 100;
            if (playerInventory.ContainsItem(KnownItem.VenrefInterrogated) && (playerWallet.GetAmount() >= ticketprice))
            {
                parentOS.MenuItems.Add(
                    new SolarOSMenuItem(
                        "buy launch assistance ticket (" + ticketprice + " solarbits)",
                        () =>
                        {
                            playerWallet.Transfer(ticketprice, new SolarbitsWallet());
                            KnownItemsInventory.Instance().TransferItem(KnownItem.VenusLaunchAssistanceTicket, playerInventory);

                            parentOS.CurrentApplication = parentOS.PopPreviousApplicationBreadcrumb();

                            Run();
                        },
                        RefreshDisplay));
            }
            else
            {
                parentOS.MenuItems.Add(new SolarOSMenuItem("buy launch assistance ticket (" + ticketprice + " solarbits)\n", false));
            }
        }

        /// <summary>
        /// Inits bookmarks
        /// </summary>
        private void InitBookmarks()
        {
            bookmarks = new List<SolarOSMenuItem>();

            if (parentOS.NetworkIsOnVenus())
            {
                bookmarks.Add(new SolarOSMenuItem("news.venus", this));
                bookmarks.Add(new SolarOSMenuItem("tickets.venus", this));
                bookmarks.Add(new SolarOSMenuItem("vite.venus", this));
                bookmarks.Add(new SolarOSMenuItem("venref.venus", this));
                bookmarks.Add(new SolarOSMenuItem("voasis.venus", this));
                bookmarks.Add(new SolarOSMenuItem("api.voasis.venus", this));
            }
        }

        /// <summary>
        /// Inits login menu items
        /// </summary>
        private void InitLogin()
        {
            loginpage = new List<SolarOSMenuItem>();

            var playerInventory = PlayerInventory.Instance();
            if (parentOS.NetworkIsOnVenus() && playerInventory.ContainsItem(KnownItem.VoasisCredentials))
            {
                loginPageContent =
                    "username: admin\n" +
                    "password: ***\n";
                loginpage.Add(new SolarOSMenuItem(
                    "login",
                    () =>
                    {
                        KnownItemsInventory.Instance().TransferItem(KnownItem.VoasisWebsiteCredentialsUsage, playerInventory);
                        parentOS.MenuItems = new List<SolarOSMenuItem>();
                        LoadTextFileAsWebPage(currentWebsite);
                    },
                    RefreshDisplay));
            }
            else
            {
                loginPageContent =
                    "username:\n" +
                    "password:\n";

                loginpage.Add(new SolarOSMenuItem("Need login credentials", false));
            }
        }

        /// <summary>
        /// IsInMainMenu
        /// </summary>
        /// <returns>bool</returns>
        private bool IsInMainMenu()
        {
            return parentOS.MenuItems == bookmarks;
        }

        /// <summary>
        /// Is on a login page
        /// </summary>
        /// <returns>bool</returns>
        private bool IsOnLoginpage()
        {
            return parentOS.MenuItems == loginpage;
        }

        /// <summary>
        /// Init menuitem selection
        /// </summary>
        private void InitSelection()
        {
            if (IsInMainMenu())
            {
                parentOS.InitSelection();
            }
            else if (parentOS.MenuItems.Count > 0)
            {
                parentOS.InitSelection();
            }
            else
            {
                parentOS.SelectedMenuItem = null;
            }
        }

        /// <summary>
        /// Loads a textfile from the resources as a menu
        /// </summary>
        /// <param name="resourceName">string</param>
        private void LoadTextFileAsWebPage(string resourceName)
        {
            currentWebsite = resourceName;

            currentWebContent = string.Empty;

            var scriptText = Resources.Load(resourceName) as TextAsset;
            var stringSeparators = new string[] { "\r\n", "\r", "\n" };
            var result = scriptText.text.Split(stringSeparators, StringSplitOptions.None);

            foreach (var line in result)
            {
                currentWebContent += line.ToLower() + "\n";
            }
        }
    }
}
