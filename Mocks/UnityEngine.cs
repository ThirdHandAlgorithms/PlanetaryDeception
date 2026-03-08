namespace UnityEngine
{
    public class Color
    {
        public float r, g, b;

        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public static Color black {
            get
            {
                return new Color(0f, 0f, 0f);
            }
        }
    }

    public static class Time
    {
        public static float time {
            get
            {
                return 0f + System.DateTime.Now.ToFileTimeUtc();
            }
        }
    }

    public static class Input
    {
        public static float GetAxis(string direction)
        {
            return 0f;
        }

        public static bool GetButtonDown(string buttonname)
        {
            return false;
        }

        public static bool GetButton(string buttonname)
        {
            return false;
        }

        public static bool GetButtonUp(string buttonname)
        {
            return false;
        }
    }

    public class CanvasGroup
    {
        public float alpha { get; set; }
    }

    public class GameObject
    {
        public string name { get; set; }
    }

    public class Collider2D
    {
    }

    public class Position
    {
        public float x, y, z;
    }

    public class Transform
    {
        public Position position;
    }

    public class SpriteRenderer
    {
        public Transform transform;
        public bool flipX, flipY;
    }

    public class TextAsset
    {
        public string text { get; set; }
    }

    public static class Resources
    {
        public static string BasePath { get; set; } = "Assets/resources";

        public static object Load(string name)
        {
            var path = System.IO.Path.Combine(BasePath, name + ".txt");
            if (System.IO.File.Exists(path))
            {
                return new TextAsset { text = System.IO.File.ReadAllText(path) };
            }

            return new TextAsset { text = string.Empty };
        }
    }

    public static class Application
    {
        public static string persistentDataPath
        {
            get { return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), ".PlanetaryDeception"); }
        }
    }

    public class MonoBehaviour
    {
        public GameObject gameObject { get; set; }

        public T GetComponent<T>() where T : new()
        {
            return new T();
        }
    }
}
