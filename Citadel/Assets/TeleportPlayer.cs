using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [Tooltip("The GameObject to which colliding objects will be teleported.")]
    public Transform destination;

    void OnCollisionEnter(Collision collision)
    {
        if (destination != null)
        {
            collision.transform.position = destination.position;
            // Optionally, you might want to adjust the rotation as well:
            // collision.transform.rotation = destination.rotation;

            Debug.Log($"Teleported {collision.gameObject.name} to {destination.name}");
        }
        else
        {
            Debug.LogError("Target Teleport Location is not assigned on " + gameObject.name);
        }
    }

    void onTriggerEnter2D(Collider other)
    {
        if (destination != null)
        {
            other.transform.position = destination.position;
            // Optionally, you might want to adjust the rotation as well:
            // other.transform.rotation = destination.rotation;

            Debug.Log($"Teleported {other.gameObject.name} to {destination.name} (Trigger)");
        }
        else
        {
            Debug.LogError("Target Teleport Location is not assigned on " + gameObject.name);
        }
    }
}