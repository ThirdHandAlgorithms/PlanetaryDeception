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
        /// Sprite Renderer of the Player GameObject.
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// Starts the scene.
        /// </summary>
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
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
            spriteRenderer.flipX = !CharacterFacingRight;
        }
    }
}
