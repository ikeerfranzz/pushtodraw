using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Inputsys : MonoBehaviour
{
    public enum FaceDirection { DOWN, LEFT, RIGHT, UP, NONE };

    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private Vector2 movementPoint;
    [SerializeField] private Vector2 offsetMovementPoint;
    [SerializeField] private LayerMask obstacles = 1 << 3;
    [SerializeField] private float circleRadius = 0.3f;
    [SerializeField] private float moveCooldown = 0.3f;

    private float cooldownTimer = 0f;
    private SpriteRenderer spriteRenderer;

    private bool inputLocked = false;
    private bool moving = false;
    private bool grabbingBox = false;

    private Vector2 input;
    private Vector3 lastValidPosition = Vector3.zero;

    public FaceDirection facing = FaceDirection.DOWN;

    private GameObject grabbedBox = null;

    public Vector2 lastDirection { get; private set; }

    [Header("Audio")]
    public AudioClip playerSteps;
    public AudioSource AudioSource;

    [Header("Footstep Randomization")]
    [Range(0f, 1f)][SerializeField] private float volumeModSize = 0.08f;
    [Range(0f, 3f)][SerializeField] private float pitchModSize = 0.2f;

    private float baseVolume;
    private float basePitch;

    public InputSystem_Actions PlayerControls;

    private void Awake()
    {
        PlayerControls = new InputSystem_Actions();
        PlayerControls.Enable();
    }

    private void OnEnable()
    {
        PlayerControls.Player.Enable();
        PlayerControls.UI.Enable();
        PlayerControls.CHEATS.Enable();
    }

    private void OnDestroy()
    {
        PlayerControls.Player.Disable();
        PlayerControls.UI.Disable();
        PlayerControls.CHEATS.Disable();
    }

    private void Start()
    {
        // Add components if not exist
        if (GetComponent<BoxCollider2D>() == null)
            gameObject.AddComponent<BoxCollider2D>();

        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }

        if (GetComponent<SpriteRenderer>() != null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        movementPoint = transform.position;
        lastValidPosition = transform.position;

        // Guardar valores originales del audio
        baseVolume = AudioSource.volume;
        basePitch = AudioSource.pitch;
    }

    private void Update()
    {
        if (inputLocked)
        {
            input = Vector2.zero;
            return;
        }

        input = PlayerControls.Player.Move.ReadValue<Vector2>();

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // PLAYER MOVEMENT
        if (moving)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                movementPoint,
                movementSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, movementPoint) == 0)
            {
                moving = false;
            }
        }

        // PLAYER NOT GRABBING BOX
        if (!grabbingBox)
        {
            if ((input.x != 0 ^ input.y != 0) && !moving && cooldownTimer <= 0f)
            {
                lastDirection = input;

                if (input.x > 0) facing = FaceDirection.RIGHT;
                else if (input.x < 0) facing = FaceDirection.LEFT;
                else if (input.y > 0) facing = FaceDirection.UP;
                else if (input.y < 0) facing = FaceDirection.DOWN;

                Vector2 evaluatePoint = (Vector2)transform.position + offsetMovementPoint + input;

                Collider2D hit = Physics2D.OverlapCircle(evaluatePoint, circleRadius, obstacles);

                if (!hit)
                {
                    lastValidPosition = transform.position;
                    moving = true;

                    // RANDOMIZAR AUDIO
                    AudioSource.volume = Modulation(baseVolume + 0.2f, volumeModSize);
                    AudioSource.pitch = Modulation(basePitch, pitchModSize);

                    AudioSource.PlayOneShot(playerSteps);

                    movementPoint += input;
                    cooldownTimer = moveCooldown;
                }
            }

            // TRY TO GRAB BOX
            if (PlayerControls.Player.Jump.IsPressed() && !moving)
            {
                grabbedBox = null;

                Vector2 direction = GetFacingDirection();

                RaycastHit2D hit = Physics2D.Raycast(
                    transform.position,
                    direction,
                    1.0f,
                    obstacles
                );

                if (hit.collider != null && hit.collider.CompareTag("object"))
                {
                    grabbedBox = hit.collider.gameObject;
                    grabbingBox = true;
                }
            }
            else
            {
                grabbedBox = null;
            }
        }
        // PLAYER GRABBING BOX
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

                // PULL
                if (IsOppositeDirection(facing, boxDirection))
                {
                    Vector2 evaluatePoint = (Vector2)transform.position + offsetMovementPoint + input;

                    if (!Physics2D.OverlapCircle(evaluatePoint, circleRadius, obstacles))
                    {
                        BoxManager box = grabbedBox.GetComponent<BoxManager>();

                        if (box != null && box.Move(input))
                        {
                            moving = true;
                            lastValidPosition = transform.position;

                            // RANDOMIZAR AUDIO
                            AudioSource.volume = Modulation(baseVolume + 0.2f, volumeModSize);
                            AudioSource.pitch = Modulation(basePitch, pitchModSize);

                            AudioSource.PlayOneShot(playerSteps);

                            movementPoint += input;
                            cooldownTimer = moveCooldown;
                        }
                    }
                }
                // PUSH
                else if (facing == boxDirection)
                {
                    BoxManager box = grabbedBox.GetComponent<BoxManager>();

                    if (box != null && box.Move(input))
                    {
                        Vector2 evaluatePoint = (Vector2)transform.position + offsetMovementPoint + input;

                        if (!Physics2D.OverlapCircle(evaluatePoint, circleRadius, obstacles))
                        {
                            moving = true;
                            lastValidPosition = transform.position;

                            // RANDOMIZAR AUDIO
                            AudioSource.volume = Modulation(baseVolume + 0.2f, volumeModSize);
                            AudioSource.pitch = Modulation(basePitch, pitchModSize);

                            AudioSource.PlayOneShot(playerSteps);

                            movementPoint += input;
                            cooldownTimer = moveCooldown;
                        }
                    }

                    grabbingBox = false;
                    grabbedBox = null;
                }
            }

            // RELEASE BOX
            if (PlayerControls.Player.Jump.WasReleasedThisFrame())
            {
                grabbedBox = null;
                grabbingBox = false;
            }
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

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir,
            1f,
            obstacles
        );

        return hit.collider != null && hit.collider.gameObject;
    }

    public void DisablePlayerInput()
    {
        inputLocked = true;

        input = Vector2.zero;
        moving = false;
        grabbingBox = false;
        grabbedBox = null;

        movementPoint = transform.position;

        if (PlayerControls != null)
        {
            PlayerControls.Player.Disable();
        }

        Debug.Log("PLAYER INPUT DISABLED");
    }

    public void EnablePlayerInput()
    {
        inputLocked = false;

        if (PlayerControls != null)
        {
            PlayerControls.Player.Enable();
        }

        Debug.Log("PLAYER INPUT ENABLED");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(movementPoint + offsetMovementPoint, circleRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy" ||
            collision.gameObject.tag == "damage" ||
            collision.gameObject.tag == "rabbit")
        {
            GameManager.gameManager.damageLifes();

            if (collision.gameObject.tag != "rabbit")
                teleportToLastPosition();
        }
    }

    public void teleportToLastPosition()
    {
        transform.position = lastValidPosition;
        movementPoint = lastValidPosition;

        if (grabbedBox != null)
        {
            grabbedBox.GetComponent<BoxManager>().teleportToLastPosition();
        }

        moving = false;
    }

    // RANDOM AUDIO FUNCTION
    private float Modulation(float startValue, float size)
    {
        float max = startValue + size / 2f;
        float min = startValue - size / 2f;

        return Random.Range(min, max);
    }
}