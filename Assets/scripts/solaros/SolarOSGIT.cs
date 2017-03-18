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

        private int currentLineCount;

        private int previousCursorRow = 0;

        private int virtualScrollStartingRow = 0;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="os"></param>
        public SolarOSGIT(ISolarOS os) : base(os)
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

            var FullOutputText =
                parentOS.OSTxt("vite git repo") +
                description +
                MenuOptionsTxt();

            currentLineCount = FullOutputText.Split('\n').Length;

            parentOS.SetConsoleText(FullOutputText);

            DetermineViewportPositionByCursorRow(CursorRow);
        }

        protected void DetermineViewportPositionByCursorRow(int cursorRow)
        {
            // currentLineCount = 100%
            // cursorRow between 0..currentLineCount
            // cursorRow / currentLineCount

            // yay!
            int screenRowCount = 18;

            float skipPerScroll = 0.02f;

            if (cursorRow < previousCursorRow)
            {
                // scrolling down
                if (cursorRow < virtualScrollStartingRow + screenRowCount)
                {
                    // actually scroll down
                    if (parentOS.ScrollVerticalPosition + skipPerScroll < 1.0f)
                    {
                        parentOS.ScrollVerticalPosition = parentOS.ScrollVerticalPosition + skipPerScroll;
                        virtualScrollStartingRow--;
                    }
                    else
                    {
                        parentOS.ScrollVerticalPosition = 1.0f;
                    }
                }
            }
            else if (cursorRow > previousCursorRow)
            {
                // scrolling up
                if (cursorRow > virtualScrollStartingRow + screenRowCount)
                {
                    // actually scroll down
                    if (parentOS.ScrollVerticalPosition - skipPerScroll > 0.0f)
                    {
                        parentOS.ScrollVerticalPosition = parentOS.ScrollVerticalPosition - skipPerScroll;
                        virtualScrollStartingRow++;
                    }
                    else
                    {
                        parentOS.ScrollVerticalPosition = 0.0f;
                    }
                }
            }
            else
            {
                // staying put
            }


            // only scroll when at the bottom... can we do that?

            Debug.Log("scroll vert pos = " + parentOS.ScrollVerticalPosition);

            if (previousCursorRow != cursorRow)
            {
                previousCursorRow = cursorRow;
            }
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
