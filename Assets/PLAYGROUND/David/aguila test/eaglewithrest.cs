using UnityEngine;

public class EagleWithRest : MonoBehaviour
{
    [SerializeField] private LayerMask visionMask;

    [Header("Targets")]
    public Transform nest; // Nest
    public Transform attackPoint; // Attack point

    [Header("Movement")]
    public float speed = 3f;

    [Header("Jump Cooldown")]
    public float jumpCooldown = 3f;
    private float jumpTimer;

    [Header("Active/Inactive Cycle")]
    public float stateDuration = 3f;
    private float stateTimer;
    private bool isActive = true;

    private bool flying = false;
    private Vector2 currentTarget;
    private SpriteRenderer spriteRenderer;

    [Header("Audio")]
    [SerializeField] private AudioClip flySound;
    private AudioSource audioSource;
    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("EagleWithRest: No SpriteRenderer found on this object.");
        }
    }

    void Start()
    {
        stateTimer = stateDuration;
        jumpTimer = 0f;

        if (nest == null || attackPoint == null)
        {
            Debug.LogError("EagleWithRest: nest or attackPoint not assigned in the inspector.");
        }
    }

    void Update()
    {
        HandleStateCycle();
        HandleJumpCooldown();

        // If flying, continue the flight without interruptions
        if (flying)
        {
            ContinueFlight();
            return;
        }

        // If inactive, cannot start a new flight
        if (!isActive)
            return;

        DetectPlayerAndStartFlight();
    }

    void HandleStateCycle()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            // Toggle active/inactive state
            isActive = !isActive;
            stateTimer = stateDuration;

            // Change color even if flying
            if (spriteRenderer != null)
                spriteRenderer.color = isActive ? Color.white : Color.gray;
        }
    }

    void HandleJumpCooldown()
    {
        if (jumpTimer > 0f)
            jumpTimer -= Time.deltaTime;
    }

    void DetectPlayerAndStartFlight()
    {
        
        if (nest == null || attackPoint == null)
            return;

        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            10f,
            LayerMask.GetMask("Player") | visionMask.value
        );

        if (hit.collider != null && hit.collider.CompareTag("player") && jumpTimer <= 0f)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            flying = true;

            if (Vector2.Distance(transform.position, nest.position) < 0.1f)
                currentTarget = attackPoint.position;
            else
                currentTarget = nest.position;

            Flip();
        }
    }

    void ContinueFlight()
    {
        if (nest == null || attackPoint == null)
            return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            currentTarget,
            speed * Time.deltaTime
        );

        // When flight ends
        if (Vector2.Distance(transform.position, currentTarget) < 0.05f)
        {
            flying = false;
            jumpTimer = jumpCooldown;

            // If inactive, stays gray and cannot start new flight
            if (!isActive && spriteRenderer != null)
            {
                spriteRenderer.color = Color.gray;
            }
            else if (isActive && spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
            }
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only deal damage if active
        if (!isActive) return;

        if (collision.CompareTag("player"))
        {
            GameManager.gameManager.damageLifes();
        }
    }
}
