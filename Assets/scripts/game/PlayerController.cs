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

        /// <summary>
        /// Player sprite
        /// </summary>
        public SpriteRenderer PlayerSprite;

        /// <summary>
        /// Select sprite with the Player's Outfit
        /// </summary>
        public SpriteRenderer OutfitSprite;

        /// <summary>
        /// Sprite for the Player's Hair
        /// </summary>
        public SpriteRenderer HairSprite;

        /// <summary>
        /// Sprite for the Player's Accessory
        /// </summary>
        public SpriteRenderer AccessorySprite;

        /// <summary>
        /// outfitAnimName
        /// </summary>
        private string outfitAnimPrefix;

        /// <summary>
        /// player animator
        /// </summary>
        private Animator playerAnim;

        /// <summary>
        /// outfit animator
        /// </summary>
        private Animator outfitAnim;

        /// <summary>
        /// Starts the scene.
        /// </summary>
        private void Start()
        {
            var settings = CharacterSettings.Instance();

            var allHairStyles = Resources.LoadAll<Sprite>("Sprites/hair_styles");
            var allAccessories = Resources.LoadAll<Sprite>("Sprites/hair_accessories");
            var allOutfits = Resources.LoadAll<Sprite>("Sprites/dress_styles");

            PlayerSprite.color = settings.SkinColor;

            OutfitSprite.sprite = allOutfits[settings.Outfit];
            HairSprite.sprite = allHairStyles[settings.HairStyle];
            HairSprite.color = settings.HairColor;
            AccessorySprite.sprite = allAccessories[settings.Accessory];
            AccessorySprite.color = settings.AccessoryColor;

            outfitAnimPrefix = "outfit" + settings.Outfit;

            playerAnim = PlayerSprite.GetComponent<Animator>();
            outfitAnim = OutfitSprite.GetComponent<Animator>();

            LoadCharacterPositionForThisScene();
        }

        /// <summary>
        /// Load Character X, Y and flip
        /// </summary>
        private void LoadCharacterPositionForThisScene()
        {
            var sceneSettings = CharacterSettings.Instance().GetCurrentSceneSettings();
            if (sceneSettings.PlayerPosIsSet)
            {
                transform.position = new Vector3(sceneSettings.PlayerPosX, sceneSettings.PlayerPosY, 0);

                if (!sceneSettings.PlayerIsFacingRight)
                {
                    FlipCharacter();
                }
            }
        }

        /// <summary>
        /// Fixed update called every physics update.
        /// </summary>
        private void FixedUpdate()
        {
            if (StandardSpeed > 0)
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

                    playerAnim.Play("player_walking");
                    outfitAnim.Play(outfitAnimPrefix + "_walking");
                }
                else if (horizontalAxis > 0)
                {
                    transform.position += Vector3.right * StandardSpeed * Time.deltaTime;
                    if (!CharacterFacingRight)
                    {
                        FlipCharacter();
                    }

                    playerAnim.Play("player_walking");
                    outfitAnim.Play(outfitAnimPrefix + "_walking");
                }
                else
                {
                    playerAnim.Play("player_idle");
                    outfitAnim.Play(outfitAnimPrefix + "_idle");
                }
            }
            else
            {
                playerAnim.Play("player_idle");
                outfitAnim.Play(outfitAnimPrefix + "_idle");
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
