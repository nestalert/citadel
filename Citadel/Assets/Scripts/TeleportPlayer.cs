using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TeleportPlayer : MonoBehaviour
{
    // The target object to teleport to (still useful for position)
    public GameObject targetObject;

    // The name of the scene to load
    public string sceneToLoad;

    // Duration of the fade effect (in seconds)
    public float fadeDuration = 1f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the object colliding is the persistent player
        if (collider.CompareTag("Player") && PersistentPlayer.Instance != null && PersistentCanvas.Instance != null && PersistentCanvas.Instance.FadeImage != null && !string.IsNullOrEmpty(sceneToLoad))
        {
            // Start the fade-out and scene load sequence
            StartCoroutine(FadeAndLoadScene(PersistentPlayer.Instance.GetComponent<Collider2D>(), PersistentCanvas.Instance.FadeImage));
        }
        else
        {
            if (PersistentCanvas.Instance == null || PersistentCanvas.Instance.FadeImage == null)
            {
                Debug.LogError("Persistent Canvas or Fade Image not found!");
            }
            if (string.IsNullOrEmpty(sceneToLoad))
            {
                Debug.LogError("Scene to Load is not specified on " + gameObject.name + "!");
            }
        }
    }

    private IEnumerator FadeAndLoadScene(Collider2D playerCollider, Image fadeImage)
    {
        // Get the Rigidbody2D from the persistent Player instance
        Rigidbody2D persistentRb = PersistentPlayer.Instance.PlayerRigidbody;

        // Freeze the persistent Rigidbody
        if (persistentRb != null)
        {
            persistentRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Fade out using the persistent fadeImage
        yield return StartCoroutine(Fade(1f, fadeImage));
        
        // Load the new scene additively
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        while (!loadOperation.isDone)
        {
            yield return null;
        }
        // Find the new player in the loaded scene (assuming it has the "Player" tag)
        GameObject newPlayer = GameObject.FindGameObjectWithTag("Player");
        if (newPlayer != null && targetObject != null)
        {
            newPlayer.transform.position = targetObject.transform.position;
        }
        else if (targetObject == null)
        {
            Debug.LogWarning("Target Object is not set. New player will be at the scene's default spawn.");
        }
        else
        {
            Debug.LogError("Could not find a Player with the 'Player' tag in the loaded scene!");
        }

        // Fade in using the persistent fadeImage
        yield return StartCoroutine(Fade(0f, fadeImage));

        // Unfreeze the persistent Rigidbody
        if (persistentRb != null)
        {
            persistentRb.constraints = RigidbodyConstraints2D.None;
            persistentRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private IEnumerator Fade(float targetAlpha, Image fadeImage)
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image is null!");
            yield break;
        }

        // Get the current alpha value of the image
        float startAlpha = fadeImage.color.a;

        // Lerp the alpha from start to target over the duration of the fade
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the target alpha is set at the end (to prevent overshooting)
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
    }
}