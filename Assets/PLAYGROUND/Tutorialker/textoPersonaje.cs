using TMPro;
using UnityEngine;
using System.Collections;

public class textoPersonaje : MonoBehaviour
{
    private int pasos = 0;
    private bool bloqueado = false;

    public GameObject boton;
    public GameObject space;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bloqueado) return;

        if (collision.CompareTag("noReturn"))
        {
            space.SetActive(false);
            bloqueado = true;
            return;
        }

        if (collision.CompareTag("checkcasiCaja"))
        {
            boton.SetActive(false);
            space.SetActive(true);
        }

        //if (collision.CompareTag("check"))
        //{
        //    boton.SetActive(false);
        //}

        //if (collision.CompareTag("checkInicial"))
        //{
        //    boton.SetActive(true);
        //    space.SetActive(false);
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("checkcasiCaja"))
        {
            boton.SetActive(true);
            space.SetActive(false);
        }

    }
}

