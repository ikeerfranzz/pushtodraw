using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BoxAnimator : MonoBehaviour
{
    public AudioClip sound;
    public AudioSource AudioSource;
    public void PlayOnTargetEffect()
    {
        Debug.Log("BoxAnimator");
        StartCoroutine(OnTarget());
    }

    IEnumerator OnTarget()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        AudioSource.PlayOneShot(sound);
        Vector3 startScale = transform.localScale;
        Vector3 effectScale = startScale * 0.8f;

        float duration = 0.15f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, effectScale, time / duration);
            yield return null;
        }
        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(effectScale, startScale, time / duration);
            yield return null;
        }
        transform.localScale = startScale;

    }
}
