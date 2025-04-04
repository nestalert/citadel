using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Assign your player's Transform in the Inspector
    public float smoothing = 5f; // Adjust for smoother camera movement

    private Vector3 offset;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target (Player) not assigned to CameraFollow script!");
            enabled = false; // Disable the script if no target is set
            return;
        }

        // Calculate the initial offset between the camera and the target
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return; // Don't do anything if the target is gone
        }

        // Calculate the desired position of the camera
        Vector3 targetCamPos = target.position + offset;

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}