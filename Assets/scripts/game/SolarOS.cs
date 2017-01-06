namespace PlanetaryDeception
{
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
        /// delay helper vars
        /// </summary>
        protected float nextInputAllowed = 0.0F;

        /// <summary>
        /// Current menu's menuitems
        /// </summary>
        protected List<SolarOSMenuItem> menuItems;

        /// <summary>
        /// The previously opened menuitem
        /// </summary>
        protected SolarOSMenuItem previousApplication;

        /// <summary>
        /// The currently running application
        /// </summary>
        protected SolarOSMenuItem currentApplication;

        /// <summary>
        /// The current by the user selected menu item
        /// </summary>
        protected SolarOSMenuItem selectedMenuItem;

        /// <summary>
        /// Unity start
        /// </summary>
        public void Start()
        {
            previousApplication = null;
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
                    if ((previousApplication == null) && (currentApplication == null))
                    {
                        var currentScene = SceneManager.GetActiveScene();
                        var currentSceneName = currentScene.name;
                        SceneManager.LoadScene(currentSceneName.Substring(0, currentSceneName.Length - 1));
                        return;
                    }
                    else
                    {
                        RunMenuItem(previousApplication);
                    }
                }
            }

            if (currentApplication == null)
            {
                RefreshDisplay();
            }
            else
            {
                currentApplication.OnDisplay();
            }
        }

        /// <summary>
        /// Main menu
        /// </summary>
        protected virtual void LoadMainMenu()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("social media", LoadApplicationNotInstalled, RefreshDisplay));
            menuItems.Add(new SolarOSMenuItem("IOT devices", LoadApplicationNotInstalled, RefreshDisplay));
            menuItems.Add(new SolarOSMenuItem("VPN to work", LoadApplicationNotInstalled, RefreshDisplay));
            menuItems.Add(new SolarOSMenuItem("local tor", LoadApplicationNotInstalled, RefreshDisplay));
            menuItems.Add(new SolarOSMenuItem("solarbits wallet", LoadSolarBitsWallet, RefreshDisplay));

            previousApplication = null;
        }

        /// <summary>
        /// Your home Internet Of Things devices
        /// </summary>
        protected virtual void LoadIOTDevices()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("front door lock", () => { }, RefreshDisplay));
            menuItems.Add(new SolarOSMenuItem("front door camera", () => { }, RefreshDisplay));
            menuItems.Add(new SolarOSMenuItem("air conditioning", () => { }, RefreshDisplay));
            menuItems.Add(new SolarOSMenuItem("lights", () => { }, RefreshDisplay));
        }

        /// <summary>
        /// Work VPN application listing
        /// </summary>
        protected virtual void LoadVPNToWork()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("GIT", () => { }, RefreshDisplay));
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
            return "Solar OS V3.2\n\n";
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
        protected virtual void RunMenuItem(SolarOSMenuItem menuItem)
        {
            currentApplication = menuItem;

            if (menuItem == null)
            {
                LoadMainMenu();
            }
            else
            {
                menuItem.OnRunApplication();
            }
        }
    }
}