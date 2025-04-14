using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothing = 100000f; // Adjust for smoother camera movement

    private Vector3 offset;
    private Transform target;

    void Start()
    {
        // Find the PersistentPlayer instance
        if (PersistentPlayer.Instance == null || PersistentPlayer.Instance.transform == null)
        {
            Debug.LogError("PersistentPlayer instance not found or doesn't have a transform for CameraFollow!");
            enabled = false; // Disable the script if no PersistentPlayer is found
            return;
        }

        target = PersistentPlayer.Instance.transform;

        // Calculate the initial offset between the camera and the target
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Re-check for the target in LateUpdate in case of scene changes
        if (target == null && PersistentPlayer.Instance != null && PersistentPlayer.Instance.transform != null)
        {
            target = PersistentPlayer.Instance.transform;
            // Recalculate offset if the target was just found
            offset = transform.position - target.position;
        }

        if (target == null)
        {
            return; // Don't do anything if the target is still gone
        }

        // Calculate the desired position of the camera
        Vector3 targetCamPos = target.position + offset;

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}