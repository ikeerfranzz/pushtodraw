using UnityEngine;

public class UISounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip click;
    public AudioClip hurt;
    public void playClickSound ()
    {
        Debug.Log("playingsound");
        audioSource.PlayOneShot(click);
    }

    public void playHurtSound()
    {
        audioSource.PlayOneShot(hurt, .2f);
    }
}
