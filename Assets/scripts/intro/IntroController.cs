using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene("Level_1");
        }
    }
}
