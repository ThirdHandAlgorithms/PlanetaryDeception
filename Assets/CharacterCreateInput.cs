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
        /// Skin/player sprite
        /// </summary>
        public SpriteRenderer SkinPreview;

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
        /// List of skin colors
        /// </summary>
        private List<Color> possibleSkinColors;

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

            possibleSkinColors = new List<Color>();

            possibleSkinColors.Add(New255Color(0xCDA184));
            possibleSkinColors.Add(New255Color(0x93614A));
            possibleSkinColors.Add(New255Color(0x875B3E));
            possibleSkinColors.Add(New255Color(0x764630));
            possibleSkinColors.Add(New255Color(0x925C36));
            possibleSkinColors.Add(New255Color(0x582812));
            possibleSkinColors.Add(New255Color(0x4C2D18));

            possibleSkinColors.Add(New255Color(0xF6E4E2));
            possibleSkinColors.Add(New255Color(0xF5DAD3));
            possibleSkinColors.Add(New255Color(0xF7D6C7));
            possibleSkinColors.Add(New255Color(0xE89A84));
            possibleSkinColors.Add(New255Color(0xECD3D9));
            possibleSkinColors.Add(New255Color(0xDAA48A));
            possibleSkinColors.Add(New255Color(0xEBBFA6));

            possibleSkinColors.Add(New255Color(0xFFD1C1));
            possibleSkinColors.Add(New255Color(0xD9A494));
            possibleSkinColors.Add(New255Color(0xF5DAD3));
            possibleSkinColors.Add(New255Color(0xFFD0BC));
            possibleSkinColors.Add(New255Color(0xF1E4D1));
            possibleSkinColors.Add(New255Color(0xF5AF95));
            possibleSkinColors.Add(New255Color(0xE6A680));


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
                    else if (currentControl.name == "TextSkinColor")
                    {
                        currentPreview = SkinPreview;
                        currentColorArray = possibleSkinColors;
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
                    else if (currentControl.name == "TextSkinColor")
                    {
                        currentPreview = SkinPreview;
                        currentColorArray = possibleSkinColors;
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
        /// <param name="r">byte</param>
        /// <param name="g">byte</param>
        /// <param name="b">byte</param>
        /// <returns>Color</returns>
        private Color New255Color(byte r, byte g, byte b)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
        }

        /// <summary>
        /// Color from base-255 to floatbase color
        /// </summary>
        /// <param name="rgb">int</param>
        /// <returns>Color</returns>
        private Color New255Color(int rgb)
        {
            byte r = (byte)((rgb & 0xff0000) >> 16);
            byte g = (byte)((rgb & 0x00ff00) >> 8);
            byte b = (byte)((rgb & 0x0000ff));

            return New255Color(r, g, b);
        }

        /// <summary>
        /// Save
        /// </summary>
        private void SaveCharacterSettings()
        {
            var settings = CharacterSettings.Instance();

            settings.SkinColor = SkinPreview.color;

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