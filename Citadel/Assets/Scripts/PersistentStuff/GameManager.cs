// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // A singleton instance for easy access from anywhere
    public static GameManager Instance { get; private set; }

    // This flag will be true if any of the specified canvases is active
    public bool isInteractiveCanvasActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Optional: To persist across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}