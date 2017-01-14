namespace PlanetaryDeception
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

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
        /// Known scenes and their settings
        /// </summary>
        private Dictionary<string, SceneSettings> knownScenes;

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

            knownScenes = new Dictionary<string, SceneSettings>();
        }

        /// <summary>
        /// Save this scene's settings and Load a new scene
        /// </summary>
        public void TransitionToNewScene(string newScene, SpriteRenderer playerObject)
        {
            var settings = GetCurrentSceneSettings();
            if (playerObject != null)
            {
                settings.PlayerPosX = playerObject.transform.position.x;
                settings.PlayerPosY = playerObject.transform.position.y;
                settings.PlayerIsFacingRight = !playerObject.flipX;
                settings.PlayerPosIsSet = true;
            }

            SceneManager.LoadScene(newScene);
        }

        /// <summary>
        /// Gets or creates the current scene's settings
        /// </summary>
        /// <returns></returns>
        public SceneSettings GetCurrentSceneSettings()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            if (knownScenes.ContainsKey(currentSceneName))
            {
                return knownScenes[currentSceneName];
            }
            else
            {
                var settings = new SceneSettings();
                settings.SceneName = currentSceneName;

                knownScenes.Add(currentSceneName, settings);

                return settings;
            }
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
