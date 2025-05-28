using UnityEngine;

public class CollectablePig : MonoBehaviour
{
    [Header("Pig Settings")]
    [SerializeField] private string pigId = ""; // Unique identifier
    [SerializeField] private bool isCollected = false;
    
    [Header("Visual Feedback")]
    [SerializeField] private GameObject interactionPrompt;
    
    public string PigId => pigId;
    public bool IsCollected => isCollected;
    
    void Start()
    {
        // Generate unique ID if not set
        if (string.IsNullOrEmpty(pigId))
        {
            pigId = $"pig_{gameObject.scene.name}_{transform.position.x}_{transform.position.z}";
        }
        
        // Check if already collected
        if (PigCollectionManager.Instance != null && 
            PigCollectionManager.Instance.IsPigCollected(pigId))
        {
            gameObject.SetActive(false);
        }
        
        // Hide interaction prompt initially
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
    
    void Update()
    {
        if (PigCollectionManager.Instance == null) return;
        
        // Show/hide interaction prompt based on player proximity
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && interactionPrompt != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            bool shouldShow = distance <= PigCollectionManager.Instance.interactionRange;
            
            if (interactionPrompt.activeInHierarchy != shouldShow)
                interactionPrompt.SetActive(shouldShow);
        }
    }
    
    public void OnCollected()
    {
        isCollected = true;
        
        // Visual/audio effects here (particles, sound, etc.)
        PlayCollectionEffect();
        
        // Disable the pig
        gameObject.SetActive(false);
    }
    
    void PlayCollectionEffect()
    {
        // Add particle effects, sound, animation, etc.
        // Example:
        // if (GetComponent<AudioSource>())
        //     GetComponent<AudioSource>().Play();
    }
    
    // Helper method to set unique ID in editor
    [ContextMenu("Generate Unique ID")]
    void GenerateUniqueId()
    {
        pigId = System.Guid.NewGuid().ToString();
    }
}