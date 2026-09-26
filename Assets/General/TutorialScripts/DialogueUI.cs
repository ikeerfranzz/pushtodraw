using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    public GameObject characterImage;

    private void Awake()
    {
        instance = this;
    }

    public void ShowDialogue(string message)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = message;

        if (characterImage != null)
            characterImage.SetActive(true);
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);

        if (characterImage != null)
            characterImage.SetActive(false);
    }
}