namespace PlanetaryDeception
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class SolarOS : LevelBase
    {
        protected class SolarOSMenuItem
        {
            public string Description;
            public bool Selected;

            public SolarOSMenuItem(string description)
            {
                Description = description;
                Selected = false;
            }
        }

        public Text ConsoleOutput;

        protected float nextInputAllowed = 0.0F;

        protected bool isInMenu;
        protected List<SolarOSMenuItem> menuItems;
        protected SolarOSMenuItem previousMenuItem;
        protected SolarOSMenuItem currentMenuItem;

        public void Start()
        {
            previousMenuItem = null;
            loadMainMenu();
        }

        protected virtual void loadMainMenu()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("social media"));
            menuItems.Add(new SolarOSMenuItem("IOT devices"));
            menuItems.Add(new SolarOSMenuItem("VPN to work"));
            menuItems.Add(new SolarOSMenuItem("local tor"));
            menuItems.Add(new SolarOSMenuItem("solarbits wallet"));

            isInMenu = true;
            previousMenuItem = null;
        }

        protected virtual void loadIOTDevices()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("Application not installed"));
            //menuItems.Add(new SolarOSMenuItem("front door lock"));
            //menuItems.Add(new SolarOSMenuItem("front door camera"));
            //menuItems.Add(new SolarOSMenuItem("air conditioning"));
            //menuItems.Add(new SolarOSMenuItem("lights"));

            isInMenu = true;
        }

        protected virtual void loadVPNToWork()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("Application not installed"));
            //menuItems.Add(new SolarOSMenuItem("GIT"));
            //menuItems.Add(new SolarOSMenuItem("work orders"));

            isInMenu = true;
        }

        protected virtual void loadSocialMedia()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("Application not installed"));
            //menuItems.Add(new SolarOSMenuItem("send direct message"));

            isInMenu = true;
        }

        protected virtual void loadLocalTOR()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("Application not installed"));

            isInMenu = true;
        }

        protected virtual void loadSolarBitsWallet()
        {
            menuItems = new List<SolarOSMenuItem>();
            menuItems.Add(new SolarOSMenuItem("Account balance: 0 solarbits"));
            //menuItems.Add(new SolarOSMenuItem("Transfer"));

            isInMenu = true;
        }

        protected SolarOSMenuItem getSelectedMenuItem()
        {
            foreach (var menuItem in menuItems)
            {
                if (menuItem.Selected)
                {
                    return menuItem;
                }
            }

            return null;
        }

        protected void initSelection()
        {
            if ((menuItems != null) && (menuItems.Count > 0) && (getSelectedMenuItem() == null))
            {
                menuItems[0].Selected = true;
            }
        }

        protected string osTxt()
        {
            return "Solar OS V3.2\n\n";
        }

        protected string menuTxt()
        {
            return "Available functions:\n";
        }

        protected string menuOptionsTxt()
        {
            string menuOptions = "";

            foreach (var menuItem in menuItems)
            {
                if (menuItem.Selected)
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

        protected virtual void RefreshDisplay()
        {
            if (isInMenu)
            {
                initSelection();

                ConsoleOutput.text =
                    osTxt() +
                    menuTxt() +
                    menuOptionsTxt();
            }
            else
            {
                ConsoleOutput.text =
                    osTxt();
            }
        }

        protected void moveSelectionUp()
        {
            var currentSelection = getSelectedMenuItem();
            if (currentSelection != null)
            {
                var itemIndex = menuItems.IndexOf(currentSelection);
                if (itemIndex > 0)
                {
                    currentSelection.Selected = false;
                    menuItems[itemIndex - 1].Selected = true;
                }
            }
        }

        protected void moveSelectionDown()
        {
            var currentSelection = getSelectedMenuItem();
            if (currentSelection != null)
            {
                var itemIndex = menuItems.IndexOf(currentSelection);
                if (itemIndex < menuItems.Count - 1)
                {
                    currentSelection.Selected = false;
                    menuItems[itemIndex + 1].Selected = true;
                }
            }
        }

        protected virtual void runMenuItem(SolarOSMenuItem menuItem)
        {
            currentMenuItem = menuItem;

            if (menuItem == null)
            {
                loadMainMenu();
            }
            else
            {
                if (menuItem.Description == "social media")
                {
                    loadSocialMedia();
                }
                else if (menuItem.Description == "IOT devices")
                {
                    loadIOTDevices();
                }
                else if (menuItem.Description == "VPN to work")
                {
                    loadVPNToWork();
                }
                else if (menuItem.Description == "local tor")
                {
                    loadLocalTOR();
                }
                else if (menuItem.Description == "solarbits wallet")
                {
                    loadSolarBitsWallet();
                }
            }
        }

        public void Update()
        {
            if (Time.time >= nextInputAllowed)
            {
                var verticalAxis = Input.GetAxis("Vertical");

                if (verticalAxis > 0)
                {
                    moveSelectionUp();

                    nextInputAllowed = Time.time + 0.2F;
                }
                else if (verticalAxis < 0)
                {
                    moveSelectionDown();

                    nextInputAllowed = Time.time + 0.2F;
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    var selected = getSelectedMenuItem();
                    if (selected != null)
                        runMenuItem(selected);
                }
                else if (Input.GetButtonUp("Fire2"))
                {
                    if ((previousMenuItem == null) && (currentMenuItem == null))
                    {
                        var currentScene = SceneManager.GetActiveScene();
                        var currentSceneName = currentScene.name;
                        SceneManager.LoadScene(currentSceneName.Substring(0, currentSceneName.Length - 1));
                        return;
                    }
                    else
                    {
                        runMenuItem(previousMenuItem);
                    }
                }
            }

            RefreshDisplay();
        }
    }
}