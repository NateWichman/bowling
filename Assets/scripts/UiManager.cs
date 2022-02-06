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

    public GameObject FrameText;

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

        var width = Grid.GetComponent<RectTransform>().rect.width;
        var gridSize = (width / 4);
        Grid.cellSize = new Vector2(gridSize, 30);


        var scores = new List<string>();
        var texts = new List<string>();

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

            texts.Add(text);
            scores.Add($"{frame.totalScore}");
        }


        // find the current frame,
        var currentFrame = frames.Count;
        if (currentFrame >= 3)
        {
            for (var i = 2; i >= 0; i--)
            {
                AddFrameText($"{frames.Count - i}");
            }
        }
        else
        {
            AddFrameText("1");
            AddFrameText("2");
            AddFrameText("3");
        }
        AddFrameText("");


        var last4Texts = new List<string>();
        for (var i = 0; i < 3; i++)
        {
            if (texts.Count >= 1)
            {
                last4Texts.Add(texts.ElementAt(texts.Count - 1));
                texts.RemoveAt(texts.Count - 1);
            }
        }
        last4Texts.Reverse();
        last4Texts.ForEach(x => AddFrameText(x));
        for (var i = 0; i < 3 - last4Texts.Count; i++)
        {
            AddFrameText("");
        }
        AddFrameText("Total");


        last4Texts = new List<string>();
        for (var i = 0; i < 3; i++)
        {
            if (scores.Count >= 1)
            {
                last4Texts.Add(scores.ElementAt(scores.Count - 1));
                scores.RemoveAt(scores.Count - 1);
            }
        }
        last4Texts.Reverse();
        last4Texts.ForEach(x => AddFrameText(x));
        for (var i = 0; i < 3 - last4Texts.Count; i++)
        {
            AddFrameText("");
        }

        var total = frames.Sum(x => x.totalScore);

        AddFrameText($"{ total }");


    }

    private void AddFrameText(string text)
    {
        var frameObj = Instantiate(FrameText);
        if (text == "")
        {
            var img = frameObj.GetComponent<Image>();
            img.enabled = false;
        }
        var textObj = frameObj.GetComponentInChildren<TextMeshProUGUI>();
        textObj.SetText(text);
        frameObj.transform.parent = Grid.gameObject.transform;
    }
}
