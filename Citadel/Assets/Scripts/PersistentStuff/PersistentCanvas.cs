using UnityEngine;
using UnityEngine.UI;

public class PersistentCanvas : MonoBehaviour
{
    public static PersistentCanvas Instance { get; private set; }
    public Image FadeImage { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Find the Image component on this Canvas or a child
        FadeImage = GetComponentInChildren<Image>();

        if (FadeImage != null)
        {
            // Forcefully set the initial alpha to 0 (invisible)
            Color initialColor = FadeImage.color;
            initialColor.a = 0f;
            FadeImage.color = initialColor;
        }
        else
        {
            Debug.LogError("Fade Image not found on or within the Persistent Canvas!");
        }
    }
}