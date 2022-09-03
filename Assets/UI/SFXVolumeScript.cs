using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXVolumeScript : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    void Start() {
        slider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
    public void SetLevel() {
        float sliderValue = slider.value;
        mixer.SetFloat("sfxVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }
}
