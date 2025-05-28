using UnityEngine;
using UnityEngine.SceneManagement;

public class PigSceneManager : MonoBehaviour
{
    void Start()
    {
        // Refresh pig states when scene loads
        if (PigCollectionManager.Instance != null)
        {
            PigCollectionManager.Instance.RefreshScenePigs();
        }
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find player reference again in new scene
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && PigCollectionManager.Instance != null)
        {
            // Update player reference and refresh pigs
            Invoke(nameof(DelayedRefresh), 0.1f); // Small delay to ensure everything is loaded
        }
    }
    
    void DelayedRefresh()
    {
        if (PigCollectionManager.Instance != null)
        {
            PigCollectionManager.Instance.RefreshScenePigs();
        }
    }
}