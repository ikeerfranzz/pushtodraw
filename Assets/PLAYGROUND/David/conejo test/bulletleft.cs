using UnityEngine;

public class bulletleft : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce((transform.right) * speed);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "player")
        {
            GameManager.gameManager.damageLifes();
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "object")
        {
            Destroy(gameObject);
        }
    }
}
