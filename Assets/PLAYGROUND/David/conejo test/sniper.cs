using UnityEngine;
using UnityEngine.Tilemaps;

public class sniper : MonoBehaviour
{
    [SerializeField] private LayerMask visionmask;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Tilemap groundTilemap;

    [Header("Direction")]
    public Direction shootDirection;

    public enum Direction
    {
        derecha,
        izquierda,
        abajo
    }

    [Header("Raycast Vision")]
    [Tooltip("Distancia de los rayos (hay 3)")]
    public float distance = 10f;

    [Tooltip("Distancia entre rayo y rayo")]
    public float height = 1f;

    private Transform player;
    private Vector3Int lastPlayerCell;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;
        lastPlayerCell = groundTilemap.WorldToCell(player.position);
    }

    void Update()
    {
        Vector3Int playerCell = groundTilemap.WorldToCell(player.position);

        if (playerCell != lastPlayerCell)
        {
            lastPlayerCell = playerCell;
            CheckShoot();
        }
    }

    void CheckShoot()
    {
        Vector2 dir = Vector2.zero;

        switch (shootDirection)
        {
            case Direction.derecha:
                dir = Vector2.right;
                break;

            case Direction.izquierda:
                dir = Vector2.left;
                break;

            case Direction.abajo:
                dir = Vector2.down;
                break;
        }

        float halfHeight = height * 0.5f;
        int rays = 5;
        bool seesPlayer = false;

        for (int i = 0; i < rays; i++)
        {
            float offset = Mathf.Lerp(-halfHeight, halfHeight, i / (float)(rays - 1));

            Vector2 perpendicular = new Vector2(-dir.y, dir.x);
            Vector2 origin = (Vector2)transform.position
             + (shootDirection == Direction.abajo ? dir * 0.5f : Vector2.zero)
             + perpendicular * offset;

            RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, visionmask);

            Debug.DrawRay(origin, dir * distance, Color.red, 0.1f);


            if (hit.collider != null && hit.collider.CompareTag("player"))
            {
                seesPlayer = true;
            }
        }

        if (seesPlayer)
            Shoot();
    }

    private void Shoot()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
    }
}