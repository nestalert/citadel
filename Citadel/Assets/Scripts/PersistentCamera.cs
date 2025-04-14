using UnityEngine;

public class PersistentCamera : MonoBehaviour
{
    private static PersistentCamera _instance;
    public static PersistentCamera Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}