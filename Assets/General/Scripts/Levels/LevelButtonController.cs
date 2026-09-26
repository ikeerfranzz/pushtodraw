using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonController : MonoBehaviour
{
    [SerializeField] int index;
    public GameObject[] starsUI;
    [SerializeField] Sprite starEarned;
    [SerializeField] Sprite starEmpty;

    private void Start()
    {
        GameObject childText = transform.GetChild(0).gameObject;

        if (MapLevelsData.instance.completed[index] == true)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
            childText.GetComponent<TextMeshProUGUI>().color = new Color(55f / 255, 108f / 255, 133f / 255, 110f / 255);
        }
        updateStars();
    }

    void updateStars()
    {
        if (MapLevelsData.instance != null)
        {
            int realIndex = index + 1;
            int earnedStars = MapLevelsData.instance.levelStars[realIndex];
            if (MapLevelsData.instance.completed[realIndex] == false)
            {
                for (int i = 0; i < starsUI.Length; i++)
                {
                    starsUI[i].SetActive(false);
                }
                return; 
            }

            Debug.Log("Nivel: " + realIndex + "/" + index + " stars: " + earnedStars);
            for (int i = 0; i < starsUI.Length; i++)
            {
                starsUI[i].SetActive(true);
                if (i < earnedStars)
                {
                    starsUI[i].GetComponent<Image>().sprite = starEarned;
                }
                else // Si no la ha ganado, ponemos la vacía
                {
                    starsUI[i].GetComponent<Image>().sprite = starEmpty;
                }
            }
        }
    }
}
