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
        /// Current Network the console is connected to
        /// </summary>
        public SolarOSNetwork Network;

        /// <summary>
        /// delay helper vars
        /// </summary>
        protected float nextInputAllowed = 0.0F;

        /// <summary>
        /// Current menu's menuitems
        /// </summary>
        protected List<SolarOSMenuItem> menuItems;

        /// <summary>
        /// Breadcrumb path
        /// </summary>
        protected List<SolarOSMenuItem> breadCrumbs;

        /// <summary>
        /// The currently running application
        /// </summary>
        protected SolarOSMenuItem currentApplication;

        /// <summary>
        /// The current by the user selected menu item
        /// </summary>
        protected SolarOSMenuItem selectedMenuItem;

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
        /// Unity start
        /// </summary>
        public void Start()
        {
            breadCrumbs = new List<SolarOSMenuItem>();
            LoadMainMenu();
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
                }
                else if (Input.GetButtonUp("Fire2"))
                {
                    var lastBreadcrumb = PopPreviousApplicationBreadcrumb();
                    if ((lastBreadcrumb == null) && (currentApplication == null))
                    {
                        var currentScene = SceneManager.GetActiveScene();
                        var currentSceneName = currentScene.name;
                        SceneManager.LoadScene(currentSceneName.Substring(0, currentSceneName.Length - 1));
                        return;
                    }
                    else
                    {
                        RunMenuItemWithoutAddingToBreadcrumbs(lastBreadcrumb);
                    }
                }
            }

            if (currentApplication == null)
            {
                RefreshDisplay();
            }
            else if (currentApplication.OnDisplay != null)
            {
                currentApplication.OnDisplay();
            }
        }

        /// <summary>
        /// Returns the latest breadcrumb
        /// </summary>
        /// <returns>SolarOSMenuItem</returns>
        protected SolarOSMenuItem PopPreviousApplicationBreadcrumb()
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
        /// Main menu
        /// </summary>
        protected virtual void LoadMainMenu()
        {
            menuItems = new List<SolarOSMenuItem>();
            if (Network != SolarOSNetwork.Space)
            {
                menuItems.Add(new SolarOSMenuItem("social media", LoadApplicationNotInstalled, RefreshDisplay));
                if ((Network == SolarOSNetwork.VenusHome) || (Network == SolarOSNetwork.Venref))
                {
                    menuItems.Add(new SolarOSMenuItem("IOT devices", LoadIOTDevices, RefreshDisplay));
                }

                if ((Network & SolarOSNetwork.Venus) > 0)
                {
                    menuItems.Add(new SolarOSMenuItem("VPN to work", LoadVPNToWork, RefreshDisplay));
                }

                menuItems.Add(new SolarOSMenuItem("local tor", LoadApplicationNotInstalled, RefreshDisplay));
            }

            menuItems.Add(new SolarOSMenuItem("solarbits wallet", LoadSolarBitsWallet, RefreshDisplay));
        }

        /// <summary>
        /// Loads a textfile from the resources as a menu
        /// </summary>
        /// <param name="resourceName">string</param>
        protected void LoadTextFileAsMenu(string resourceName)
        {
            menuItems = new List<SolarOSMenuItem>();

            var scriptText = Resources.Load(resourceName) as TextAsset;
            var stringSeparators = new string[] { "\r\n", "\r", "\n" };
            var result = scriptText.text.Split(stringSeparators, StringSplitOptions.None);

            foreach (var line in result)
            {
                menuItems.Add(new SolarOSMenuItem(line.ToLower(), null, null));
            }
        }

        /// <summary>
        /// Your home Internet Of Things devices
        /// </summary>
        protected virtual void LoadIOTDevices()
        {
            menuItems = new List<SolarOSMenuItem>();

            var inv = PlayerInventory.Instance();

            // todo: make a separate class for IOT control, this code is getting silly
            if (Network == SolarOSNetwork.VenusHome)
            {
                string frontDoorLockText;
                if (inv.ContainsItem(KnownItemsInventory.IOTVenusHouseLockControl))
                {
                    frontDoorLockText = "lock front door";
                }
                else
                {
                    frontDoorLockText = "unlock front door";
                }

                menuItems.Add(new SolarOSMenuItem(
                    frontDoorLockText,
                    () =>
                    {
                        if (inv.ContainsItem(KnownItemsInventory.IOTVenusHouseLockControl))
                        {
                            inv.Pull(KnownItemsInventory.IOTVenusHouseLockControl);
                            menuItems[0].Description = "unlock front door";
                        }
                        else
                        {
                            KnownItemsInventory.Instance().TransferItem(KnownItemsInventory.IOTVenusHouseLockControl, inv);
                            menuItems[0].Description = "lock front door";
                        }

                        PopPreviousApplicationBreadcrumb();
                    },
                    RefreshDisplay));
            }
        }

        /// <summary>
        /// Load Work GIT
        /// </summary>
        protected virtual void LoadWorkGIT()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(
                new SolarOSMenuItem(
                    "latest code review request",
                    () =>
                    {
                        LoadTextFileAsMenu("VoasisAPI");
                    },
                    RefreshDisplayGITCodeReview));
        }

        /// <summary>
        /// Work VPN application listing
        /// </summary>
        protected virtual void LoadVPNToWork()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("GIT", LoadWorkGIT, RefreshDisplay));
            menuItems.Add(new SolarOSMenuItem("work orders", () => { }, RefreshDisplay));
        }

        /// <summary>
        /// Twitter
        /// </summary>
        protected virtual void LoadSocialMedia()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("send direct message", () => { }, RefreshDisplay));
        }

        /// <summary>
        /// Default menu when an application is not installed
        /// </summary>
        protected virtual void LoadApplicationNotInstalled()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("Application not installed", () => { }, RefreshDisplay));
        }

        /// <summary>
        /// Loads the SolarBitsWallet application
        /// </summary>
        protected virtual void LoadSolarBitsWallet()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("Account balance: 0 solarbits", () => { }, RefreshDisplay));
        }

        /// <summary>
        /// Returns the selected menu item
        /// </summary>
        /// <returns>SolarOSMenuItem</returns>
        protected SolarOSMenuItem GetSelectedMenuItem()
        {
            return selectedMenuItem;
        }

        /// <summary>
        /// Appends a breadcrumb
        /// </summary>
        /// <param name="menuItem">SolarOSMenuItem</param>
        protected void AddBreadCrumb(SolarOSMenuItem menuItem)
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
        protected bool HasItems()
        {
            return (menuItems != null) && (menuItems.Count > 0);
        }

        /// <summary>
        /// Initialize selectedMenuItem for the current menu
        /// </summary>
        protected void InitSelection()
        {
            if (HasItems() && ((selectedMenuItem == null) || !menuItems.Contains(selectedMenuItem)))
            {
                selectedMenuItem = menuItems[0];
            }
        }

        /// <summary>
        /// Return OS header
        /// </summary>
        /// <returns>string</returns>
        protected string OSTxt()
        {
            return "Solar OS V3.2 - logged in as: " + CharacterSettings.Instance().Name + "\n\n";
        }

        /// <summary>
        /// Return menu header
        /// </summary>
        /// <returns>string</returns>
        protected string MenuTxt()
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

            foreach (var menuItem in menuItems)
            {
                if (menuItem == selectedMenuItem)
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
        /// GIT code review request
        /// </summary>
        protected virtual void RefreshDisplayGITCodeReview()
        {
            InitSelection();

            ConsoleOutput.text =
                OSTxt() +
                "1 file to review:\n" +
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
                var itemIndex = menuItems.IndexOf(currentSelection);
                if (itemIndex > 0)
                {
                    selectedMenuItem = menuItems[itemIndex - 1];
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
                var itemIndex = menuItems.IndexOf(currentSelection);
                if (itemIndex < menuItems.Count - 1)
                {
                    selectedMenuItem = menuItems[itemIndex + 1];
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
                currentApplication = menuItem;

                menuItem.OnRunApplication();
            }
            else if (menuItem == null)
            {
                currentApplication = null;

                LoadMainMenu();
            }
        }

        /// <summary>
        /// Executes the function connected to the given MenuItem
        /// </summary>
        /// <param name="menuItem"></param>
        protected virtual void RunMenuItem(SolarOSMenuItem menuItem)
        {
            AddBreadCrumb(currentApplication);

            RunMenuItemWithoutAddingToBreadcrumbs(menuItem);
        }
    }
}