using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class audioTracks : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [SerializeField] AudioMixer Mixer;

    [SerializeField] Slider MasterVolumeSlider;
    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
            MasterVolumeChange();
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            MusicVolumeChange();
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            SFXVolumeChange();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void previewSFX()
    {
        SFXSource.Play();
    }
    public void MasterVolumeChange()
    {
        PlayerPrefs.SetFloat("MasterVolume", MasterVolumeSlider.value);
        if (MasterVolumeSlider.value == 0)
            Mixer.SetFloat("MasterVolume", -80.0f);
        else
        Mixer.SetFloat("MasterVolume", Mathf.Log10(MasterVolumeSlider.value) * 20);
    }
    public void MusicVolumeChange()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolumeSlider.value);
        if (MusicVolumeSlider.value == 0)
            Mixer.SetFloat("MusicVolume", -80.0f);
        else
        Mixer.SetFloat("MusicVolume", Mathf.Log10(MusicVolumeSlider.value) * 20);
    }
    public void SFXVolumeChange()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);
        if (SFXVolumeSlider.value == 0)
            Mixer.SetFloat("SFXVolume", -80.0f);
        else
        Mixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolumeSlider.value) * 20);
    }
}
