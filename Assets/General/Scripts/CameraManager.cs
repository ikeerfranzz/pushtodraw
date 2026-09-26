using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Script for managing the camera and re-sizing it depending on the size of the map in each level

    public static CameraManager instance;

    private void Awake()
    {
        if (instance != null)// just in case, in the case there's already a camera in the scene
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    [Tooltip("Resizes the camera taking into account the size of map of each level")]
    public void RecalculateCameraSize()
    {
        Camera camera = GetComponent<Camera>(); // we get the camera
        camera.orthographicSize = (GameManager.gameManager.currentMapSize / 2) + 2; // we calculate the size and resizes it
        Debug.Log("Camera Size changed to: " + camera.orthographicSize);
    }
}
