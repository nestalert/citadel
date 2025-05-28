using UnityEngine;

public class StablePig : MonoBehaviour
{
    [Header("Stable Pig Settings")]
    [SerializeField] private string pigId = ""; // Must match CollectablePig ID
    
    public string PigId => pigId;
    
    void Start()
    {
        // Ensure this pig starts disabled unless already collected
        if (PigCollectionManager.Instance == null || 
            !PigCollectionManager.Instance.IsPigCollected(pigId))
        {
            gameObject.SetActive(false);
        }
    }
}