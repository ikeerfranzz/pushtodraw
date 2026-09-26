using System.Collections;
using UnityEngine;

public class GameWon : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        OcultarCursor.instance.showCursor();
        animator = GetComponent<Animator>();
        animator.Play("Canvas_Idle");
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.3f);
        animator.Play("TutorialDone_Anim");
    }

}
