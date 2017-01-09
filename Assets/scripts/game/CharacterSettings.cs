namespace PlanetaryDeception
{
    using UnityEngine;

    public class CharacterSettings
    {
        private static CharacterSettings thisInstance = null;

        public string Name;
        public int HairStyle;
        public int Accessory;
        public int Outfit;
        public Color HairColor;
        public Color AccessoryColor;

        public CharacterSettings()
        {
            Name = "Player";
            HairStyle = 0;
            Accessory = 0;
            Outfit = 0;
            HairColor = Color.black;
            AccessoryColor = Color.black;
        }

        public static CharacterSettings Instance()
        {
            if (thisInstance == null)
            {
                thisInstance = new CharacterSettings();
            }

            return thisInstance;
        }
    }
}
