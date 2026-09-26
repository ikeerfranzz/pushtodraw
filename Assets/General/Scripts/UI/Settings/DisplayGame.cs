using UnityEngine;
using TMPro;
public class DisplayGame : MonoBehaviour
{
    [SerializeField] TMP_Dropdown displayDropdown;
    [SerializeField] TMP_Dropdown resolutionDropdown;

    // OPTCIONES D DISPLAY: 0 - Fullscreen, 1 - Windowed, 2 - Borderless
    private void Start()
    {
        displayDropdown.value = PlayerPrefs.GetInt("DisplayMode", 0); 
        displayDropdown.onValueChanged.AddListener(setDisplayMode);

        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", 3); // 1920x1080 por defecto
        resolutionDropdown.onValueChanged.AddListener(setResolution);
    }

    public void setDisplayMode(int index) 
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
        PlayerPrefs.SetInt("DisplayMode", index);
    }

    public void setResolution(int index)
    {
        switch (index)
        {
            case 0: Screen.SetResolution(1280, 720, Screen.fullScreenMode); break;
            case 1: Screen.SetResolution(1366, 768, Screen.fullScreenMode); break;
            case 2: Screen.SetResolution(1920, 1080, Screen.fullScreenMode); break;
            case 3: Screen.SetResolution(2560, 1440, Screen.fullScreenMode); break;
            case 4: Screen.SetResolution(3840, 2160, Screen.fullScreenMode); break;
        }

        PlayerPrefs.SetInt("Resolution", index);
    }

    void OnDestroy()
    {
        displayDropdown.onValueChanged.RemoveListener(setDisplayMode);
        resolutionDropdown.onValueChanged.RemoveListener(setResolution);
    }


}
