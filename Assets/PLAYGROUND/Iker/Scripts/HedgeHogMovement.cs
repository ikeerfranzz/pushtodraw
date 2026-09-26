using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HedgeHogMovement : MonoBehaviour
{
    public Tilemap groundTilemap;

    public Transform target1;
    public Transform target2;
    public Transform target3;

    public float moveSpeed = 3f;
    public float waitSeconds = 0.3f;

    private Transform player;

    private Vector3Int lastPlayerCell;
    private Vector3Int currentCell;

    private List<Vector3Int> path = new();
    private int pathIndex = 0;

    public bool isMoving = false;
    private bool isDead = false;  //Para la parte de las cajas

    private Transform[] targets;
    private int currentTargetIndex = 0;

    public enum FacingDirection { Up, Down, Left, Right }
    public FacingDirection currentFacing;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;

        targets = new Transform[] { target1, target2, target3 };

        currentCell = groundTilemap.WorldToCell(transform.position);
        transform.position = groundTilemap.GetCellCenterWorld(currentCell);

        lastPlayerCell = groundTilemap.WorldToCell(player.position);

        CalculatePath();
    }

    void Update()
    {
        //Para detectar si ha sido golpeado por una caja
        if (isDead) return;

        CheckIfKilledByBox();

        if (isDead) return;

        if(groundTilemap == null) return;
        Vector3Int playerCell = groundTilemap.WorldToCell(player.position);


        // Solo actuar cuando el jugador termine de moverse
        if (playerCell != lastPlayerCell)
        {
            lastPlayerCell = playerCell;

            if (!isMoving)
            {
                CalculatePath();
                MoveOneTile();
            }
        }
    }

    public void MoveOneTile()
    {
        if (isMoving || pathIndex >= path.Count)
            return;

        StartCoroutine(WaitAndMove());
    }

    IEnumerator WaitAndMove()
    {
        isMoving = true;

        yield return new WaitForSeconds(waitSeconds);

        CalculatePath();

        if (path.Count == 0)
        {
            isMoving = false;
            yield break;
        }

        Vector3Int directionVector = path[0] - currentCell;

        if (directionVector == Vector3Int.up) currentFacing = FacingDirection.Up;
        else if (directionVector == Vector3Int.down) currentFacing = FacingDirection.Down;
        else if (directionVector == Vector3Int.left) currentFacing = FacingDirection.Left;
        else if (directionVector == Vector3Int.right) currentFacing = FacingDirection.Right;

        currentCell = path[0];

        Vector3 targetWorldPos =
            groundTilemap.GetCellCenterWorld(currentCell);

        while (transform.position != targetWorldPos)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetWorldPos,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        isMoving = false;

        if (currentCell ==
            groundTilemap.WorldToCell(targets[currentTargetIndex].position))
        {
            currentTargetIndex =
                (currentTargetIndex + 1) % targets.Length;
        }
    }

    void CalculatePath()
    {
        path.Clear();

        HashSet<Vector3Int> blockedCells = GetBlockedCells();

        int attempts = 0;

        while (attempts < targets.Length)
        {
            Vector3Int start = currentCell;
            Vector3Int target =
                groundTilemap.WorldToCell(
                    targets[currentTargetIndex].position
                );

            Queue<Vector3Int> queue = new();
            Dictionary<Vector3Int, Vector3Int> cameFrom = new();

            queue.Enqueue(start);
            cameFrom[start] = start;

            Vector3Int[] directions =
            {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right
            };

            while (queue.Count > 0)
            {
                Vector3Int current = queue.Dequeue();
                if (current == target) break;

                foreach (Vector3Int dir in directions)
                {
                    Vector3Int next = current + dir;

                    if (cameFrom.ContainsKey(next)) continue;
                    if (!groundTilemap.HasTile(next)) continue;
                    if (blockedCells.Contains(next)) continue;

                    queue.Enqueue(next);
                    cameFrom[next] = current;
                }
            }

            if (cameFrom.ContainsKey(target))
            {
                Vector3Int step = target;

                List<Vector3Int> newPath = new();

                while (step != start)
                {
                    newPath.Insert(0, step);
                    step = cameFrom[step];
                }

                path = newPath;
                return;
            }

            currentTargetIndex =
                (currentTargetIndex + 1) % targets.Length;

            attempts++;
        }
    }

    HashSet<Vector3Int> GetBlockedCells()
    {
        HashSet<Vector3Int> blocked = new();

        foreach (var obj in GameObject.FindGameObjectsWithTag("object"))
        {
            blocked.Add(
                groundTilemap.WorldToCell(obj.transform.position)
            );
        }

        foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            blocked.Add(
                groundTilemap.WorldToCell(enemy.transform.position)
            );
        }

        return blocked;
    }


    private void CheckIfKilledByBox()
    {
        if (groundTilemap == null) return;
        Vector3Int hedgehogCell = groundTilemap.WorldToCell(transform.position);

        foreach (GameObject box in GameObject.FindGameObjectsWithTag("object"))
        {
            Vector3Int boxCell = groundTilemap.WorldToCell(box.transform.position);

            if (boxCell == currentCell || boxCell == hedgehogCell)
            {
                Die();
                return;
            }
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        StopAllCoroutines();

        Debug.Log("Hedgehog killed by box");

        Destroy(gameObject);
    }
}