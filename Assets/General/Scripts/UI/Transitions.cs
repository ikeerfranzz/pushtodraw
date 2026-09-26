using System.Collections;
using UnityEngine;

public class Transitions : MonoBehaviour
{
    public static Transitions instance;
    Animator animator;
    GameObject canvasParent;

    // fades duran .5f segundos!!
    [SerializeField] bool fadeIn;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        if (fadeIn) Fade();
        else animator.Play("Fade_None");
    }

    private void Start()
    {
        if(fadeIn) FadeIn();
    }
    public void FadeIn()
    {
        StartCoroutine(WaitBeforeFade());
    }

    public void FadeOut()
    {
        animator.Play("FadeOut");
    }

    public void Fade()
    {
        animator.Play("Fade");
    }

    IEnumerator WaitBeforeFade()
    {
        yield return new WaitForSeconds(0.1f);
        animator.Play("FadeIn");
    }
}
