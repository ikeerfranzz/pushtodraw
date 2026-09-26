using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameTutorial : MonoBehaviour
{
    public static GameTutorial instance;

    public bool firstSlotDone = false;
    public bool finished = false;

    public GameObject arrows;
    public GameObject space;
    public GameObject player;

    public GameObject UIWon;
    public Animator animator;

    private void Awake()
    {
        instance = this;
        UIWon.SetActive(false);
        animator = player.GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(waitArrows());
    }
    private void Update()
    {
        if (finished)
        {
            OcultarCursor.instance.showCursor();
            StartCoroutine(waitWonUI());
            MapLevelsData.instance.tutorialCompleted = true;
        }
    }

    IEnumerator waitWonUI()
    {
        yield return new WaitForSecondsRealtime(.6f);
        Time.timeScale = 0f;
        UIWon.SetActive(true);
        UIWon.GetComponent<Animator>().Play("TutorialDone_Anim");
    }

    public void nextLevel()
    {
        StartCoroutine(playNextLevel());
    }

    IEnumerator playNextLevel()
    {
        Transitions.instance.FadeOut();
        yield return new WaitForSecondsRealtime(0.65f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void close()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator waitArrows()
    {
        yield return new WaitForSecondsRealtime(5f);
        arrows.SetActive(true);
        animator.Play("Tutorial_Flechas");
    }
}


