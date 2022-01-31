using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI SubText;

    public Slider slider;
    public Slider secondarySlider;

    void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 100;
        secondarySlider.minValue = 0;
        secondarySlider.maxValue = 100;
    }

    public void SetScoreText(int score)
    {
        ScoreText.SetText(score.ToString());

    }

    public void SetSlider(int percent)
    {
        slider.value = GetSliderPercent(percent);
    }

    public void SetSecondarySlider(int percent)
    {
        secondarySlider.value = GetSliderPercent(percent);
    }

    private int GetSliderPercent(int percent)
    {
        if (percent > 100)
        {
            percent = 100;
        }
        else if (percent < 0)
        {
            percent = 0;
        }
        return percent;
    }

    public void SetSubText(string text)
    {
        SubText.SetText(text);
    }

    public void Reset()
    {
        ScoreText.SetText("");
        SubText.SetText("");
    }
}
