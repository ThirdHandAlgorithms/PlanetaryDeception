namespace PlanetaryDeception
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Level 2
    /// </summary>
    public class LevelController_2 : LevelBase
    {
        /// <summary>
        /// Update Event handler
        /// </summary>
        public void Update()
        {
            currentInstance = this;

            if (Input.GetButton("Fire1"))
            {
                if (PlayerIsTouching("Console"))
                {
                    AlertText.text = "You have no access to this terminal";
                }
                else if (PlayerIsTouching("Door"))
                {
                    SceneManager.LoadScene("Level_1");
                }
            }
        }
    }
}