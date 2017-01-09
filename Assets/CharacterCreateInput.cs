namespace PlanetaryDeception
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class CharacterCreateInput: MonoBehaviour
    {
        public SpriteRenderer HairPreview;
        public SpriteRenderer OutfitPreview;
        public SpriteRenderer AccessoryPreview;

        private List<Text> controls;
        private Text currentControl;
        private float nextInputAllowed = 0.0f;

        public void Start()
        {
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

        public void Update()
        {
            currentControl.color = Color.red;

            if (Time.time >= nextInputAllowed)
            {
                nextInputAllowed = Time.time + 0.3F;

                var verAxis = Input.GetAxis("Vertical");
                if (verAxis > 0)
                {
                    var idx = controls.IndexOf(currentControl);
                    if (idx > 0)
                    {
                        idx--;
                        currentControl.color = Color.white;
                        currentControl = controls[idx];
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
                    }
                }

                var horAxis = Input.GetAxis("Horizontal");
                if (horAxis > 0)
                {
                    if (currentControl.name == "TextHair")
                    {
                        string spritename = HairPreview.sprite.name;
                        var number = int.Parse(spritename.Split('_')[2]);
                        if (number < 10)
                        {
                            number++;
                            Sprite sp = Resources.LoadAll<Sprite>("Sprites/hair_styles")[number] as Sprite;
                            HairPreview.sprite = sp;
                        }
                    }
                }
                else if (horAxis < 0)
                {

                }
            }
        }
    }
}