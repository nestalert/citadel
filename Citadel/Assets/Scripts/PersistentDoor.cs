using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentDoor : MonoBehaviour
{
    private static PersistentDoor instance;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}