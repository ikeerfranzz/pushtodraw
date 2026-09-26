using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Script : MonoBehaviour
{
    public enum FaceDirection { DOWN, LEFT, RIGHT, UP, NONE };

    [SerializeField] private float movementSpeed; // The speed of the movement of the character.
    [SerializeField] private Vector2 movementPoint; // Where we want to move the character.
    [SerializeField] private Vector2 offsetMovementPoint; // To adjust to a different position if we need.
    [SerializeField] private LayerMask obstacles; // It will tell us what the obstacles are so we don't go over them.
    [SerializeField] private float circleRadius; // To evaluate if we have obstacles where we want to move.
    [SerializeField] private float moveCooldown = 1f; // Time of whait between movements

    private float cooldownTimer = 0f;
    private SpriteRenderer spriteRenderer;

    private bool moving = false; // It tells if we are moving or not
    private bool grabbingBox = false; // Tells if we are grabbing a box
    private Vector2 input; // To separate the entrance of the controls
    public FaceDirection facing = FaceDirection.DOWN;

    private GameObject grabbedBox = null; // Box that we are grabbing

    public Vector2 lastDirection { get; private set; }

    [SerializeField] private AudioClip pushSound;
    private AudioSource pushBoxSound;

    // input actions
    //InputSystem_Actions PlayerControls;

    private void Start()
    {
        //input system
        //PlayerControls.Enable();

        spriteRenderer = GetComponent<SpriteRenderer>();
        movementPoint = transform.position; // Initiate it on the position of the player
    }

    private void Update()
    {
        //para el nuevo input system
        //if (PlayerControls.Player.Move.WasPressedThisFrame())
        //{
        //    input.x = Input.GetAxisRaw("Horizontal");
        //}
       
        
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        //PLAYER MOVEMENT
        if (moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementPoint, movementSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, movementPoint) == 0)
            {
                moving = false; // If we arrive where we want to move, the movement stops
            }
        }

        //THE PART WHEN THE PLAYER IS NOT GRABBING A BOX
        if (!grabbingBox)
        {
            // Normal player movement
            if ((input.x != 0 ^ input.y != 0) && !moving && cooldownTimer <= 0f) // ^ This is to make sure that the player can't move diagonally
            {
                lastDirection = input; // Save the last valid direction

                if (input.x > 0) facing = FaceDirection.RIGHT;
                else if (input.x < 0) facing = FaceDirection.LEFT;
                else if (input.y > 0) facing = FaceDirection.UP;
                else if (input.y < 0) facing = FaceDirection.DOWN;

                UpdateGraphic();

                Vector2 evaluatePoint = (Vector2)transform.position + offsetMovementPoint + input;
                if (!Physics2D.OverlapCircle(evaluatePoint, circleRadius))
                {
                    moving = true;
                    movementPoint += input; // Start the movement
                    cooldownTimer = moveCooldown;
                }
            }

            // Try to grab a box
            if (Input.GetKey(KeyCode.LeftShift) && !moving)
            {
                grabbedBox = null; // Reset grabbed box before raycast

                // Raycast direction depending on facing
                Vector2 direction = GetFacingDirection();

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, obstacles);

                if (hit.collider != null && hit.collider.CompareTag("object"))
                {
                    grabbedBox = hit.collider.gameObject;
                    grabbingBox = true; // We are now grabbing a box
                }
            }
            else
            {
                grabbedBox = null;
            }
        }
        // The part when the player is grabbing a box
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
                if (grabbedBox == null) // Safety check to avoid NullReference
                {
                    grabbingBox = false;
                    return;
                }

                // Direction the player is trying to move
                FaceDirection boxDirection = FaceDirection.NONE;
                if (input.x > 0) boxDirection = FaceDirection.RIGHT;
                else if (input.x < 0) boxDirection = FaceDirection.LEFT;
                else if (input.y > 0) boxDirection = FaceDirection.UP;
                else if (input.y < 0) boxDirection = FaceDirection.DOWN;

                // Pull Action
                if (IsOppositeDirection(facing, boxDirection))
                {
                    Vector2 evaluatePoint = (Vector2)transform.position + offsetMovementPoint + input;

                    if (!Physics2D.OverlapCircle(evaluatePoint, circleRadius, obstacles))                    {
                        // Move the box towards the player
                        BoxManager box = grabbedBox.GetComponent<BoxManager>();
                        if (box != null && box.Move(input))
                        {
                            moving = true;
                            movementPoint += input;
                            cooldownTimer = moveCooldown;
                        }
                    }
                }
                // Push Action
                else if (facing == boxDirection)
                {
                    // IMPORTANT: move the box BEFORE setting grabbedBox to null
                    BoxManager box = grabbedBox.GetComponent<BoxManager>();
                    if (box != null && box.Move(input))
                    {
                        pushBoxSound = GetComponent<AudioSource>();
                        pushBoxSound.Play();
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

            // If we release shift, we stop grabbing the box
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
        //In that part when we have the sprites of the player we put it here instead of the colors, it is now like that to make us know that the facing part is working right
        switch (facing)
        {
            case FaceDirection.DOWN:
                spriteRenderer.color = Color.red;
                break;
            case FaceDirection.UP:
                spriteRenderer.color = Color.blue;
                break;
            case FaceDirection.LEFT:
                spriteRenderer.color = Color.yellow;
                break;
            case FaceDirection.RIGHT:
                spriteRenderer.color = Color.green;
                break;
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
            // Raycast direction depending on facing
            case FaceDirection.DOWN: return Vector2.down;
            case FaceDirection.UP: return Vector2.up;
            case FaceDirection.LEFT: return Vector2.left;
            case FaceDirection.RIGHT: return Vector2.right;
            default: return Vector2.zero;
        }
    }
    //To make sure that the box continue in front of the player because without that some times we can move the box in a incorrect position
    private bool IsBoxStillInFront()
    {
        if (grabbedBox == null) return false;

        Vector2 dir = GetFacingDirection();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, obstacles);
        return hit.collider != null && hit.collider.gameObject;
    }

    private void OnDrawGizmos() // To see the circle that we are creating
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(movementPoint + offsetMovementPoint, circleRadius);
    }
}