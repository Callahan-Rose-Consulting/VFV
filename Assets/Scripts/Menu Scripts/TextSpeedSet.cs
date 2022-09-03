using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextSpeedSet : MonoBehaviour
{
    List<string> Speeds = new List<string>() { "Slow", "Medium", "Fast" };
    List<float> SpeedsNum = new List<float>() { 0.08f, 0.06f, 0.04f };
    int SpeedIndex = 1;
    public TextMeshProUGUI SpeedText;
    public Button IncreaseButton;
    public Button DecreaseButton;

    void Start()
    {
        SpeedIndex = PlayerPrefs.GetInt("TextSpeedIndex");
        SpeedText.text = Speeds[SpeedIndex];
    }

    public void IncreaseTextSpeed()
    {
        if ((SpeedIndex + 1) <= 2)
        {
            SpeedIndex++;
            SpeedText.text = Speeds[SpeedIndex];
        }
        PlayerPrefs.SetFloat("TextSpeed", SpeedsNum[SpeedIndex]);
        PlayerPrefs.SetInt("TextSpeedIndex", SpeedIndex);
        PlayerPrefs.Save();
    }

    public void DecreaseTextSpeed()
    {
        if ((SpeedIndex - 1) >= 0)
        {
            SpeedIndex--;
            SpeedText.text = Speeds[SpeedIndex];
        }
        PlayerPrefs.SetFloat("TextSpeed", SpeedsNum[SpeedIndex]);
        PlayerPrefs.SetInt("TextSpeedIndex", SpeedIndex);
        PlayerPrefs.Save();
    }
}
