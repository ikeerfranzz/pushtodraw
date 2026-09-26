using UnityEngine;
using UnityEngine.Events;

public class TriggerSlot1 : MonoBehaviour
{
    bool entered = false;
    public GameObject slot1;
    public GameObject slot2;


    private void Update()
    {
        if (entered) GameTutorial.instance.arrows.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        slot1.SetActive(false);
        GameTutorial.instance.animator.Play("Tutorial_Space");
        GameTutorial.instance.arrows.SetActive(false);
        GameTutorial.instance.space.SetActive(true);
        entered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        entered = false;
        if (!GameTutorial.instance.firstSlotDone)
        {
            slot1.SetActive(true);
            GameTutorial.instance.animator.Play("Tutorial_Flechas");
            GameTutorial.instance.arrows.SetActive(true);
            GameTutorial.instance.space.SetActive(false);
        }
        else
        {
            slot2.SetActive(true);
            GameTutorial.instance.space.SetActive(false);
        }
    }


}
