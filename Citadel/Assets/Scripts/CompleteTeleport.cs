using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CompleteTeleport : MonoBehaviour
{
    private static bool sceneLoaded = false;
    private Rigidbody2D persistentRb;
    private Image fadeImage;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoadedHandler;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedHandler;
    }

    void OnSceneLoadedHandler(Scene scene, LoadSceneMode mode)
    {
        persistentRb = PersistentPlayer.Instance?.PlayerRigidbody;
        fadeImage = PersistentCanvas.Instance?.FadeImage;

        if (!sceneLoaded)
        {
            StartCoroutine(RunOnSceneLoad());
            sceneLoaded = true;
        }
    }

    private IEnumerator RunOnSceneLoad()
    {
        if (fadeImage != null)
        {
            yield return StartCoroutine(Fade(0f, fadeImage));
        }
        else
        {
            Debug.LogError("Fade Image is null!");
        }

        // Unfreeze the persistent Rigidbody
        if (persistentRb != null)
        {
            persistentRb.constraints = RigidbodyConstraints2D.None;
            persistentRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public static void ResetSceneLoadedFlag()
    {
        sceneLoaded = false;
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
        while (elapsedTime < 1f)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / 1f);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the target alpha is set at the end (to prevent overshooting)
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
    }
}