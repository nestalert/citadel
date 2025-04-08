using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Canvas targetCanvas;
    public float interactionRadius = 1f;
    public string interactionButton = "Z";

    private Transform playerTransform;

    void Start()
    {
        // Try to find a player with a specific tag, or adjust as needed
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("No GameObject tagged 'Player' found in the scene. Please tag your player object.");
            enabled = false; // Disable the script if no player is found
        }

        // Ensure the canvas is initially hidden (though you should set this in the Inspector too)
        if (targetCanvas != null)
        {
            targetCanvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Target Canvas is not assigned in the Inspector on " + gameObject.name);
            enabled = false; // Disable the script if no canvas is assigned
        }
    }

    void Update()
{
    if (playerTransform == null || targetCanvas == null)
    {
        return; // Exit if player or canvas isn't set
    }

    float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

    if (distanceToPlayer <= interactionRadius)
    {
        // Player is within range
        if (Input.GetKeyDown(KeyCode.Z)) // Directly check for the 'Z' key
        {
            targetCanvas.gameObject.SetActive(true);
        }
    }
    else
    {
        // Player is out of range, hide the canvas
        targetCanvas.gameObject.SetActive(false);
    }
}

    // Optional: For visual feedback in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}