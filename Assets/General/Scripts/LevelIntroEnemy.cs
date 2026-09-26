using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class LevelIntroEnemy : MonoBehaviour
{
    public static bool introIsActive = false;  //Para desactivar el menu de options

    [Header("Panel")]
    [SerializeField] private GameObject introPanel;

    [Header("Animación")]
    [SerializeField] private float delayBeforeShow = 2f;
    [SerializeField] private float popDuration = 0.5f;
    [SerializeField] private float popScale = 1.15f;


    [Header("Texto de la intro")]
    [SerializeField] private TMP_Text descriptionText;

    [TextArea(3, 8)]
    [SerializeField] private string levelDescription;

    [Header("Jugador")]
    [SerializeField] private Player_Inputsys player;

    private bool introActive = false;
    private bool panelShown = false;
    private bool isPopping = false;
    private bool isTyping = false;

    [SerializeField] private float charDelay = 0.08f;
    private float delayTimer = 0f;
    private float popTimer = 0f;

    private RectTransform panelRect;

    private void Awake()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<Player_Inputsys>();
        }
        panelRect = introPanel.GetComponent<RectTransform>();
    }

    private void Start()
    {
        introIsActive = true;

        if (introPanel != null)
        {
            introPanel.SetActive(false);
        }


        if (player != null)
        {
            player.DisablePlayerInput();
        }
        else
        {
            Debug.LogError("No se ha encontrado el Player_Inputsys.");
        }

    }

    private void Update()
    {
        if (!panelShown)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= delayBeforeShow)
            {
                ShowIntro();
            }
        }

        if (isPopping)
        {
            AnimatePop();
        }

        if (introActive && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            CloseIntro();
        }
    }

    private void ShowIntro()
    {
        panelShown = true;
        introActive = true;
        isPopping = true;
        popTimer = 0f;

        introPanel.SetActive(true);

        if (panelRect != null)
        {
            panelRect.localScale = Vector3.zero;
        }
    }

    private void AnimatePop()
    {
        popTimer += Time.deltaTime;

        float t = popTimer / popDuration;
        t = Mathf.Clamp01(t);

        float scale;
        //Con chat para lo de la animacion
        if (t < 0.7f)
        {
            scale = Mathf.Lerp(0f, popScale, t / 0.7f);
        }
        else
        {
            scale = Mathf.Lerp(popScale, 0.85f, (t - 0.7f) / 0.3f);
        }

        if (panelRect != null)
        {
            panelRect.localScale = new Vector3(scale, scale, scale);
        }

        if (t >= 1f)
        {
            if (panelRect != null)
            {
                panelRect.localScale = new Vector3(.85f, .85f, .85f);
            }

            isPopping = false;
            StartCoroutine(WriteTutorial());
        }
    }

    private void CloseIntro()
    {
        introPanel.SetActive(false);

        if (player != null)
        {
            player.EnablePlayerInput();
        }

        introActive = false;
        introIsActive = false;
    }

    private IEnumerator WriteTutorial()
    {
        isTyping = true;
        descriptionText.text = "";

        foreach (char letter in levelDescription)
        {
            descriptionText.text += letter;
            yield return new WaitForSeconds(charDelay);
        }

        isTyping = false;
    }
}