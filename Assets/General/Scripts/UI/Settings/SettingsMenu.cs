using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] AudioSource backgroundMusic;

    [SerializeField] Slider sfxSlider;
    [SerializeField] AudioSource sfxMusic;

    [SerializeField] Button audioButton;
    [SerializeField] GameObject audioSettings;
    private void Start()
    {
        musicSlider.value = backgroundMusic.volume;
        sfxSlider.value = sfxMusic.volume;

        musicSlider.onValueChanged.AddListener(setMusicVolume);
        sfxSlider.onValueChanged.AddListener(setSFXVolume);

        enableSettings();
    }

    public void setMusicVolume(float value)
    {
        backgroundMusic.volume = value;
    }

    public void setSFXVolume(float value)
    {
        sfxMusic.volume = value;
        SoundData.Instance.SFXVolume = value;
    }

    public void enableSettings()
    {
        EventSystem.current.SetSelectedGameObject(audioButton.gameObject);
        audioButton.Select();
        audioSettings.SetActive(true);
    }
}
