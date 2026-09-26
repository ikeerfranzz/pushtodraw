using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class T_LevelManager : MonoBehaviour
{
    public static T_LevelManager instance;
    public GameObject[] levelButtons;
    public int levelsUnblocked = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].GetComponent<Button>().interactable = false;
        }
    }

    public void reloadButtons()
    {
        for (int i = 0; i <= levelsUnblocked; i++)
        {
            levelButtons[i].GetComponent<Button>().interactable = true;
        }
    }
    public void loadLevel1()
    {
        SceneManager.LoadScene("Prototype");
    }
    public void loadLevel2()
    {
        SceneManager.LoadScene("Drawing_8x8");

    }
}
