using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DialogVolumeScript : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    void Start() {
        slider.value = PlayerPrefs.GetFloat("DialogVolume", 1f);
    }
    public void SetLevel() {
        float sliderValue = slider.value;
        mixer.SetFloat("dialogVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("DialogVolume", sliderValue);
    }
}