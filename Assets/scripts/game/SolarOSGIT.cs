namespace PlanetaryDeception
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// GIT app
    /// </summary>
    public class SolarOSGIT : SolarOSApplication
    {
        /// <summary>
        /// main menu
        /// </summary>
        private List<SolarOSMenuItem> mainMenu;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="os"></param>
        public SolarOSGIT(SolarOS os) : base(os)
        {
            mainMenu = new List<SolarOSMenuItem>();
            mainMenu.Add(new SolarOSMenuItem("latest code review request", this));
        }

        /// <summary>
        /// refresh
        /// </summary>
        public override void RefreshDisplay()
        {
            parentOS.InitSelection();

            string description = string.Empty;
            if (parentOS.CurrentApplication == mainMenu[0])
            {
                description = "select noteworthy lines of code...\n";
            }

            parentOS.ConsoleOutput.text =
                parentOS.OSTxt("vite git repo") +
                description +
                MenuOptionsTxt();
        }

        /// <summary>
        /// init
        /// </summary>
        public override void Run()
        {
            if (parentOS.CurrentApplication == mainMenu[0])
            {
                LoadTextFileAsMenu("VoasisAPI");
            }
            else
            {
                parentOS.MenuItems = mainMenu;
            }
        }

        /// <summary>
        /// Loads a textfile from the resources as a menu
        /// </summary>
        /// <param name="resourceName">string</param>
        private void LoadTextFileAsMenu(string resourceName)
        {
            var menu = new List<SolarOSMenuItem>();

            var scriptText = Resources.Load(resourceName) as TextAsset;
            var stringSeparators = new string[] { "\r\n", "\r", "\n" };
            var result = scriptText.text.Split(stringSeparators, StringSplitOptions.None);

            var playerInventory = PlayerInventory.Instance();
            bool hasMarkedCredentials = playerInventory.ContainsItem(KnownItem.VoasisCredentials);

            foreach (var line in result)
            {
                if (line.Contains("// todo:") || line.Contains("username = \"") || line.Contains("password = \""))
                {
                    if (hasMarkedCredentials)
                    {
                        menu.Add(new SolarOSMenuItem(" >> " + line.ToLower()));
                    }
                    else
                    {
                        menu.Add(new SolarOSMenuItem(
                            line.ToLower(),
                            () =>
                            {
                                if (!hasMarkedCredentials)
                                {
                                    KnownItemsInventory.Instance().TransferItem(KnownItem.VoasisCredentials, playerInventory);
                                }

                                parentOS.PopPreviousApplicationBreadcrumb();
                                parentOS.CurrentApplication = mainMenu[0];
                                Run();
                            },
                            RefreshDisplay));
                    }
                }
                else
                {
                    menu.Add(new SolarOSMenuItem(line.ToLower()));
                }
            }

            parentOS.MenuItems = menu;
        }
    }
}
