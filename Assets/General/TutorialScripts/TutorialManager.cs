using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;

    [SerializeField] private Player_Inputsys player_input;
    private Transform player;

    public enum TutorialState
    {
        Intro,
        MoveExplain,
        MovePractice,
        AbilityExplain,
        AbilityPractice,
        Finished
    }

    public TutorialState currentState;

    private Vector3 startPosition;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player_input = FindFirstObjectByType<Player_Inputsys>();
        player = GameObject.FindGameObjectWithTag("player").transform;

        StartTutorial();
    }

    void Update()
    {
        HandleInput();
        HandlePracticeCheck();
    }

    public void StartTutorial()
    {
        ChangeState(TutorialState.MoveExplain);
    }

    public void ChangeState(TutorialState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case TutorialState.MoveExplain:

                DialogueUI.instance.ShowDialogue("Te mueves con WASD\nPulsa SPACE para continuar");

                player_input.PlayerControls.Disable();

                break;

            case TutorialState.MovePractice:

                DialogueUI.instance.HideDialogue();

                player_input.PlayerControls.Enable();

                // SOLO movimiento
                player_input.PlayerControls.Player.Jump.Disable();

                startPosition = player.position;

                break;

            case TutorialState.AbilityExplain:

                DialogueUI.instance.ShowDialogue("Pulsa SPACE para empujar cajas");

                player_input.PlayerControls.Disable();

                break;

            case TutorialState.AbilityPractice:

                DialogueUI.instance.HideDialogue();

                player_input.PlayerControls.Enable();

                break;

            case TutorialState.Finished:

                DialogueUI.instance.ShowDialogue("Tutorial completado");

                player_input.PlayerControls.Enable();

                break;
        }
    }

    // INPUT CONTROL
    void HandleInput()
    {
        // Avanzar diálogo con SPACE (usando tu sistema)
        if (currentState == TutorialState.MoveExplain &&
            player_input.PlayerControls.Player.Jump.triggered)
        {
            ChangeState(TutorialState.MovePractice);
        }

        if (currentState == TutorialState.AbilityExplain &&
            player_input.PlayerControls.Player.Jump.triggered)
        {
            ChangeState(TutorialState.AbilityPractice);
        }
    }

    // PRACTICE CHECK
    void HandlePracticeCheck()
    {
        // Detectar movimiento real
        if (currentState == TutorialState.MovePractice)
        {
            if (Vector3.Distance(startPosition, player.position) > 2f)
            {
                ChangeState(TutorialState.AbilityExplain);
            }
        }

        // Detectar uso de habilidad (SPACE)
        if (currentState == TutorialState.AbilityPractice &&
            player_input.PlayerControls.Player.Jump.triggered)
        {
            ChangeState(TutorialState.Finished);
        }
    }
}