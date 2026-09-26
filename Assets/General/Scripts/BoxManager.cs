using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoxManager : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f; // Speed of the box movement
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private Vector2 offsetMovementPoint;
    [SerializeField] private float circleRadius = 0.2f;

    private Vector2 movementPoint;
    private Vector2 lastValidPosition = Vector2.zero;
    private bool moving = false;

    bool correctlyPlaced = false;
    public BoxType boxType;
    private Vector3Int lastPositionCell;

    public AudioClip moveSound;
    public AudioSource AudioSource;

    private void Start()
    {
        Tilemap tileMap = TargetGridManager.targetGridManager.targetTileMap; //tilemap where the target tile are
        SnapToGrid(tileMap); // to snap the boxes in the center of the cell where they are (so they are perfectly centered and avoid colisions or other errors)
        movementPoint = transform.position; // Start in its own cell
        lastValidPosition = transform.position;
    }

    private void Update()
    {
        //******************************************* BOX MOVEMENT *******************************************
        //This is to make the box make the movement like the player because if we don't do that the box moves faster than the player, and we want to make that box mov in cells
        if (moving)
        {
            Vector2 position = Vector2.MoveTowards(transform.position, movementPoint, movementSpeed * Time.deltaTime);
            
            if (Vector2.Distance(position, movementPoint) <= 0.01)
            {
                moving = false;
            }
            transform.position = position;
        }

        //******************************************* BOX/TILE POSITION *******************************************
        bool isNowCorrect = TargetGridManager.targetGridManager.isTargetCorrect(transform.position, boxType); //we get if the box is now correct placed, in case the player moves the box
        Vector3Int currentCell = TargetGridManager.targetGridManager.targetTileMap.WorldToCell(transform.position); 

        if (!isNowCorrect && correctlyPlaced)  //if now is not correct but it was before, means the player moved it to another tile (not the target)
        {
            // it moves from a correct cell to a "emtpy" one
            correctlyPlaced = false;
            TargetGridManager.targetGridManager.boxRemoved(lastPositionCell);
        }
        else if (isNowCorrect && !correctlyPlaced) // if its now correct placed but it wasn't before it means the player moved it where it belongs
        {
            // it moves into a correct cell FROM an empty one
            correctlyPlaced = true;
            lastPositionCell = TargetGridManager.targetGridManager.targetTileMap.WorldToCell(transform.position);
            TargetGridManager.targetGridManager.boxCorrectlyPlaced(lastPositionCell, gameObject);
        }
        else if (isNowCorrect && correctlyPlaced)
        {
            // it moves from a correct cell INTO another correct cell
            if (lastPositionCell != currentCell)
            {
                TargetGridManager.targetGridManager.boxRemoved(lastPositionCell);
                lastPositionCell = currentCell;
                TargetGridManager.targetGridManager.boxCorrectlyPlaced(lastPositionCell, gameObject);
            }
        }
    }

    //That part is called by the player
    public bool Move(Vector2 direction)
    {
        if (moving) return false;

        Vector2 evaluatePoint = (Vector2)transform.position + offsetMovementPoint + direction;

        Collider2D[] hits = Physics2D.OverlapCircleAll(evaluatePoint, circleRadius, obstacles);
        //lo de foreach me lo ha recomendado el chat
        //para poder matar al erizo porq no detectaba que habia delante exactamente simplemente que habia algo y ya
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            HedgeHogMovement hedgehog = hit.GetComponentInParent<HedgeHogMovement>();

            if (hedgehog != null)
            {
                hedgehog.Die();
                continue;
            }

            return false;
        }
        AudioSource.PlayOneShot(moveSound);
        movementPoint += direction;
        moving = true;
        lastValidPosition = transform.position;
        return true;
    }
    private void SnapToGrid(Tilemap tileMap)
    {
        Vector3Int cell = tileMap.WorldToCell(transform.position); // we get the position of the cell where the box is
        transform.position = tileMap.GetCellCenterWorld(cell); // we transform the box position to the center of that cell
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            Destroy(other.gameObject, 0.2f);
        }
    }

    public void teleportToLastPosition()
    {
        transform.position = lastValidPosition; //teletransporte a la última posición válida
        movementPoint = lastValidPosition;
        moving = false;
    }

}
