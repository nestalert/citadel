using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Target Settings")]
    public int pointValue = 1;
    public GameObject hitEffect; // Optional particle effect
    public AudioClip hitSound; // Optional sound effect
    
    [Header("Visual Feedback")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.2f;
    
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
        
        // Visual feedback
        StartCoroutine(FlashTarget());
        
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
}