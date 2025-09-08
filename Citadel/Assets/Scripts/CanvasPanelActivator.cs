using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CanvasPanelActivator : MonoBehaviour
{
    [Tooltip("Drag the Store panel GameObject here, even if it's inactive at start")]
    public GameObject panelToToggle;

    public float activationRadius = 1f;
    public string keyToPress = "z";

    private Transform playerTransform;
    private Rigidbody2D persistentRb; // <<-- Declare persistentRb as a class field here

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            // Now you can assign the Rigidbody2D to the class-level variable
            persistentRb = PersistentPlayer.Instance.PlayerRigidbody;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure the 'Player' tag is correct.");
        }

        if (panelToToggle != null)
            panelToToggle.SetActive(false); // Make sure it starts hidden
        else
            Debug.LogWarning("Panel to toggle is not assigned!");

        // Add a check to make sure the Rigidbody2D was successfully found
        if (persistentRb == null)
        {
            Debug.LogWarning("Persistent player's Rigidbody2D not found!");
        }
    }

    void Update()
    {
        // Add a check to ensure persistentRb is not null before using it
        if (playerTransform == null || panelToToggle == null || persistentRb == null)
            return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance <= activationRadius && Input.GetKeyDown(keyToPress))
        {
            bool isActive = panelToToggle.activeSelf;
            if (isActive)
            {
                GameManager.Instance.isInteractiveCanvasActive = false;
                // Unfreeze player movement
                persistentRb.constraints = RigidbodyConstraints2D.None;
                persistentRb.constraints = RigidbodyConstraints2D.FreezeRotation; // Reapply rotation freeze
            }
            else
            {
                GameManager.Instance.isInteractiveCanvasActive = true;
                // Freeze player movement
                persistentRb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            panelToToggle.SetActive(!isActive);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}