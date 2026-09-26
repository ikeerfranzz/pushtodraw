using UnityEngine;

public class bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public bool vertical;
    private float spawnTime;
    Transform transform;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform = gameObject.transform;
        if (vertical)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            rb.AddForce(Vector2.down * speed);
        }
        else { 
            rb.AddForce(transform.right * speed);
        }
        spawnTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("enemy") && Time.time - spawnTime >= 0.2f )
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.CompareTag("object"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.name == "WallTileMap" && Time.time - spawnTime >= 0.2f)
        {
            Destroy(gameObject);
        }
    }
}