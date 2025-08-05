using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    [Header("Target Settings")]
    public int pointValue = 1;
    public GameObject hitEffect; // Optional particle effect
    public AudioClip hitSound; // Optional sound effect
    
    [Header("Visual Feedback")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.2f;

    [Header("Shake Feedback")]
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.05f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private AudioSource audioSource;
    
    // Static score tracking
    public static int totalScore = 0;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && hitSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    public void OnHit()
    {
        CurrencyManager.Instance.AddMoney(1);

        totalScore += pointValue;

        // Visual feedback
        StartCoroutine(FlashTarget());
        StartCoroutine(Shake(shakeDuration, shakeMagnitude));


        // Sound effect
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        
        // Particle effect
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        
    }
    
    System.Collections.IEnumerator FlashTarget()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            OnHit();
        }
    }
    
    // Static method to get current score
    public static int GetScore()
    {
        return totalScore;
    }
    
    // Static method to reset score
    public static void ResetScore()
    {
        totalScore = 0;
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}