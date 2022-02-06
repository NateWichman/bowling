using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI SubText;

    public TextMeshProUGUI FrameText;

    public GridLayoutGroup Grid;

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

    public void DisplayFrames(List<Frame> frames)
    {
        foreach (Transform child in Grid.gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        var totalsText = "";
        var width = Grid.GetComponent<RectTransform>().rect.width;
        var gridSize = width / 10;
        Grid.cellSize = new Vector2(gridSize, gridSize);


        var scores = new List<string>();

        for (var i = 0; i < frames.Count; i++)
        {
            var frame = frames.ElementAt(i);
            var text = "";

            for (var j = 0; j < frame.shots.Count; j++)
            {
                var shot = frame.shots[j];

                if (j == 0 && shot == 10)
                {
                    text += "X ";
                }
                else if (j == 1 | j == 2)
                {
                    var prevShot = frame.shots[0];
                    if ((prevShot + shot) == 10)
                    {
                        text += "/ ";
                    }
                    else if (shot == 10)
                    {
                        text += "X ";
                    }
                    else
                    {
                        text += $"{shot} ";
                    }
                }
                else
                {
                    text += $"{shot} ";
                }
            }

            AddFrameText(text);
            scores.Add($"{frame.totalScore}");
        }

        for (var i = 0; i < 10 - frames.Count; i++)
        {
            AddFrameText("");
        }
        for (var i = 0; i < scores.Count; i++)
        {
            AddFrameText(scores.ElementAt(i));
        }


    }

    private void AddFrameText(string text)
    {
        var textObj = Instantiate(FrameText);
        textObj.SetText(text);
        textObj.gameObject.transform.parent = Grid.gameObject.transform;
    }
}
