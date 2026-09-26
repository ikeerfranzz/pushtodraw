using NUnit.Framework;
using UnityEngine;

public class BoxTutorial : MonoBehaviour
{
    public Sprite target;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("checkCaja"))
        {
            Debug.Log("CAJA COLOCADA PERFECTAMENTE");
            GameTutorial.instance.firstSlotDone = true;
            GameTutorial.instance.arrows.SetActive(false);
            GameTutorial.instance.space.SetActive(true);
        }
    }

    private void Update()
    {
        if(GameTutorial.instance.firstSlotDone)
        {
            GetComponent<SpriteRenderer>().sprite = target;
        }
    }
}
