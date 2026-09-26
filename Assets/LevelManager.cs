using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int index;

    private void Start()
    {
        OcultarCursor.instance.showCursor();
    }

    public void setIndex(int _index)
    {
        index = _index;
    }
    public void Play()
    {
        StartCoroutine(PlayStart());
    }

    IEnumerator PlayStart()
    {
        yield return new WaitForSecondsRealtime(0.45f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index);
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


}
