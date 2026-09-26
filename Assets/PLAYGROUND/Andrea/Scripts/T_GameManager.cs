using UnityEngine;
using UnityEngine.SceneManagement;

public class T_GameManager : MonoBehaviour
{
    public static T_GameManager tgameManager; // instance of the game manager 
    public int lifes; // public variable of the LIFES
    public int stars; // public variable of the STARS 
    [Tooltip("Variable that saves the size of the actual map (Placed in the inspector)")]
    public int currentMapSize;

    void Awake()
    {
        tgameManager = this;
        lifes = 3; // we set the default lifes at the begining as 3
        stars = 0; // we set the default stars at the begining as 0
        DontDestroyOnLoad(gameObject); // so it doesn't destroy itself when a new scene it's loaded
    }
    void Update()
    {
        //***************************************** DEBUG *****************************************
        if (Input.GetKeyDown(KeyCode.L)) // -1 lifes, KEY: L
        {
            damageLifes();
        }

        if (Input.GetKeyDown(KeyCode.E)) // +1 stars, KEY: E
        {
            addStar();
        }
    }

    /*function that adds 1 star*/
    public void addStar()
    {
        stars++;
    }

    /*function that reduces the heath 1 life*/
    public void damageLifes()
    {
        lifes--;
    }

    /*function that pauses the game*/
    public void gamePaused()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0;
    }

    /*function that resumes the game*/
    public void gameResumed()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1;
    }

}

