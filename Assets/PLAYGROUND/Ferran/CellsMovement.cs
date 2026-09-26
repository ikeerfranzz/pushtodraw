using Unity.VisualScripting;
using UnityEngine;

public class CellsMovement : MonoBehaviour
{
    public enum FaceDirection { DOWN, LEFT, RIGHT, UP, NONE };

    [SerializeField] private float movementSpeed;
    [SerializeField] private float retrocesoSpeed; // Velocidad de retroceso
    [SerializeField] private Vector2 movementPoint;
    [SerializeField] private Vector2 offsetMovementPoint;
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private float circleRadius;
    [SerializeField] private float moveCooldown = 1f;

    private float cooldownTimer = 0f;
    private bool returning = false;

    private SpriteRenderer spriteRenderer;
    private bool moving = false;
    private bool grabbingBox = false;
    private Vector2 input;
    public FaceDirection facing = FaceDirection.DOWN;

    private GameObject grabbedBox = null;
    public Vector2 lastDirection { get; private set; }
    public Vector2 lastPosition;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementPoint = transform.position;
        lastPosition = movementPoint; // Posición inicial segura
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            returning = true;
            moving = false; // Cancelamos movimiento normal
        }
    }

    private void Update()
    {
        // ---------- RETROCESO ----------
        if (returning)
        {
            transform.position = Vector2.MoveTowards(transform.position, lastPosition, retrocesoSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, lastPosition) < 0.01f)
            {
                returning = false;
                movementPoint = lastPosition; // Reset del movementPoint
            }

            return; // Bloquea input mientras retrocede
        }

        // ---------- INPUT ----------
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        // ---------- MOVIMIENTO NORMAL ----------
        if (moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementPoint, movementSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, movementPoint) < 0.01f)
                moving = false;
        }

        // ---------- MOVIMIENTO CUANDO NO SE AGARRA CAJA ----------
        if (!grabbingBox)
        {
            if ((input.x != 0 ^ input.y != 0) && !moving && cooldownTimer <= 0f)
            {
                lastPosition = transform.position; // Guardamos posición segura antes de movernos
                lastDirection = input;

                if (input.x > 0) facing = FaceDirection.RIGHT;
                else if (input.x < 0) facing = FaceDirection.LEFT;
                else if (input.y > 0) facing = FaceDirection.UP;
                else if (input.y < 0) facing = FaceDirection.DOWN;

                UpdateGraphic();

                Vector2 evaluatePoint = (Vector2)transform.position + offsetMovementPoint + input;
                Collider2D[] hits = Physics2D.OverlapCircleAll(evaluatePoint, circleRadius);

                bool canMove = true;
                foreach (Collider2D hit in hits)
                {
                    if (hit.isTrigger) { continue; }
                    if (hit != null)
                    {
                        canMove = false;
                    }
                }

                if (canMove)
                {
                    moving = true;
                    movementPoint += input;
                    cooldownTimer = moveCooldown;
                }
            }

            // Intentar agarrar caja
            if (Input.GetKey(KeyCode.LeftShift) && !moving)
            {
                grabbedBox = null;
                Vector2 direction = GetFacingDirection();
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, obstacles);

                if (hit.collider != null && hit.collider.CompareTag("object"))
                {
                    grabbedBox = hit.collider.gameObject;
                    grabbingBox = true;
                }
            }
            else grabbedBox = null;
        }
        // ---------- MOVIMIENTO CUANDO SE AGARRA CAJA ----------
        else
        {
            if (!IsBoxStillInFront())
            {
                grabbedBox = null;
                grabbingBox = false;
                return;
            }

            if ((input.x != 0 ^ input.y != 0) && !moving && cooldownTimer <= 0f)
            {
                if (grabbedBox == null)
                {
                    grabbingBox = false;
                    return;
                }

                FaceDirection boxDirection = FaceDirection.NONE;
                if (input.x > 0) boxDirection = FaceDirection.RIGHT;
                else if (input.x < 0) boxDirection = FaceDirection.LEFT;
                else if (input.y > 0) boxDirection = FaceDirection.UP;
                else if (input.y < 0) boxDirection = FaceDirection.DOWN;

                BoxController box = grabbedBox.GetComponent<BoxController>();

                // PULL
                if (IsOppositeDirection(facing, boxDirection) && box != null)
                {
                    Vector2 evaluatePoint = (Vector2)transform.position + offsetMovementPoint + input;
                    if (!Physics2D.OverlapCircle(evaluatePoint, circleRadius, obstacles) && box.Move(input))
                    {
                        moving = true;
                        movementPoint += input;
                        cooldownTimer = moveCooldown;
                    }
                }
                // PUSH
                else if (facing == boxDirection && box != null)
                {
                    if (box.Move(input))
                    {
                        Vector2 evaluatePoint = (Vector2)transform.position + offsetMovementPoint + input;
                        if (!Physics2D.OverlapCircle(evaluatePoint, circleRadius, obstacles))
                        {
                            moving = true;
                            movementPoint += input;
                            cooldownTimer = moveCooldown;
                        }
                    }

                    grabbingBox = false;
                    grabbedBox = null;
                }
            }

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                grabbedBox = null;
                grabbingBox = false;
            }
        }
    }

    private void UpdateGraphic()
    {
        if (spriteRenderer == null) return;

        switch (facing)
        {
            case FaceDirection.DOWN: spriteRenderer.color = Color.red; break;
            case FaceDirection.UP: spriteRenderer.color = Color.blue; break;
            case FaceDirection.LEFT: spriteRenderer.color = Color.yellow; break;
            case FaceDirection.RIGHT: spriteRenderer.color = Color.green; break;
        }
    }

    private bool IsOppositeDirection(FaceDirection dir1, FaceDirection dir2)
    {
        return (dir1 == FaceDirection.LEFT && dir2 == FaceDirection.RIGHT) ||
               (dir1 == FaceDirection.RIGHT && dir2 == FaceDirection.LEFT) ||
               (dir1 == FaceDirection.UP && dir2 == FaceDirection.DOWN) ||
               (dir1 == FaceDirection.DOWN && dir2 == FaceDirection.UP);
    }

    private Vector2 GetFacingDirection()
    {
        switch (facing)
        {
            case FaceDirection.DOWN: return Vector2.down;
            case FaceDirection.UP: return Vector2.up;
            case FaceDirection.LEFT: return Vector2.left;
            case FaceDirection.RIGHT: return Vector2.right;
            default: return Vector2.zero;
        }
    }

    private bool IsBoxStillInFront()
    {
        if (grabbedBox == null) return false;

        Vector2 dir = GetFacingDirection();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, obstacles);
        return hit.collider != null && hit.collider.gameObject;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(movementPoint + offsetMovementPoint, circleRadius);
    }
}