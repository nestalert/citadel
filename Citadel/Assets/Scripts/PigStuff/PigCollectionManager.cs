// PigCollectionManager.cs - Singleton to manage collected pigs across scenes
using System.Collections.Generic;
using UnityEngine;

public class PigCollectionManager : MonoBehaviour
{
    public static PigCollectionManager Instance { get; private set; }
    
    [Header("Collection Settings")]
    public KeyCode collectKey = KeyCode.Z;
    public float interactionRange = 2f;
    
    private HashSet<string> collectedPigs = new HashSet<string>();
    private Transform playerTransform;
    
    void Awake()
    {
        // Singleton pattern - persist across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Find player - adjust tag/name as needed
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(collectKey))
        {
            TryCollectNearbyPig();
        }
    }
    
    void TryCollectNearbyPig()
    {
        if (playerTransform == null) return;
        
        // Find all collectible pigs
        CollectablePig[] pigs = FindObjectsOfType<CollectablePig>();
        
        foreach (CollectablePig pig in pigs)
        {
            if (pig.gameObject.activeInHierarchy && !pig.IsCollected)
            {
                float distance = Vector3.Distance(playerTransform.position, pig.transform.position);
                if (distance <= interactionRange)
                {
                    CollectPig(pig);
                    return; // Only collect one pig at a time
                }
            }
        }
    }
    
    public void CollectPig(CollectablePig pig)
    {
        string pigId = pig.PigId;
        
        if (!collectedPigs.Contains(pigId))
        {
            collectedPigs.Add(pigId);
            pig.OnCollected();
            
            // Enable corresponding pig at stable
            EnableStablePig(pigId);
            
            Debug.Log($"Collected pig: {pigId}");
        }
    }
    
    void EnableStablePig(string pigId)
    {
        // Find the stable pig with matching ID
        StablePig[] stablePigs = FindObjectsOfType<StablePig>(true); // Include inactive objects
        
        foreach (StablePig stablePig in stablePigs)
        {
            if (stablePig.PigId == pigId)
            {
                stablePig.gameObject.SetActive(true);
                Debug.Log($"Enabled stable pig: {pigId}");
                return;
            }
        }
    }
    
    public bool IsPigCollected(string pigId)
    {
        return collectedPigs.Contains(pigId);
    }
    
    public int GetCollectedCount()
    {
        return collectedPigs.Count;
    }
    
    // Call this when scene loads to update pig states
    public void RefreshScenePigs()
    {
        // Disable already collected pigs
        CollectablePig[] pigs = FindObjectsOfType<CollectablePig>();
        foreach (CollectablePig pig in pigs)
        {
            if (collectedPigs.Contains(pig.PigId))
            {
                pig.gameObject.SetActive(false);
            }
        }
        
        // Enable stable pigs that should be enabled
        StablePig[] stablePigs = FindObjectsOfType<StablePig>(true);
        foreach (StablePig stablePig in stablePigs)
        {
            if (collectedPigs.Contains(stablePig.PigId))
            {
                stablePig.gameObject.SetActive(true);
            }
        }
    }
}
