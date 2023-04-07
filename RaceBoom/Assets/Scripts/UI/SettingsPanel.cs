using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{

    [Header("---Audio")]
    [SerializeField] private AudioMixer mixer = null;
    [SerializeField] private Slider sfxSlider = null;
    private string _sfxVolumeParameter = "SFXVolume";
    [SerializeField] private Slider musicSlider = null;
    private string _musicVolumeParameter = "MusicVolume";
    // Start is called before the first frame update
    void Start()
    {
        //set listeners
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeSliderChange);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeSliderChange);

        //---load prefs
        LoadPrefs();
    }

    public void LoadPrefs()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(_sfxVolumeParameter, 1);
        OnSFXVolumeSliderChange(sfxSlider.value);
        musicSlider.value = PlayerPrefs.GetFloat(_musicVolumeParameter, 1);
        OnMusicVolumeSliderChange(musicSlider.value);
    }

    private void OnDisable()
    {
        //settingsCog.OnClickEvent -= SettingsCog_OnClickEvent;
        //qualityGroup.OnSelectedOption -= QualityGroup_OnSelectedOption;
        //frameGroup.OnSelectedOption -= FrameGroup_OnSelectedOption;
        //showFramesToggle.OnToggleClicked -= ShowFramesToggle_OnToggleClicked;

        //---save prefs
        PlayerPrefs.SetFloat(_sfxVolumeParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(_musicVolumeParameter, musicSlider.value);
    }

    //settings
    private void OnSFXVolumeSliderChange(float _v)
    {
        SetMixerVolumeNormalized(_v, _sfxVolumeParameter);

        //display
        sfxSlider.SetValueWithoutNotify(_v);
    }
    private void OnMusicVolumeSliderChange(float _v)
    {
        SetMixerVolumeNormalized(_v, _musicVolumeParameter);

        //display
        musicSlider.SetValueWithoutNotify(_v);
    }
    private void SetMixerVolumeNormalized(float _v, string _mixerName)
    {
        //Debug.Log(_v);
        mixer.SetFloat(_mixerName, Mathf.Log10(_v) * 20f);
        if (_v == 0)
        {
            mixer.SetFloat(_mixerName, -40f);
        }
    }


}
