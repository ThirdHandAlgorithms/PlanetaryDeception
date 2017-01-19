namespace PlanetaryDeception
{
    using System.Collections.Generic;

    public interface ISolarOS
    {
        bool IsStarted { get; set; }
        List<SolarOSMenuItem> MenuItems { get; set; }
        SolarOSMenuItem SelectedMenuItem { get; set; }

        void SelectItemWithSameDescription(SolarOSMenuItem reselectItem);
        bool NetworkIsOnVenus();
        SolarOSMenuItem PopPreviousApplicationBreadcrumb();
        void AddBreadCrumb(SolarOSMenuItem menuItem);
        bool HasItems();
        void InitSelection();
        string OSTxt(string optionalAppName = "");
    }
}
