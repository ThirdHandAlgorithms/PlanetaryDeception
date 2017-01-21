namespace UnityEngine.SceneManagement
{
    using UnityEngine;

    public static class SceneManager
    {
        private static GameObject currentScene;

        public static GameObject GetActiveScene()
        {
            return currentScene;
        }

        public static void LoadScene(string sceneName)
        {
            currentScene = new GameObject();
            currentScene.name = sceneName;
        }
    }
}
