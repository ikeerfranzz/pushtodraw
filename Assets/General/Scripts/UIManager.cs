using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager UIManagerScript;
    public static UIAnimationController UIAnimationController;

    [SerializeField] public int index;

    //life images 
    [SerializeField] GameObject life1;
    [SerializeField] GameObject life2;
    [SerializeField] GameObject life3;

    // no lifes 
    [SerializeField] GameObject NOlife1;
    [SerializeField] GameObject NOlife2;
    [SerializeField] GameObject NOlife3;

    //stars images 
    [SerializeField] GameObject star1;
    [SerializeField] GameObject star2;
    [SerializeField] GameObject star3;

    //menu 
    [SerializeField] GameObject menuOptions;
    [SerializeField] GameObject menuWin;

    public InputSystem_Actions UIControls;
    public bool optionsOpen = false;

    public void Awake()
    {
        UIManagerScript = this;
        UIControls = new InputSystem_Actions();
        UIControls.Enable();

        UIAnimationController = GetComponent<UIAnimationController>();
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
        if(GameManager.gameManager.lifes <= 0)
        {
            GameManager.gameManager.stars = 0;
            NOlife1.SetActive(true);
            NOlife2.SetActive(true);
            NOlife3.SetActive(true);
            life1.SetActive(false);
            showWinMenu();
        }
        else { 
            switch (GameManager.gameManager.lifes) //LIFES CHECK
            {
                case 0: // no lifes, none enabled

                    break;
                case 1: // 1 lifes, second and third not enabled
                    life1.SetActive(true);
                    life2.SetActive(false);
                    life3.SetActive(false);
                    NOlife2.SetActive(true);
                    NOlife3.SetActive(true);
                    break;
                case 2: // 2 lifes, third not enabled
                    life1.SetActive(true);
                    life2.SetActive(true);
                    life3.SetActive(false);
                    NOlife3.SetActive(true);
                    break;
                case 3: // 3 lifes, all images shown in the UI
                    life1.SetActive(true);
                    life2.SetActive(true);
                    life3.SetActive(true);
                    break;
                default:
                    Debug.Log("[ERROR] There's " + GameManager.gameManager.lifes + " lifes."); // message if the lifes are outside the range of 0-3
                    break;
            }
        }

        //Para desactivar el menu in game cuando la explicacion de enemigos
        if (LevelIntroEnemy.introIsActive)
        {
            return;
        }

        if (UIControls.UI.MenuOptions.WasPressedThisFrame())
        {
            if (optionsOpen)
            {
                
                menuOptions.GetComponent<PausedMenuController>().hideOptionsMenu();
                
            }
            else
            {
                showOptionsMenu();
            }
        }

    }
    void updateStars()
    {
        switch (GameManager.gameManager.stars) //LIFES CHECK
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
                Debug.Log("[ERROR] There's " + GameManager.gameManager.stars + " stars."); // message if the lifes are outside the range of 0-3
                break;
        }
    }

    /*function that shows the options menu*/
    public void showOptionsMenu()
    {
        OcultarCursor.instance.showCursor();
        menuOptions.GetComponent<Animator>().ResetTrigger("Hide");
        menuOptions.SetActive(true);
        GameManager.gameManager.gamePaused(); // it calls the function on the game manager that pauses the game
        optionsOpen = true;
    }

    /*reverse function of the showOptionsMenu*/
    public void hideOptionsMenu()
    {
        menuOptions.SetActive(false);
        GameManager.gameManager.gameResumed();
    }

    public void showWinMenu()
    {
        StartCoroutine(showMenu());
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void gameOver()
    {
        SceneManager.LoadScene("LevelsMenu");
    }


    IEnumerator showMenu()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        WinMenu.winMenuScript.showWinMenu();
        GameManager.gameManager.gamePaused(); // it calls the function on the game manager that pauses the game
    }

}
