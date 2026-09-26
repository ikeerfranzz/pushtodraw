using UnityEngine;

public class SoundData : MonoBehaviour
{
    public static SoundData Instance;
    public float SFXVolume;

    private void Awake()
    {
        Instance = this;
    }

    public float getVolume()
    {
        return SFXVolume;
    }

}
