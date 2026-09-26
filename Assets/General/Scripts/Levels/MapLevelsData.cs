using UnityEngine;

public class MapLevelsData : MonoBehaviour
{
    public static MapLevelsData instance;

    public bool tutorialCompleted = false;
    public bool[] completed = new bool[11];

    // estrellas nivel
    public int[] levelStars = new int[11];

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        completed[0] = true;
        completed[10] = false;
    }

    public void LevelFinished(int levelIndex, int starsEarned)
    {
        completed[levelIndex] = true;

        if (starsEarned > levelStars[levelIndex])
        {
            levelStars[levelIndex] = starsEarned;
        }
    }
}
