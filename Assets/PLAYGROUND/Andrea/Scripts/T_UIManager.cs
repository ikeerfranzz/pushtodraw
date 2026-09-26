using UnityEngine;
using UnityEngine.SceneManagement;

public class T_UIManager : MonoBehaviour
{
    public static T_UIManager UIManagerScript;
    //life images 
    [SerializeField] GameObject life1;
    [SerializeField] GameObject life2;
    [SerializeField] GameObject life3;

    //stars images 
    [SerializeField] GameObject star1;
    [SerializeField] GameObject star2;
    [SerializeField] GameObject star3;

    //menu 
    [SerializeField] GameObject menuOptions;
    [SerializeField] GameObject menuWin;

    public void Awake()
    {
        UIManagerScript = this;
    }

    // Update is called once per frame
    void Update()
    {
        updateLifes();
        updateStars();
    }

    /*functions that updates the lifes and stars according to the number of lifes in the gamemanager*/
    void updateLifes()
    {
        switch (T_GameManager.tgameManager.lifes) //LIFES CHECK
        {
            case 0: // no lifes, none enabled
                gameOver();
                break;
            case 1: // 1 lifes, second and third not enabled
                life1.SetActive(true);
                life2.SetActive(false);
                life3.SetActive(false);
                break;
            case 2: // 2 lifes, third not enabled
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(false);
                break;
            case 3: // 3 lifes, all images shown in the UI
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(true);
                break;
            default:
                Debug.Log("[ERROR] There's " + T_GameManager.tgameManager.lifes + " lifes."); // message if the lifes are outside the range of 0-3
                break;
        }
    }
    void updateStars()
    {
        switch (T_GameManager.tgameManager.stars) //LIFES CHECK
        {
            case 0: // no lifes, none enabled
                break;
            case 1: // 1 star,
                star1.SetActive(true);
                break;
            case 2: // 2 stars, third not enabled
                star1.SetActive(true);
                star2.SetActive(true);
                break;
            case 3: // 3 stars, all images shown in the UI
                star1.SetActive(true);
                star2.SetActive(true);
                star3.SetActive(true);
                break;
            default:
                Debug.Log("[ERROR] There's " + T_GameManager.tgameManager.stars + " stars."); // message if the lifes are outside the range of 0-3
                break;
        }
    }

    /*function that shows the options menu*/
    public void showOptionsMenu()
    {
        menuOptions.SetActive(true);
        T_GameManager.tgameManager.gamePaused(); // it calls the function on the game manager that pauses the game
    }

    /*reverse function of the showOptionsMenu*/
    public void hideOptionsMenu()
    {
        menuOptions.SetActive(false);
        T_GameManager.tgameManager.gameResumed();
    }

    public void showWinMenu()
    {
        //delay??
        menuWin.SetActive(true);
        T_GameManager.tgameManager.gamePaused(); // it calls the function on the game manager that pauses the game
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu_Test");
    }

    public void gameOver()
    {
        SceneManager.LoadScene("MainMenu_Test");
    }
}
