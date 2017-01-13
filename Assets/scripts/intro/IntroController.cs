namespace PlanetaryDeception
{
    using UnityEngine;

    /// <summary>
    /// Intro Controller.
    /// Does nothing for now, just waits for key input to launch the game.
    /// </summary>
    public class IntroController : MonoBehaviour
    {
        /// <summary>
        /// Update this instance. Press any key..
        /// </summary>
        public void Update()
        {
            if (Input.anyKey)
            {
                var settings = CharacterSettings.Instance();
                settings.TransitionToNewScene("CharacterCreation", null);
            }
        }
    }
}
