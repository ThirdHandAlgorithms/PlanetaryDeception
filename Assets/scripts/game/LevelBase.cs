namespace PlanetaryDeception
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Base class for Level controllers
    /// </summary>
    public class LevelBase : MonoBehaviour
    {
        /// <summary>
        /// Connect to Text gameobject in Unity
        /// </summary>
        public Text AlertText;

        /// <summary>
        /// The last level that was instantiated
        /// </summary>
        protected static LevelBase currentInstance = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public LevelBase()
        {
            Start();
        }

        /// <summary>
        /// Level start, if you reimplement Start, you need to manually execute this
        /// </summary>
        public virtual void Start()
        {
            PlayerInContactWith = null;
            currentInstance = this;
        }

        /// <summary>
        /// Indication of what the Player is currently touching
        /// </summary>
        public GameObject PlayerInContactWith { get; set; }

        /// <summary>
        /// Returns the current level
        /// </summary>
        /// <returns>LevelBase</returns>
        public static LevelBase CurrentLevel()
        {
            return currentInstance;
        }

        /// <summary>
        /// Returns true when the last gameobject the Player touched was called objectName
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns>bool</returns>
        public bool PlayerIsTouching(string objectName)
        {
            return (PlayerInContactWith != null) && (PlayerInContactWith.name == objectName);
        }
    }
}
