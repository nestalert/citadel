using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    [Header("Mini-Game Settings")]
    public Transform miniGamePosition; // Where to teleport the player
    public GameObject bowAndArrowPrefab; // The bow prefab
    public GameObject targetPrefab; // The target prefab
    public float interactionRadius = 2f;
    
    [Header("Camera Settings")]
    public Camera miniGameCamera; // Camera for the mini-game
    public Camera mainCamera = null;
    
    [Header("References")]
    public Transform player = null;
    
    private Vector3 originalPlayerPosition;
    private bool playerInRange = false;
    private bool miniGameActive = false;
    private GameObject currentBow;
    private GameObject currentTarget;
    private BowController bowController;
    
void Start()
{
    // Auto-find main camera if not assigned
    if (mainCamera == null)
    {
        // Check for the singleton instance of the persistent camera
        if (PersistentCamera.Instance != null)
        {
            mainCamera = PersistentCamera.Instance.GetComponent<Camera>();
        }
        else
        {
            // Fallback for cases where the persistent camera isn't set up as a singleton
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = FindObjectOfType<Camera>();
            }
        }
    }
    
    // Ensure mini-game camera starts disabled
    if (miniGameCamera != null)
    {
        miniGameCamera.gameObject.SetActive(false);
    }
}
    
void Update()
{
    player = PersistentPlayer.Instance.transform;
    if (player != null)
    {
        float distance = Vector2.Distance(transform.position, player.position);
        playerInRange = distance <= interactionRadius;
    }
    
    // Handle input
    if (Input.GetKeyDown(KeyCode.Z))
    {
        if (!miniGameActive && playerInRange)
        {
            StartMiniGame();
        }
        else if (miniGameActive && bowController != null)
        {
            bowController.FireArrow();
        }
    }
    
    if (Input.GetKeyDown(KeyCode.X) && miniGameActive)
    {
        EndMiniGame();
    }
}

    void StartMiniGame()
    {
        // Store original position
        originalPlayerPosition = player.position;
        
        // Switch cameras
        SwitchToMiniGameCamera();
        
        // Teleport player (position them behind the bow)
        player.position = miniGamePosition.position + Vector3.down * 1f;
        
        // Lock rigidbody using PersistentPlayer reference
        Rigidbody2D playerRigidbody = PersistentPlayer.Instance.PlayerRigidbody;
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
        // Spawn bow and target (bow at mini-game position, target above)
        currentBow = Instantiate(bowAndArrowPrefab, miniGamePosition.position, Quaternion.identity);
        currentTarget = Instantiate(targetPrefab, miniGamePosition.position + Vector3.up * 5f, Quaternion.identity);
        
        // Get bow controller
        bowController = currentBow.GetComponent<BowController>();
        if (bowController != null)
        {
            bowController.Initialize();
        }
        
        miniGameActive = true;
        Debug.Log("Mini-game started! Press Z to fire, X to exit.");
    }
    
    void EndMiniGame()
    {
        // Switch back to main camera
        SwitchToMainCamera();
        
        // Return player to original position
        player.position = originalPlayerPosition;
        
        // Unfreeze rigidbody using PersistentPlayer reference
        Rigidbody2D playerRigidbody = PersistentPlayer.Instance.PlayerRigidbody;
        if (playerRigidbody != null)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.None;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        // Clean up mini-game objects
        if (currentBow != null)
            Destroy(currentBow);
        if (currentTarget != null)
            Destroy(currentTarget);
        
        miniGameActive = false;
        Debug.Log("Mini-game ended!");
    }
        
    void SwitchToMiniGameCamera()
    {
        if (mainCamera != null && miniGameCamera != null)
        {
            mainCamera.gameObject.SetActive(false);
            miniGameCamera.gameObject.SetActive(true);
            
            // Position mini-game camera to show the full mini-game area
            Vector3 cameraPosition = miniGamePosition.position + Vector3.back * 10f + Vector3.up * 1f;
            miniGameCamera.transform.position = cameraPosition;
            
            Debug.Log("Switched to mini-game camera");
        }
    }
    
    void SwitchToMainCamera()
    {
        if (mainCamera != null && miniGameCamera != null)
        {
            miniGameCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            
            Debug.Log("Switched back to main camera");
        }
    }
    
    void OnDrawGizmos()
    {
        // Draw interaction radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
        
        // Draw mini-game position
        if (miniGamePosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(miniGamePosition.position, Vector3.one * 0.5f);
        }
    }
}