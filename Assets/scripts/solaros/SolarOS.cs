namespace PlanetaryDeception
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// SolarOS
    /// </summary>
    public class SolarOS : LevelBase
    {
        /// <summary>
        /// The Text field for the OS output
        /// </summary>
        public Text ConsoleOutput;

        /// <summary>
        /// The scrolling view port for the ConsoleOutput
        /// </summary>
        public ScrollRect ConsoleScrollRect;

        /// <summary>
        /// Current Network the console is connected to
        /// </summary>
        public SolarOSNetwork Network;

        /// <summary>
        /// The currently running application
        /// </summary>
        public SolarOSMenuItem CurrentApplication;

        /// <summary>
        /// Allow automatic exit if you go back from the main menu
        /// </summary>
        public bool AllowExit = true;

        /// <summary>
        /// Allow to go back a menu with Fire2
        /// </summary>
        public bool AllowGoingBack = true;

        /// <summary>
        /// IsStarted
        /// </summary>
        public bool IsStarted { get; set; }

        /// <summary>
        /// The current by the user selected menu item
        /// </summary>
        public SolarOSMenuItem SelectedMenuItem;

        /// <summary>
        /// delay helper vars
        /// </summary>
        protected float nextInputAllowed = 0.0F;

        /// <summary>
        /// Breadcrumb path
        /// </summary>
        protected List<SolarOSMenuItem> breadCrumbs;

        /// <summary>
        /// Possible Networks
        /// </summary>
        public enum SolarOSNetwork
        {
            Space = 0,

            Venus = 1,
            VenusHome = 3,
            Venref = 5,

            EarthMoon = 8,

            Mars = 16,

            Ceres = 24,

            Europa = 32
        }

        /// <summary>
        /// Current menu's menuitems
        /// </summary>
        public List<SolarOSMenuItem> MenuItems { get; set; }

        /// <summary>
        /// Unity start
        /// </summary>
        public override void Start()
        {
            base.Start();

            breadCrumbs = new List<SolarOSMenuItem>();
            LoadMainMenu();

            IsStarted = true;
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            if (Time.time >= nextInputAllowed)
            {
                var verticalAxis = Input.GetAxis("Vertical");

                if (verticalAxis > 0)
                {
                    MoveSelectionUp();

                    nextInputAllowed = Time.time + 0.2F;
                }
                else if (verticalAxis < 0)
                {
                    MoveSelectionDown();

                    nextInputAllowed = Time.time + 0.2F;
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    var selected = GetSelectedMenuItem();
                    if (selected != null)
                    {
                        RunMenuItem(selected);
                    }

                    nextInputAllowed = Time.time + 0.2F;
                }
                else if (AllowGoingBack && Input.GetButtonUp("Fire2"))
                {
                    ScrollToTop();

                    var lastBreadcrumb = PopPreviousApplicationBreadcrumb();
                    if ((lastBreadcrumb == null) && (CurrentApplication == null))
                    {
                        if (AllowExit)
                        {
                            var currentScene = SceneManager.GetActiveScene();
                            var currentSceneName = currentScene.name;
                            var parentSceneName = currentSceneName.Substring(0, currentSceneName.LastIndexOf('_'));
                            var settings = CharacterSettings.Instance();
                            settings.TransitionToNewScene(parentSceneName, null);
                            return;
                        }
                    }
                    else
                    {
                        var reselectOption = CurrentApplication;

                        RunMenuItemWithoutAddingToBreadcrumbs(lastBreadcrumb);

                        SelectItemWithSameDescription(reselectOption);
                    }

                    nextInputAllowed = Time.time + 0.2F;
                }
            }

            if (CurrentApplication == null)
            {
                RefreshDisplay();
            }
            else if (CurrentApplication.OnDisplay != null)
            {
                CurrentApplication.OnDisplay();
            }
        }

        /// <summary>
        /// Sometimes we make new menuitems but stil want to select our previous selection
        /// </summary>
        /// <param name="reselectItem"></param>
        public void SelectItemWithSameDescription(SolarOSMenuItem reselectItem)
        {
            foreach (var item in MenuItems)
            {
                if (item.Description == reselectItem.Description)
                {
                    SelectedMenuItem = item;
                    return;
                }
            }
        }

        /// <summary>
        /// If network is venus/venushome/venref/etc
        /// </summary>
        /// <returns>bool</returns>
        public bool NetworkIsOnVenus()
        {
            return (Network & SolarOSNetwork.Venus) > 0;
        }

        /// <summary>
        /// Returns the latest breadcrumb
        /// </summary>
        /// <returns>SolarOSMenuItem</returns>
        public SolarOSMenuItem PopPreviousApplicationBreadcrumb()
        {
            if (breadCrumbs.Count > 0)
            {
                var idx = breadCrumbs.Count - 1;
                var item = breadCrumbs[idx];
                breadCrumbs.RemoveAt(idx);

                return item;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Appends a breadcrumb
        /// </summary>
        /// <param name="menuItem">SolarOSMenuItem</param>
        public void AddBreadCrumb(SolarOSMenuItem menuItem)
        {
            if (!breadCrumbs.Contains(menuItem))
            {
                breadCrumbs.Add(menuItem);
            }
        }

        /// <summary>
        /// Menu has items?
        /// </summary>
        /// <returns>bool</returns>
        public bool HasItems()
        {
            return (MenuItems != null) && (MenuItems.Count > 0);
        }

        /// <summary>
        /// Initialize selectedMenuItem for the current menu
        /// </summary>
        public void InitSelection()
        {
            if (HasItems() && ((SelectedMenuItem == null) || !MenuItems.Contains(SelectedMenuItem)))
            {
                SelectedMenuItem = MenuItems[0];
                if (!SelectedMenuItem.IsEnabled)
                {
                    // skip all disabled menuitems
                    MoveSelectionDown();
                }
            }
        }

        /// <summary>
        /// Return OS header
        /// </summary>
        /// <param name="optionalAppName">string</param>
        /// <returns>string</returns>
        public virtual string OSTxt(string optionalAppName = "")
        {
            string txt =
                "Solar OS V3.2" +
                " - logged in as: " + CharacterSettings.Instance().Name;

            if (optionalAppName != string.Empty)
            {
                txt += " - " + optionalAppName;
            }

            return txt + "\n\n";
        }

        /// <summary>
        /// Scrolls the Scrollview to the top
        /// </summary>
        protected void ScrollToTop()
        {
            if (ConsoleScrollRect != null)
            {
                ConsoleScrollRect.verticalNormalizedPosition = 1;
            }
        }

        /// <summary>
        /// Main menu
        /// </summary>
        protected virtual void LoadMainMenu()
        {
            MenuItems = new List<SolarOSMenuItem>();
            if (Network != SolarOSNetwork.Space)
            {
                MenuItems.Add(new SolarOSMenuItem("social media", new SolarOSSocialMedia(this)));
                if ((Network == SolarOSNetwork.VenusHome) || (Network == SolarOSNetwork.Venref))
                {
                    MenuItems.Add(new SolarOSMenuItem("IOT devices", LoadIOTDevices, RefreshDisplay));
                }

                if (NetworkIsOnVenus())
                {
                    MenuItems.Add(new SolarOSMenuItem("VPN to work", LoadVPNToWork, RefreshDisplay));
                }

                MenuItems.Add(new SolarOSMenuItem("secure web", new SolarOSWeb(this)));
            }

            MenuItems.Add(new SolarOSMenuItem("solarbits wallet", LoadSolarBitsWallet, RefreshDisplay));
        }

        /// <summary>
        /// Your home Internet Of Things devices
        /// </summary>
        protected virtual void LoadIOTDevices()
        {
            MenuItems = new List<SolarOSMenuItem>();

            var inv = PlayerInventory.Instance();

            // todo: make a separate class for IOT control, this code is getting silly
            if (Network == SolarOSNetwork.VenusHome)
            {
                string frontDoorLockText;
                if (inv.ContainsItem(KnownItem.IOTVenusHouseLockControl))
                {
                    frontDoorLockText = "lock front door";
                }
                else
                {
                    frontDoorLockText = "unlock front door";
                }

                MenuItems.Add(new SolarOSMenuItem(
                    frontDoorLockText,
                    () =>
                    {
                        if (inv.ContainsItem(KnownItem.IOTVenusHouseLockControl))
                        {
                            inv.Pull(KnownItem.IOTVenusHouseLockControl);
                            MenuItems[0].Description = "unlock front door";
                        }
                        else
                        {
                            KnownItemsInventory.Instance().TransferItem(KnownItem.IOTVenusHouseLockControl, inv);
                            MenuItems[0].Description = "lock front door";
                        }

                        PopPreviousApplicationBreadcrumb();
                    },
                    RefreshDisplay));
            }
        }

        /// <summary>
        /// Work VPN application listing
        /// </summary>
        protected virtual void LoadVPNToWork()
        {
            MenuItems = new List<SolarOSMenuItem>();
            MenuItems.Add(new SolarOSMenuItem("source code", new SolarOSGIT(this)));
        }
        
        /// <summary>
        /// Default menu when an application is not installed
        /// </summary>
        protected virtual void LoadApplicationNotInstalled()
        {
            MenuItems = new List<SolarOSMenuItem>();
            MenuItems.Add(new SolarOSMenuItem("Application not installed", () => { }, RefreshDisplay));
        }

        /// <summary>
        /// Loads the SolarBitsWallet application
        /// </summary>
        protected virtual void LoadSolarBitsWallet()
        {
            var wallet = PlayerWallet.Instance();

            MenuItems = new List<SolarOSMenuItem>();
            MenuItems.Add(new SolarOSMenuItem("Account balance: " + wallet.GetAmount() + " solarbits", () => { }, RefreshDisplay));
        }

        /// <summary>
        /// Returns the selected menu item
        /// </summary>
        /// <returns>SolarOSMenuItem</returns>
        protected SolarOSMenuItem GetSelectedMenuItem()
        {
            return SelectedMenuItem;
        }

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
        protected string MenuOptionsTxt()
        {
            string menuOptions = string.Empty;

            if (MenuItems == null)
            {
                return menuOptions;
            }

            foreach (var menuItem in MenuItems)
            {
                if (!menuItem.IsEnabled)
                {
                    menuOptions += "[-]";
                }
                else if (menuItem == SelectedMenuItem)
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

        /// <summary>
        /// General refresh function
        /// </summary>
        protected virtual void RefreshDisplay()
        {
            InitSelection();

            ConsoleOutput.text =
                OSTxt() +
                MenuTxt() +
                MenuOptionsTxt();
        }

        /// <summary>
        /// Go up a menuitem
        /// </summary>
        protected void MoveSelectionUp()
        {
            var currentSelection = GetSelectedMenuItem();
            if (currentSelection != null)
            {
                var itemIndex = MenuItems.IndexOf(currentSelection);
                itemIndex--;

                while (itemIndex >= 0)
                {
                    var item = MenuItems[itemIndex];
                    if (!item.IsEnabled)
                    {
                        // skip disabled menu items
                        itemIndex--;
                    }
                    else
                    {
                        SelectedMenuItem = item;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Go down a menuitem
        /// </summary>
        protected void MoveSelectionDown()
        {
            var currentSelection = GetSelectedMenuItem();
            if (currentSelection != null)
            {
                var itemIndex = MenuItems.IndexOf(currentSelection);
                itemIndex++;
                while (itemIndex < MenuItems.Count)
                {
                    var item = MenuItems[itemIndex];
                    if (!item.IsEnabled)
                    {
                        // skip disabled menu items
                        itemIndex++;
                    }
                    else
                    {
                        SelectedMenuItem = item;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Executes the function connected to the given MenuItem
        /// </summary>
        /// <param name="menuItem"></param>
        protected virtual void RunMenuItemWithoutAddingToBreadcrumbs(SolarOSMenuItem menuItem)
        {
            if ((menuItem != null) && (menuItem.OnRunApplication != null))
            {
                CurrentApplication = menuItem;

                menuItem.OnRunApplication();
            }
            else if (menuItem == null)
            {
                CurrentApplication = null;

                LoadMainMenu();
            }
        }

        /// <summary>
        /// Executes the function connected to the given MenuItem
        /// </summary>
        /// <param name="menuItem"></param>
        protected virtual void RunMenuItem(SolarOSMenuItem menuItem)
        {
            if ((menuItem != null) && (menuItem.OnRunApplication == null) && (menuItem.OnDisplay == null))
            {
                // nothing to run
            }
            else
            {
                AddBreadCrumb(CurrentApplication);

                RunMenuItemWithoutAddingToBreadcrumbs(menuItem);
            }
        }
    }
}