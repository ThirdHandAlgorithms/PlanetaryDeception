namespace PlanetaryDeception
{
    using UnityEngine;

    /// <summary>
    /// Player Controller.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Direction the Character is currently facing.
        /// </summary>
        [HideInInspector]
        public bool CharacterFacingRight = true;

        /// <summary>
        /// Move force.
        /// </summary>
        public float MoveForce = 365f;

        /// <summary>
        /// Max speed.
        /// </summary>
        public float MaxSpeed = 5f;

        /// <summary>
        /// Standard Speed.
        /// </summary>
        public float StandardSpeed = 2;

        public SpriteRenderer OutfitSprite;
        public SpriteRenderer HairSprite;
        public SpriteRenderer AccessorySprite;

        /// <summary>
        /// Starts the scene.
        /// </summary>
        private void Start()
        {
            var settings = CharacterSettings.Instance();

            var allHairStyles = Resources.LoadAll<Sprite>("Sprites/hair_styles");
            var allAccessories = Resources.LoadAll<Sprite>("Sprites/hair_accessories");
            var allOutfits = Resources.LoadAll<Sprite>("Sprites/dress_styles");

            OutfitSprite.sprite = allOutfits[settings.Outfit];
            HairSprite.sprite = allHairStyles[settings.HairStyle];
            HairSprite.color = settings.HairColor;
            AccessorySprite.sprite = allAccessories[settings.Accessory];
            AccessorySprite.color = settings.AccessoryColor;
        }

        /// <summary>
        /// Fixed update called every physics update.
        /// </summary>
        private void FixedUpdate()
        {
            var horizontalAxis = Input.GetAxis("Horizontal");

            // Multiple Ifs since you can press multiple keys in the same frameupdate
            if (horizontalAxis < 0)
            {
                transform.position += Vector3.left * StandardSpeed * Time.deltaTime;
                if (CharacterFacingRight)
                {
                    FlipCharacter();
                }
            }

            if (horizontalAxis > 0)
            {
                transform.position += Vector3.right * StandardSpeed * Time.deltaTime;
                if (!CharacterFacingRight)
                {
                    FlipCharacter();
                }
            }
        }

        /// <summary>
        /// Flips the character.
        /// </summary>
        private void FlipCharacter()
        {
            CharacterFacingRight = !CharacterFacingRight;

            var renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.flipX = !CharacterFacingRight;
            }
        }
    }
}
