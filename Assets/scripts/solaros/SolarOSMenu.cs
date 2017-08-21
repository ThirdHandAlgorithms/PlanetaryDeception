namespace PlanetaryDeception
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
        /// Actual Application
        /// </summary>
        public SolarOSApplication Application;

        /// <summary>
        /// Custom render function for the application
        /// </summary>
        public DisplayApplication OnDisplay;

        /// <summary>
        /// Is enabled?
        /// </summary>
        public bool IsEnabled;

        /// <summary>
        /// Constructor menuitem
        /// </summary>
        /// <param name="description"></param>
        /// <param name="onRun"></param>
        /// <param name="onDisplay"></param>
        public SolarOSMenuItem(string description, RunApplication onRun, DisplayApplication onDisplay)
        {
            Application = null;
            Description = description;
            OnRunApplication = onRun;
            OnDisplay = onDisplay;
            IsEnabled = true;
        }

        /// <summary>
        /// Supply actual application as well as separate run and display functions
        /// </summary>
        /// <param name="description"></param>
        /// <param name="application"></param>
        /// <param name="onRun"></param>
        /// <param name="onDisplay"></param>
        public SolarOSMenuItem(string description, SolarOSApplication application, RunApplication onRun, DisplayApplication onDisplay)
        {
            Application = application;
            Description = description;
            OnRunApplication = onRun;
            OnDisplay = onDisplay;
            IsEnabled = true;
        }

        /// <summary>
        /// Constructor2 menuitem
        /// </summary>
        /// <param name="description"></param>
        /// /// <param name="enabled"></param>
        public SolarOSMenuItem(string description, bool enabled = true)
        {
            Application = null;
            Description = description;
            OnRunApplication = null;
            OnDisplay = null;
            IsEnabled = enabled;
        }

        /// <summary>
        /// Constructor3 menuitem
        /// </summary>
        /// <param name="description"></param>
        /// <param name="application"></param>
        public SolarOSMenuItem(string description, SolarOSApplication application)
        {
            Application = application;
            Description = description;
            OnRunApplication = application.Run;
            OnDisplay = application.RefreshDisplay;
            IsEnabled = true;
        }
    }
}
