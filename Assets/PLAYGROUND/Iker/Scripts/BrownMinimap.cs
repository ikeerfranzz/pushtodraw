using UnityEngine;

public class BrownMinimap : MonoBehaviour
{
    public GameObject cruz;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("object"))
        {
            if(other.GetComponent<BoxManager>().boxType == BoxType.Brown)
            {
                cruz.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("object"))
        {
            if (other.GetComponent<BoxManager>().boxType == BoxType.Brown)
            {
                cruz.SetActive(true);
            }
        }
    }
}