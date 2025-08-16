using UnityEngine;

public class CanvasPanelActivator : MonoBehaviour
{
    [Tooltip("Drag the Store panel GameObject here, even if it's inactive at start")]
    public GameObject panelToToggle;

    public float activationRadius = 1f;
    public string keyToPress = "z";

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;

        if (panelToToggle != null)
            panelToToggle.SetActive(false);  // Make sure it starts hidden
        else
            Debug.LogWarning("Panel to toggle is not assigned!");
    }

    void Update()
    {
        if (playerTransform == null || panelToToggle == null)
            return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance <= activationRadius && Input.GetKeyDown(keyToPress))
        {
            bool isActive = panelToToggle.activeSelf;
            panelToToggle.SetActive(!isActive);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}
