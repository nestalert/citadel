using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFired = false;
    
    [Header("Arrow Settings")]
    public float lifetime = 5f; // How long arrow exists before destroying
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // Configure rigidbody
        rb.gravityScale = 0f; // No gravity for straight shot
        rb.drag = 0f;
    }
    
    void Start()
    {
        // Destroy arrow after lifetime
        Destroy(gameObject, lifetime);
    }
    
    public void Fire(Vector2 direction, float speed)
    {
        if (!hasFired)
        {
            rb.velocity = direction.normalized * speed;
            hasFired = true;
            
            // Rotate arrow to face movement direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if hit target
        if (other.CompareTag("Target"))
        {
            Target target = other.GetComponent<Target>();
            if (target != null)
            {
                target.OnHit();
            }
            
            // Destroy arrow on impact
            Destroy(gameObject);
        }
        // Also destroy if hits walls or other obstacles
        else if (other.CompareTag("Wall") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Alternative collision detection
        if (collision.gameObject.CompareTag("Target"))
        {
            Target target = collision.gameObject.GetComponent<Target>();
            if (target != null)
            {
                target.OnHit();
            }
        }
        
        // Destroy arrow on any collision
        Destroy(gameObject);
    }
}