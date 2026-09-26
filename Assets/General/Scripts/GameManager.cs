using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager; // instance of the game manager 
    public int lifes; // public variable of the LIFES
    public int stars; // public variable of the STARS 
    [Tooltip("Variable that saves the size of the actual map (Placed in the inspector)")]
    public int currentMapSize;

    public GameObject player;
    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = this;


        lifes = 3; // we set the default lifes at the begining as 3
        stars = 3; // we set the default stars at the begining as 3
        gameResumed();
    }

    private void Start()
    {
        //audioSource.volume = SoundData.Instance.SFXVolume;
    }
    /*function that adds 1 star*/
    public void addStar()
    {
        stars++;
    }

    /*function that reduces the heath 1 life*/
    public void damageLifes()
    {
        GetComponent<UISounds>().playHurtSound();
        UIManager.UIAnimationController.hurtLifesAnimation();
        player.GetComponent<Animator>().SetTrigger("Hurt");
        lifes--;
    }

    public void addLifes()
    {
        lifes++;
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


    public void close()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
