using TMPro;
using UnityEngine;
using System.Collections;

public class cajaTutorial : MonoBehaviour
{
    public GameObject space;
    public TMP_Text textoHold;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("checkCaja"))
        {
            space.SetActive(false);
            textoHold.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }
}