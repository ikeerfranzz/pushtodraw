using UnityEngine;

public class PausedMenuController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    /*reverse function of the showOptionsMenu*/
    public void hideOptionsMenu()
    {
        OcultarCursor.instance.hideCursor();
        UIManager.UIManagerScript.optionsOpen = false;
        animator.SetTrigger("Hide");
    }

    public void hide()
    {
        gameObject.SetActive(false);
        GameManager.gameManager.gameResumed(); // it calls the function on the game manager that pauses the game
    }
}
