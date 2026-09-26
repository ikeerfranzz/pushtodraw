using UnityEngine;

public class GreenMinimap : MonoBehaviour
{
    public GameObject cruz;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("object"))
        {
            if(other.GetComponent<BoxManager>().boxType == BoxType.Green)
            {
                cruz.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("object"))
        {
            if (other.GetComponent<BoxManager>().boxType == BoxType.Green)
            {
                cruz.SetActive(true);
            }
        }
    }
}