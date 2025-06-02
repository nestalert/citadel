using UnityEngine;

public class BowController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float moveRange = 2f;
    
    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public Transform firePoint; // Where arrows spawn from
    public float arrowSpeed = 10f;
    
    private Vector3 startPosition;
    private int direction = 1;
    
    void Start()
    {
        startPosition = transform.position;
    }
    
    public void Initialize()
    {
        startPosition = transform.position;
        enabled = true;
    }
    
    void Update()
    {
        // Move bow left and right
        MoveBow();
    }
    
    void MoveBow()
    {
        // Move the bow
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
        
        // Check bounds and reverse direction
        float distanceFromStart = transform.position.x - startPosition.x;
        
        if (distanceFromStart >= moveRange && direction == 1)
        {
            direction = -1;
        }
        else if (distanceFromStart <= -moveRange && direction == -1)
        {
            direction = 1;
        }
    }
    
    public void FireArrow()
    {
        if (arrowPrefab != null)
        {
            // Determine fire point (use transform if firePoint is null)
            Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
            
            // Create arrow
            GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
            
            // Add arrow component if it doesn't exist
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript == null)
            {
                arrowScript = arrow.AddComponent<Arrow>();
            }
            
            // Fire the arrow
            arrowScript.Fire(Vector2.up, arrowSpeed);
            
            // Debug.Log("Arrow fired!");
        }
    }
}