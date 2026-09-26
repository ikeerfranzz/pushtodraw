using UnityEngine;

public class BoxController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f; // Speed of the box movement
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private Vector2 offsetMovementPoint;
    [SerializeField] private float circleRadius = 0.2f;

    private Vector2 movementPoint;
    private bool moving = false;

    private void Start()
    {
        movementPoint = transform.position; // Start in its own cell
    }

    private void Update()
    {
        //This is to make the box make the movement like the player because if we don't do that the box moves faster than the player, and we want to make that box mov in cells
        if (moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementPoint, movementSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, movementPoint) == 0)
            {
                moving = false;
            }
        }
    }

    //That part is called by the player
    public bool Move(Vector2 direction)
    {
        if (moving) return false; // Don't allow movement while already moving

        Vector2 evaluatePoint = (Vector2)transform.position + offsetMovementPoint + direction;

        if (Physics2D.OverlapCircle(evaluatePoint, circleRadius, obstacles))
            return false;

        movementPoint += direction;
        moving = true;
        return true;
    }
}
