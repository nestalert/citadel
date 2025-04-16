using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MapTeleportButton : MonoBehaviour
{
    [Tooltip("The coordinates to teleport the player to.")]
    public Vector2 teleportPosition = Vector2.zero;

    [Tooltip("The name of the scene to load.")]
    public string sceneToLoad;

    [Tooltip("The tag of the player GameObject.")]
    public string playerTag = "Player";

    public void TeleportPlayerAndLoadScene()
    {
        // Find the player GameObject using its tag
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        // Check if the player was found
        if (player != null)
        {   
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            // Teleport the player to the specified position
            player.transform.position = teleportPosition;
        }
        else
        {
            Debug.LogError("Player with tag '" + playerTag + "' not found!");
        }
    }
}