namespace PlanetaryDeception
{
    using UnityEngine;

    /// <summary>
    /// Connect this to a gameobject with a Collision2D component to set/unset the current level's PlayerInContactWith state
    /// </summary>
    public class BasicCollision : MonoBehaviour
    {
        /// <summary>
        /// Eventhandler when Player touches the Collision2D area
        /// </summary>
        /// <param name="item"></param>
        public void OnTriggerEnter2D(Collider2D item)
        {
            var level = LevelBase.CurrentLevel();

            level.PlayerInContactWith = gameObject;
        }

        /// <summary>
        /// Eventhandler when Player exits the Collision2D area
        /// </summary>
        /// <param name="item"></param>
        public void OnTriggerExit2D(Collider2D item)
        {
            var level = LevelBase.CurrentLevel();

            if (level.PlayerInContactWith == gameObject)
            {
                level.PlayerInContactWith = null;
            }
        }
    }
}