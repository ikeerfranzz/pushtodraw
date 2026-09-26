using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    public void Play()
    {
        StartCoroutine(PlayStart());
    }

    IEnumerator PlayStart()
    {
        yield return new WaitForSecondsRealtime(0.45f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        StartCoroutine(PlayMainMenu());
    }

    IEnumerator PlayMainMenu()
    {
        yield return new WaitForSecondsRealtime(0.45f);
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        StartCoroutine(PlayExit());
    }

    IEnumerator PlayExit()
    {
        yield return new WaitForSecondsRealtime(0.45f);
        Application.Quit();
    }

    public void Restart()
    {
        StartCoroutine(PlayRestart());
    }

    IEnumerator PlayRestart()
    {
        yield return new WaitForSecondsRealtime(0.45f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //MAIN MENU
    public void PlayMenu()
    {
        StartCoroutine(PlayINMainMenu());
    }

    IEnumerator PlayINMainMenu()
    {
        yield return new WaitForSecondsRealtime(0.45f);
        if (!MapLevelsData.instance.tutorialCompleted)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene("LevelsMenu");
        }
            
    }
}
