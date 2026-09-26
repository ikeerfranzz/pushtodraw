using UnityEngine;
using UnityEngine.UI;

public class AddHoverSoundToButtons : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverSound;
    [Range(0f, 1f)] public float volume = 1f;

    void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            ButtonHoverSound hover = button.gameObject.GetComponent<ButtonHoverSound>();

            if (hover == null)
            {
                hover = button.gameObject.AddComponent<ButtonHoverSound>();
            }

            hover.audioSource = audioSource;
            hover.hoverSound = hoverSound;
            hover.volume = volume;
        }
    }
}