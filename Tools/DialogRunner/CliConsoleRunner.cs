using System;
using System.Collections.Generic;
using PlanetaryDeception;
using UnityEngine.UI;

namespace DialogRunner
{
    public class CliConsoleRunner
    {
        private readonly SolarOS os;

        public CliConsoleRunner(SolarOSNetwork network)
        {
            os = new SolarOS();
            os.ConsoleOutput = new Text();
            os.ConsoleScrollRect = new ScrollRect();
            os.NetworkEnvironment = network;

            var settings = CharacterSettings.Instance();
            if (string.IsNullOrEmpty(settings.Name))
            {
                settings.Name = "Player";
            }

            os.Start();
        }

        public void Run()
        {
            while (true)
            {
                Render();

                Console.Write("> ");
                var input = Console.ReadLine();

                if (input == null || input.ToLower() == "quit" || input.ToLower() == "exit")
                    break;

                if (input == "0")
                {
                    var lastBreadcrumb = os.PopPreviousApplicationBreadcrumb();
                    if (lastBreadcrumb == null && os.CurrentApplication == null)
                        break;

                    var reselectOption = os.CurrentApplication;

                    if (lastBreadcrumb != null)
                    {
                        RunMenuItemWithoutBreadcrumb(lastBreadcrumb);
                    }
                    else
                    {
                        // Back to main menu
                        os.CurrentApplication = null;
                        os.Start();
                    }

                    if (reselectOption != null)
                        os.SelectItemWithSameDescription(reselectOption);
                }
                else if (int.TryParse(input, out int choice))
                {
                    var enabled = GetEnabledMenuItems();
                    if (choice >= 1 && choice <= enabled.Count)
                    {
                        var selected = enabled[choice - 1];
                        os.SelectedMenuItem = selected;
                        RunMenuItem(selected);
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice, try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice, try again.");
                }
            }
        }

        private void Render()
        {
            os.InitSelection();

            // Let active app render its content to ConsoleOutput.text
            if (os.CurrentApplication != null && os.CurrentApplication.OnDisplay != null)
            {
                os.CurrentApplication.OnDisplay();
            }

            Console.WriteLine();

            // If an app rendered content, show it (it includes header + content)
            // Otherwise show the default OS header
            if (os.CurrentApplication != null && !string.IsNullOrEmpty(os.ConsoleOutput.text))
            {
                // Replace [x]/[ ] markers with numbered options
                var lines = os.ConsoleOutput.text.Split('\n');
                int num = 1;
                foreach (var line in lines)
                {
                    if (line.StartsWith("[x] ") || line.StartsWith("[ ] "))
                        Console.WriteLine($"[{num++}] " + line.Substring(4));
                    else if (line.StartsWith("[-] "))
                        Console.WriteLine(line);
                    else
                        Console.WriteLine(line);
                }
            }
            else
            {
                Console.WriteLine(os.OSTxt());

                int num = 1;
                if (os.MenuItems != null)
                {
                    foreach (var item in os.MenuItems)
                    {
                        if (!item.IsEnabled)
                            Console.WriteLine($"  [-] {item.Description}");
                        else
                            Console.WriteLine($"  [{num++}] {item.Description}");
                    }
                }
            }

            Console.WriteLine($"  [0] Back");
            Console.WriteLine();
        }

        private List<SolarOSMenuItem> GetEnabledMenuItems()
        {
            var enabled = new List<SolarOSMenuItem>();
            if (os.MenuItems == null) return enabled;

            foreach (var item in os.MenuItems)
            {
                if (item.IsEnabled)
                    enabled.Add(item);
            }
            return enabled;
        }

        private void RunMenuItem(SolarOSMenuItem menuItem)
        {
            if (menuItem != null && (menuItem.OnRunApplication != null || menuItem.OnDisplay != null))
            {
                os.AddBreadCrumb(os.CurrentApplication);
                RunMenuItemWithoutBreadcrumb(menuItem);
            }
        }

        private void RunMenuItemWithoutBreadcrumb(SolarOSMenuItem menuItem)
        {
            if (menuItem != null && menuItem.OnRunApplication != null)
            {
                os.CurrentApplication = menuItem;
                menuItem.OnRunApplication();
            }
            else if (menuItem == null)
            {
                os.CurrentApplication = null;
            }
        }
    }
}
