﻿namespace PlanetaryDeception
{
    /// <summary>
    /// delegate runApplication
    /// </summary>
    public delegate void RunApplication();

    /// <summary>
    /// delegate displayApplication
    /// </summary>
    public delegate void DisplayApplication();

    /// <summary>
    /// Menu Item
    /// </summary>
    public class SolarOSMenuItem
    {
        /// <summary>
        /// Title/description
        /// </summary>
        public string Description;

        /// <summary>
        /// Function that initializes the application or menu
        /// </summary>
        public RunApplication OnRunApplication;

        /// <summary>
        /// Custom render function for the application
        /// </summary>
        public DisplayApplication OnDisplay;

        /// <summary>
        /// Constructor menuitem
        /// </summary>
        /// <param name="description"></param>
        /// <param name="onRun"></param>
        /// <param name="onDisplay"></param>
        public SolarOSMenuItem(string description, RunApplication onRun, DisplayApplication onDisplay)
        {
            Description = description;
            OnRunApplication = onRun;
            OnDisplay = onDisplay;
        }

        /// <summary>
        /// Constructor2 menuitem
        /// </summary>
        /// <param name="description"></param>
        public SolarOSMenuItem(string description)
        {
            Description = description;
            OnRunApplication = null;
            OnDisplay = null;
        }

        /// <summary>
        /// Constructor3 menuitem
        /// </summary>
        /// <param name="description"></param>
        /// <param name="application"></param>
        public SolarOSMenuItem(string description, SolarOSApplication application)
        {
            Description = description;
            OnRunApplication = application.Run;
            OnDisplay = application.RefreshDisplay;
        }
    }
}
