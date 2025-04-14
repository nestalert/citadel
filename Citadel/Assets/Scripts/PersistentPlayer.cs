using UnityEngine;

public class PersistentPlayer : MonoBehaviour
{
    public static PersistentPlayer Instance { get; private set; }
    public Rigidbody2D PlayerRigidbody { get; private set; }

    void Awake()
    {
        // Singleton pattern to ensure only one Player persists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Get the Rigidbody2D component once and store it
        PlayerRigidbody = GetComponent<Rigidbody2D>();

        if (PlayerRigidbody == null)
        {
            Debug.LogError("Rigidbody2D not found on the Persistent Player!");
        }
    }

    // You can add other persistent player data or methods here
}