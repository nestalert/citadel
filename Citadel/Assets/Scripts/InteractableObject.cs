using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Canvas targetCanvas;
    public float interactionRadius = 1f;
    public string interactionButton = "Z";
    public AudioClip interactionSound; // Optional audio clip
    public string targetButtonName;

    private Transform playerTransform;
    private AudioSource audioSource; // To play the sound

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

        // Setup the audio source if an audio clip is provided
        if (interactionSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = interactionSound;
            audioSource.playOnAwake = false; // Don't play until we tell it to
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
                UIManager.Instance.ActivateButton(targetButtonName);
                bool isCanvasActive = targetCanvas.gameObject.activeSelf;
                targetCanvas.gameObject.SetActive(!isCanvasActive); // Toggle canvas visibility

                // Play or stop the audio based on canvas state
                if (interactionSound != null)
                {
                    if (!isCanvasActive)
                    {
                        audioSource.Play();
                    }
                    else
                    {
                        audioSource.Stop();
                    }
                }
            }
        }
        else
        {
            // Player is out of range, hide the canvas and stop the audio
            targetCanvas.gameObject.SetActive(false);
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    // Optional: For visual feedback in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}