using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{

    public GameObject[] stars;
    public GameObject[] buttons;
    public Sprite[] winImages;
    public Image textImage;
    public GameObject winMenu;
    public static WinMenu winMenuScript;

    public AudioClip winSound;
    public AudioSource AudioSource;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("He pulsado I");
            showWinMenu();
        }
    }

    private void Awake()
    {
        winMenuScript = this;
    }

    /*funcion para enseñar el menu de win/lose */
    public void showWinMenu()
    {
        OcultarCursor.instance.showCursor();
        Debug.Log("Entro en showWinMenu");
        int playerStars = GameManager.gameManager.stars;

        AudioSource.PlayOneShot(winSound);

        winMenu.SetActive(true); // activamos el menu 

        buttons[1].SetActive(true);
        buttons[2].SetActive(true);

        if (playerStars == 0) {
            buttons[0].SetActive(false);
        }
        else
        {
            buttons[0].SetActive(true);
        }

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(false);
        }
        Sprite image;
        switch (playerStars)
        {
            case 0:
                image = winImages[0];
                break;
            case 1:
                image = winImages[2];
                stars[0].SetActive (true);
                break;
            case 2:
                image = winImages[3];
                stars[0].SetActive(true);
                stars[1].SetActive(true);
                break;
            case 3:
                image = winImages[4];
                stars[0].SetActive(true);
                stars[1].SetActive(true);
                stars[2].SetActive(true);
                break;
            default:
                image = winImages[1];
                stars[0].SetActive(true);
                stars[1].SetActive(true);
                stars[2].SetActive(true);
                break;
        }
        textImage.GetComponent<Image>().sprite = image;

        // hacemos que la imagen sea su size normal pero / 2 (la exporté en 2x para que se vea mejor, entonces hay que hacer la division)
        textImage.SetNativeSize();
        RectTransform rt = textImage.rectTransform;
        rt.sizeDelta = rt.sizeDelta * 0.5f;

    }

    public void restartLevel()
    {
        StartCoroutine(playRestartLevel());
    }

    public void nextLevel1()
    {
        StartCoroutine(playNextLevel());
    }

    IEnumerator playNextLevel()
    {
        yield return new WaitForSecondsRealtime(0.45f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator playRestartLevel()
    {
        yield return new WaitForSecondsRealtime(0.45f);
        Time.timeScale = 1f;
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }


}
