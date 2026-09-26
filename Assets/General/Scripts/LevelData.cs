using UnityEngine;

public class LevelMapData : MonoBehaviour
{
    // Script for the information of each level 

    // ************************ MAP DATA (used for the camera size algorithm after)  ************************
    // Map Sizes: Small : 6x6, Medium: 8x8, Large: 12x12

    public enum MapSize { Small, Medium, Large }
    [Tooltip("Variable that saves the size of the actual map (Placed in the inspector)")]
    public MapSize currentMapSize;

    private void Start()
    {
        int currentMapSizeValue = GetMapSizeValue();
        GameManager.gameManager.currentMapSize = currentMapSizeValue;
        CameraManager.instance.RecalculateCameraSize();
    }

    /// <summary>
    /// Returns an INT for the current size of the map (6,8 or 12)
    /// </summary>
    public int GetMapSizeValue()
    {
        switch(currentMapSize) // depending on the size of the map returns it's size
        {
            case MapSize.Small: return 6;
            case MapSize.Medium: return 8;
            case MapSize.Large: return 12;
            default: return 12; // to avoid errors, we put 12 as the default one (better it to be bigger than smaller)
        }
    }
}
