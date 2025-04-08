using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportPlayer : MonoBehaviour
{
    // The target object to teleport to
    public GameObject targetObject;

    // Reference to the Image for the fade effect
    public Image fadeImage;

    // Duration of the fade effect (in seconds)
    public float fadeDuration = 1f;

    private void Start()
    {
        // Make sure the fade image starts fully transparent
        if (fadeImage != null)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the object colliding is the player
        if (collider.CompareTag("Player"))
        {
            // Start the fade-out and teleport sequence
            StartCoroutine(FadeAndTeleport(collider));
        }
    }

    private IEnumerator FadeAndTeleport(Collider2D player)
    {
        // Fade out
        yield return StartCoroutine(Fade(1f));

        // Teleport the player to the target object's position
        player.transform.position = targetObject.transform.position;

        // Fade in
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
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
