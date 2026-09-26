using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    private void Update()
    {
        //***************************************** DEBUG *****************************************
        if (Input.GetKeyDown(KeyCode.L)) // -1 lifes, KEY: L
        {
            GameManager.gameManager.damageLifes();
        }

        if (Input.GetKeyDown(KeyCode.K)) // -1 lifes, KEY: L
        {
            GameManager.gameManager.addLifes();
        }

        if (Input.GetKeyDown(KeyCode.E)) // +1 stars, KEY: E
        {
            GameManager.gameManager.addStar();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // NEXT LEVEL, KEY: 2
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // NEXT LEVEL, KEY: 1
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }


        if (Input.GetKeyDown(KeyCode.Q)) // NEXT LEVEL, KEY: 2
        {
            SceneManager.LoadScene("LevelsMenu");
        }
    }


}
