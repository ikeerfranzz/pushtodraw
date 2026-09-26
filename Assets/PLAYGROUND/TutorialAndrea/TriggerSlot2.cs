using Unity.Mathematics;
using UnityEngine;

public class TriggerSlot2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("object"))
        {
            Debug.Log("DONE");
            collision.GetComponent<BoxAnimator>().PlayOnTargetEffect();
            GameTutorial.instance.finished = true;
        }
    }

}
