namespace PlanetaryDeception
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SaveData
    {
        public string Name { get; set; }
        public int HairStyle { get; set; }
        public int Accessory { get; set; }
        public int Outfit { get; set; }
        public float SkinColorR { get; set; }
        public float SkinColorG { get; set; }
        public float SkinColorB { get; set; }
        public float HairColorR { get; set; }
        public float HairColorG { get; set; }
        public float HairColorB { get; set; }
        public float AccessoryColorR { get; set; }
        public float AccessoryColorG { get; set; }
        public float AccessoryColorB { get; set; }
        public int WalletAmount { get; set; }
        public List<KnownItem> InventoryItems { get; set; }
        public List<SceneSettingsData> Scenes { get; set; }
        public string CurrentScene { get; set; }
    }

    [Serializable]
    public class SceneSettingsData
    {
        public string SceneName { get; set; }
        public bool PlayerPosIsSet { get; set; }
        public float PlayerPosX { get; set; }
        public float PlayerPosY { get; set; }
        public bool PlayerIsFacingRight { get; set; }
    }
}
