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
        /// constructor
        /// </summary>
        /// <param name="os"></param>
        public SolarOSWeb(SolarOS os) : base(os)
        {
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

            parentOS.ConsoleOutput.text =
                parentOS.OSTxt(appTitle) +
                description +
                MenuOptionsTxt();
        }

        /// <summary>
        /// init
        /// </summary>
        public override void Run()
        {
            if (parentOS.CurrentApplication.Description == "api.voasis.venus")
            {
                currentWebsite = parentOS.CurrentApplication.Description;

                parentOS.MenuItems = loginpage;
            }
            else if (bookmarks.Contains(parentOS.CurrentApplication))
            {
                LoadTextFileAsMenu(parentOS.CurrentApplication.Description);

                if (currentWebsite == "tickets.venus")
                {
                    var playerInventory = PlayerInventory.Instance();
                    if (!playerInventory.ContainsItem(KnownItem.VenrefInterrogated))
                    {
                        parentOS.MenuItems.Add(new SolarOSMenuItem("[ ] no tickets available, please check back later"));
                    }
                    else if (!playerInventory.ContainsItem(KnownItem.VenusLaunchAssistanceTicket))
                    {
                        var playerWallet = PlayerWallet.Instance();
                        var ticketprice = 100;
                        if (playerWallet.GetAmount() >= 100)
                        {
                            parentOS.MenuItems.Add(
                                new SolarOSMenuItem(
                                    "[x] buy launch assistance ticket (" + ticketprice + " solarbits)",
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
                            parentOS.MenuItems.Add(new SolarOSMenuItem("[ ] buy launch assistance ticket (" + ticketprice + " solarbits)"));
                        }
                    }
                }
            }
            else
            {
                currentWebsite = string.Empty;
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
            else if (IsOnLoginpage())
            {
                string menuOptions = string.Empty;

                foreach (var menuItem in parentOS.MenuItems)
                {
                    if ((parentOS.SelectedMenuItem == menuItem) && !menuItem.Description.StartsWith("[x]"))
                    {
                        menuOptions += "[x] " + menuItem.Description + "\n";
                    }
                    else
                    {
                        menuOptions += menuItem.Description + "\n";
                    }
                }

                return menuOptions;
            }
            else
            {
                string menuOptions = string.Empty;

                foreach (var menuItem in parentOS.MenuItems)
                {
                    menuOptions += menuItem.Description + "\n";
                }

                return menuOptions;
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
                loginpage.Add(new SolarOSMenuItem("username: admin"));
                loginpage.Add(new SolarOSMenuItem("password: ***"));
                loginpage.Add(new SolarOSMenuItem(
                    "[x] login",
                    () =>
                    {
                        KnownItemsInventory.Instance().TransferItem(KnownItem.VoasisWebsiteCredentialsUsage, playerInventory);
                        LoadTextFileAsMenu(currentWebsite);
                    },
                    RefreshDisplay));
            }
            else
            {
                loginpage.Add(new SolarOSMenuItem("username:"));
                loginpage.Add(new SolarOSMenuItem("password:"));
                loginpage.Add(new SolarOSMenuItem("Need login credentials"));
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
            else if (LastItemIsSelectable())
            {
                parentOS.SelectedMenuItem = GetLastMenuItem();
            }
            else
            {
                parentOS.SelectedMenuItem = null;
            }
        }

        /// <summary>
        /// Returns the last menuitem in the current menuitem list, if length 0 return null
        /// </summary>
        /// <returns>SolarOSMenuItem</returns>
        private SolarOSMenuItem GetLastMenuItem()
        {
            if (parentOS.MenuItems.Count > 0)
            {
                return parentOS.MenuItems[parentOS.MenuItems.Count - 1];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// If the last menu item has [x] in the description
        /// </summary>
        /// <returns>bool</returns>
        private bool LastItemIsSelectable()
        {
            if (parentOS.MenuItems.Count > 0)
            {
                return parentOS.MenuItems[parentOS.MenuItems.Count - 1].Description.StartsWith("[x]");
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Loads a textfile from the resources as a menu
        /// </summary>
        /// <param name="resourceName">string</param>
        private void LoadTextFileAsMenu(string resourceName)
        {
            currentWebsite = resourceName;

            var menu = new List<SolarOSMenuItem>();

            var scriptText = Resources.Load(resourceName) as TextAsset;
            var stringSeparators = new string[] { "\r\n", "\r", "\n" };
            var result = scriptText.text.Split(stringSeparators, StringSplitOptions.None);

            foreach (var line in result)
            {
                menu.Add(new SolarOSMenuItem(line.ToLower()));
            }

            parentOS.MenuItems = menu;
        }
    }
}
