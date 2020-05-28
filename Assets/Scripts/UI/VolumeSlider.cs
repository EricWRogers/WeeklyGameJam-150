using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum AudioType
    {
        Master,
        SFX,
        Music
    }

    public AudioType GameAudioType = AudioType.Master;
    public Slider slider;

    private void Awake()
    {
        slider = this.GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { SetVolume(); });
        GetVolume();
    }

    public void SetVolume()
    {
        SetVolume(slider.value);
    }

    private void GetVolume()
    {
        GameSettings gameSetting = GameManager.Instance.GetGameSettings();

        switch (GameAudioType)
        {
            case AudioType.Master:
                slider.value = gameSetting.masterVolume;
                break;
            case AudioType.Music:
                slider.value = gameSetting.musicVolume;
                break;
            case AudioType.SFX:
                slider.value = gameSetting.sfxVolume;
                break;
        }
    }

    private void SetVolume(float num)
    {
        GameSettings gameSetting = GameManager.Instance.GetGameSettings();

        switch(GameAudioType)
        {
            case AudioType.Master:
                gameSetting.masterVolume = num;
                break;
            case AudioType.Music:
                gameSetting.musicVolume = num;
                break;
            case AudioType.SFX:
                gameSetting.sfxVolume = num;
                break;
        }
        
        GameManager.Instance.SaveGameSettings(gameSetting);
    }

}
