using UnityEngine;

public class OcultarCursor : MonoBehaviour
{
    public static OcultarCursor instance;

    private void Start()
    {
        instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void showCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void hideCursor()
    {
        Debug.Log("hideCursor llamado");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Visible: " + Cursor.visible + " | LockState: " + Cursor.lockState);
    }

}
