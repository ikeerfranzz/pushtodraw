using UnityEngine;

public class eaglemovement : MonoBehaviour
{
    [SerializeField] private LayerMask visionmask;

    public Transform target1; // nest
    public Transform target2; // attack point

    public float cooldownTime = 3f;
    public float speed = 3f;

    private float cooldownTimer = 0f;

    private bool flying = false;
    private Vector2 currentTarget;

    void Update()
    {
        // Reducimos cooldown si está activo
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        Debug.DrawRay(transform.position, dir * 10f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir,
            10f,
            visionmask
        );

        bool seesPlayer = hit && hit.collider.CompareTag("player");

        // Solo puede iniciar vuelo si:
        // - Ve al jugador
        // - No está volando
        // - No está en cooldown
        if (seesPlayer && !flying && cooldownTimer <= 0f)
        {
            flying = true;

            if (Vector2.Distance(transform.position, target1.position) < 0.1f)
                currentTarget = target2.position;
            else
                currentTarget = target1.position;

            Flip();
        }

        if (flying)
        {
            Fly();

            if (Vector2.Distance(transform.position, currentTarget) < 0.05f)
            {
                flying = false;

                // Activa cooldown cuando termina el salto
                cooldownTimer = cooldownTime;
            }
        }
    }

    void Fly()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            currentTarget,
            speed * Time.deltaTime
        );
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            T_GameManager.tgameManager.damageLifes();
        }
    }
}
