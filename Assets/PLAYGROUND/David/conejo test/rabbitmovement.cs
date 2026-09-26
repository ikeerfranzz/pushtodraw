using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;


//ignorar este script /!\ aun tengo que revisarlo y adaptarlo a mi conejo, de momento es solo una copia cntr c cntrl v
// del iker probablemente desactualizado

public class rabbitmovement : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Transform target1;
    public Transform target2;
    public Transform target3;

    public float speed = 2f;

    List<Vector3Int> path = new();
    int pathIndex = 0;

    Transform[] targets;
    int currentTargetIndex = 0;

    void Start()
    {
        targets = new Transform[] { target1, target2, target3 };
        CalculatePath();
    }

    void Update()
    {
        if (path.Count == 0) return;

        Vector3 targetWorldPos =
            groundTilemap.GetCellCenterWorld(path[pathIndex]);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWorldPos,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            pathIndex++;

            if (pathIndex >= path.Count)
            {
                currentTargetIndex =
                    (currentTargetIndex + 1) % targets.Length;

                CalculatePath();
            }
        }
    }

    void CalculatePath()
    {
        path.Clear();
        pathIndex = 0;

        Vector3Int start =
            groundTilemap.WorldToCell(transform.position);

        Vector3Int target =
            groundTilemap.WorldToCell(
                targets[currentTargetIndex].position
            );

        HashSet<Vector3Int> blockedCells = GetBlockedCells();

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

            if (current == target)
                break;

            foreach (var dir in directions)
            {
                Vector3Int next = current + dir;

                if (cameFrom.ContainsKey(next)) continue;
                if (!groundTilemap.HasTile(next)) continue;
                if (blockedCells.Contains(next)) continue;

                queue.Enqueue(next);
                cameFrom[next] = current;
            }
        }

        if (!cameFrom.ContainsKey(target)) return;

        Vector3Int step = target;
        while (step != start)
        {
            path.Insert(0, step);
            step = cameFrom[step];
        }
    }

    HashSet<Vector3Int> GetBlockedCells()
    {
        HashSet<Vector3Int> blocked = new();
        foreach (var obj in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            blocked.Add(
                groundTilemap.WorldToCell(obj.transform.position)
            );
        }
        return blocked;
    }
}
