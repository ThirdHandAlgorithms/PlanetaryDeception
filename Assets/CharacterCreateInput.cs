namespace PlanetaryDeception
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// Character creation screen control input
    /// </summary>
    public class CharacterCreateInput : MonoBehaviour
    {
        /// <summary>
        /// Hair sprite
        /// </summary>
        public SpriteRenderer HairPreview;

        /// <summary>
        /// Outfit sprite
        /// </summary>
        public SpriteRenderer OutfitPreview;

        /// <summary>
        /// Accessory sprite
        /// </summary>
        public SpriteRenderer AccessoryPreview;

        /// <summary>
        /// List of text controls
        /// </summary>
        private List<Text> controls;

        /// <summary>
        /// Current active control
        /// </summary>
        private Text currentControl;

        /// <summary>
        /// When the control/keyb input is allowed
        /// </summary>
        private float nextInputAllowed = 0.0f;

        /// <summary>
        /// List of sprite resources with hairstyles
        /// </summary>
        private Sprite[] allHairStyles;

        /// <summary>
        /// List of sprite resources with accessories
        /// </summary>
        private Sprite[] allAccessories;

        /// <summary>
        /// List of sprite resources with outfits
        /// </summary>
        private Sprite[] allOutfits;

        /// <summary>
        /// List of hair colors
        /// </summary>
        private List<Color> possibleHairColors;

        /// <summary>
        /// List of accessory colors
        /// </summary>
        private List<Color> possibleAccessoryColors;

        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            allHairStyles = Resources.LoadAll<Sprite>("Sprites/hair_styles");
            allAccessories = Resources.LoadAll<Sprite>("Sprites/hair_accessories");
            allOutfits = Resources.LoadAll<Sprite>("Sprites/dress_styles");

            possibleHairColors = new List<Color>();
            possibleHairColors.Add(Color.black);
            possibleHairColors.Add(Color.white);
            possibleHairColors.Add(Color.gray);
            possibleHairColors.Add(New255Color(0x57, 0x3d, 0x30));
            possibleHairColors.Add(New255Color(0x46, 0x7f, 0x00));
            possibleHairColors.Add(New255Color(0xff, 0x6a, 0x00));
            possibleHairColors.Add(New255Color(0xf8, 0xd8, 0x78));
            possibleHairColors.Add(New255Color(0x7f, 0x00, 0x00));
            possibleHairColors.Add(New255Color(0x7f, 0x00, 0x37));
            possibleHairColors.Add(New255Color(0x98, 0x78, 0xf8));
            possibleHairColors.Add(New255Color(0x00, 0x13, 0x7f));
            possibleHairColors.Add(New255Color(0x00, 0x40, 0x58));
            possibleHairColors.Add(New255Color(0x00, 0x88, 0x88));

            possibleAccessoryColors = new List<Color>();
            possibleAccessoryColors.Add(Color.red);
            possibleAccessoryColors.Add(Color.black);
            possibleAccessoryColors.Add(New255Color(0x46, 0x7f, 0x00));
            possibleAccessoryColors.Add(Color.yellow);
            possibleAccessoryColors.Add(New255Color(0x57, 0x00, 0x7f));
            possibleAccessoryColors.Add(New255Color(0xff, 0x00, 0xdc));

            controls = new List<Text>();

            var allTexts = GetComponentsInChildren<Text>();
            foreach (var txt in allTexts)
            {
                if (txt.name != "TextCC")
                {
                    controls.Add(txt);
                }
            }
            
            currentControl = controls[0];
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            currentControl.color = Color.red;

            SpriteRenderer currentPreview = null;
            Sprite[] currentSpriteArray = null;
            List<Color> currentColorArray = null;
            float inputDelay = 0.2F;

            if (Input.GetButton("Fire1") && (currentControl.name == "ButtonGO"))
            {
                SaveCharacterSettings();

                SceneManager.LoadScene("Level_1");
                return;
            }

            if (Time.time >= nextInputAllowed)
            {
                var verAxis = Input.GetAxis("Vertical");
                if (verAxis > 0)
                {
                    var idx = controls.IndexOf(currentControl);
                    if (idx > 0)
                    {
                        idx--;
                        currentControl.color = Color.white;
                        currentControl = controls[idx];
                        nextInputAllowed = Time.time + inputDelay;
                    }
                }
                else if (verAxis < 0)
                {
                    var idx = controls.IndexOf(currentControl);
                    if (idx < controls.Count - 1)
                    {
                        idx++;
                        currentControl.color = Color.white;
                        currentControl = controls[idx];
                        nextInputAllowed = Time.time + inputDelay;
                    }
                }

                var horAxis = Input.GetAxis("Horizontal");
                if (horAxis > 0)
                {
                    if (currentControl.name == "TextHair")
                    {
                        currentPreview = HairPreview;
                        currentSpriteArray = allHairStyles;
                    }
                    else if (currentControl.name == "TextAccessory")
                    {
                        currentPreview = AccessoryPreview;
                        currentSpriteArray = allAccessories;
                    }
                    else if (currentControl.name == "TextOutfit")
                    {
                        currentPreview = OutfitPreview;
                        currentSpriteArray = allOutfits;
                    }
                    else if (currentControl.name == "TextHairColor")
                    {
                        currentPreview = HairPreview;
                        currentColorArray = possibleHairColors;
                    }
                    else if (currentControl.name == "TextAccessoryColor")
                    {
                        currentPreview = AccessoryPreview;
                        currentColorArray = possibleAccessoryColors;
                    }

                    if (currentSpriteArray != null)
                    {
                        var currentSpriteNumber = GetNumberFromSpriteName(currentPreview.sprite.name);
                        if (currentSpriteNumber < currentSpriteArray.Length - 1)
                        {
                            currentSpriteNumber++;
                            Sprite sp = currentSpriteArray[currentSpriteNumber] as Sprite;
                            currentPreview.sprite = sp;
                        }

                        nextInputAllowed = Time.time + inputDelay;
                    }
                    else if (currentColorArray != null)
                    {
                        var currentColor = currentPreview.color;
                        int currentColorIdx = currentColorArray.IndexOf(currentColor);

                        if (currentColorIdx < currentColorArray.Count - 1)
                        {
                            currentColorIdx++;
                            currentPreview.color = currentColorArray[currentColorIdx];
                        }

                        nextInputAllowed = Time.time + inputDelay;
                    }
                }
                else if (horAxis < 0)
                {
                    if (currentControl.name == "TextHair")
                    {
                        currentPreview = HairPreview;
                        currentSpriteArray = allHairStyles;
                    }
                    else if (currentControl.name == "TextAccessory")
                    {
                        currentPreview = AccessoryPreview;
                        currentSpriteArray = allAccessories;
                    }
                    else if (currentControl.name == "TextOutfit")
                    {
                        currentPreview = OutfitPreview;
                        currentSpriteArray = allOutfits;
                    }
                    else if (currentControl.name == "TextHairColor")
                    {
                        currentPreview = HairPreview;
                        currentColorArray = possibleHairColors;
                    }
                    else if (currentControl.name == "TextAccessoryColor")
                    {
                        currentPreview = AccessoryPreview;
                        currentColorArray = possibleAccessoryColors;
                    }

                    if (currentSpriteArray != null)
                    {
                        string spritename = currentPreview.sprite.name;
                        var currentSpriteNumber = int.Parse(spritename.Split('_')[2]);
                        if (currentSpriteNumber > 0)
                        {
                            currentSpriteNumber--;
                            Sprite sp = currentSpriteArray[currentSpriteNumber] as Sprite;
                            currentPreview.sprite = sp;
                        }

                        nextInputAllowed = Time.time + inputDelay;
                    }
                    else if (currentColorArray != null)
                    {
                        var currentColor = currentPreview.color;
                        int currentColorIdx = currentColorArray.IndexOf(currentColor);

                        if (currentColorIdx > 0)
                        {
                            currentColorIdx--;
                            currentPreview.color = currentColorArray[currentColorIdx];
                        }

                        nextInputAllowed = Time.time + inputDelay;
                    }
                }
            }
        }

        /// <summary>
        /// Color from base-255 to floatbase color
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns>Color</returns>
        private Color New255Color(byte r, byte g, byte b)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
        }

        /// <summary>
        /// Save
        /// </summary>
        private void SaveCharacterSettings()
        {
            var settings = CharacterSettings.Instance();

            settings.HairStyle = GetNumberFromSpriteName(HairPreview.sprite.name);
            settings.HairColor = HairPreview.color;

            settings.Accessory = GetNumberFromSpriteName(AccessoryPreview.sprite.name);
            settings.AccessoryColor = AccessoryPreview.color;

            settings.Outfit = GetNumberFromSpriteName(OutfitPreview.sprite.name);
        }

        /// <summary>
        /// Extract number from spritename
        /// </summary>
        /// <param name="spriteName"></param>
        /// <returns>int</returns>
        private int GetNumberFromSpriteName(string spriteName)
        {
            return int.Parse(spriteName.Split('_')[2]);
        }
    }
}