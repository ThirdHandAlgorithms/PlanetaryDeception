namespace PlanetaryDeception
{
    using UnityEngine;

    /// <summary>
    /// CharacterSettings
    /// </summary>
    public class CharacterSettings
    {
        /// <summary>
        /// Character Name
        /// </summary>
        public string Name;

        /// <summary>
        /// HairStyle number
        /// </summary>
        public int HairStyle;

        /// <summary>
        /// Accessory number
        /// </summary>
        public int Accessory;

        /// <summary>
        /// Outfit number
        /// </summary>
        public int Outfit;

        /// <summary>
        /// Skin color
        /// </summary>
        public Color SkinColor;

        /// <summary>
        /// Hair color
        /// </summary>
        public Color HairColor;

        /// <summary>
        /// Accessory color
        /// </summary>
        public Color AccessoryColor;

        /// <summary>
        /// Singleton var
        /// </summary>
        private static CharacterSettings thisInstance = null;

        /// <summary>
        /// Le Constructeur
        /// </summary>
        public CharacterSettings()
        {
            Name = "Player";
            HairStyle = 0;
            Accessory = 0;
            Outfit = 0;
            HairColor = Color.black;
            AccessoryColor = Color.black;
            SkinColor = new Color(0xCD / 255f, 0xA1 / 255f, 0x84 / 255f);
        }

        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns>CharacterSettings</returns>
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
