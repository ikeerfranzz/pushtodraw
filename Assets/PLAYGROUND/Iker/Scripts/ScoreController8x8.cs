using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [Header("configuración")]
    public int perfectMoves = 10;
    float totalMovements;

    [Header("referencias")]
    public Transform player;
    public Image[] stars;

    private Vector3 lastPosition;
    private int currentMoves = 0;
    private int currentStars = 3;

    public TextMeshProUGUI movesText;
    float movesRemaining;
    int newStars;

    // visual "pop"
    [SerializeField] GameObject movementsAnimations;
    private MovementsVisual movementsVisual;

    private void Start()
    {
        // get the script of the animations
        movementsVisual = movementsAnimations.GetComponent<MovementsVisual>();

        if (player == null)
        {
            Debug.LogError("player no asignado en scorecontroller");
            return;
        }

        if (stars == null || stars.Length == 0)
        {
            Debug.LogError("estrellas no asignadas");
            return;
        }

        lastPosition = player.position;

        UpdateStarsVisual(3);

        Debug.Log("perfectMoves: " + perfectMoves);

        newStars = CalculateStars();
        totalMovements = perfectMoves * 1.70f;
        movesRemaining = totalMovements - currentMoves;
        movesText.text = movesRemaining.ToString("00");
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, lastPosition);

        if (distance > 0.8f)
        {
            currentMoves++;


            lastPosition = player.position;
                
            newStars = CalculateStars();
            movesRemaining = (perfectMoves * 1.70f) - currentMoves;
            if (movesRemaining <= 0)
            {
                movesRemaining = 0;
                GameManager.gameManager.stars = 0;
                UIManager.UIManagerScript.showWinMenu();
            }
            movementsVisual.movementsPop(movesRemaining, totalMovements);
            movesText.text = movesRemaining.ToString("00");


            if (newStars != currentStars)
            {
                currentStars = newStars;
                UpdateStarsVisual(currentStars);
            }
        }
    }

    private int CalculateStars()
    {
        if (perfectMoves <= 0)
        {
            Debug.LogError("perfectMoves no puede ser 0");
            return 3;
        }

        int max3Stars = Mathf.CeilToInt(perfectMoves * 1.10f);
        int max2Stars = Mathf.CeilToInt(perfectMoves * 1.50f);
        int max1Star  = Mathf.CeilToInt(perfectMoves * 1.70f);

        if (currentMoves <= max3Stars) return 3;
        else if (currentMoves <= max2Stars) return 2;
        else if (currentMoves <= max1Star) return 1;
        else return 0;
    }

    private void UpdateStarsVisual(int starsCount)
    {
        Debug.Log("updating stars");
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = i < starsCount;
        }

        GameManager.gameManager.stars = starsCount;
    }
}