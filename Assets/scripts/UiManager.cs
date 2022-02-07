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
        StartCoroutine(FadeTextToZeroAlpha(1, SubText));
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        yield return new WaitForSeconds(1);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }

        SetSubText("");
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
    }

    public void Reset()
    {
        ScoreText.SetText("");
    }

    public void DisplayFrames(List<Frame> frames)
    {
        foreach (Transform child in Grid.gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        /*    var width = Grid.GetComponent<RectTransform>().rect.width;
            var gridSize = (width / 4);
            Grid.cellSize = new Vector2(gridSize, 30); */


        var scores = new List<string>();
        var texts = new List<string>();


        for (var i = 0; i < frames.Count; i++)
        {
            var frame = frames.ElementAt(i);
            var text = "";

            var isLastFrame = frames.Count - 1 == i;

            for (var j = 0; j < frame.shots.Count; j++)
            {
                var shot = frame.shots[j];

                if (j == 0 && shot == 10)
                {
                    text += "X   ";
                }
                else if (j == 1 | j == 2)
                {
                    var prevShot = frame.shots[0];
                    if ((prevShot + shot) == 10)
                    {
                        text += "/   ";
                    }
                    else if (shot == 10)
                    {
                        text += "X   ";
                    }
                    else
                    {
                        text += $"{shot}   ";
                    }
                }
                else
                {
                    text += $"{shot}   ";
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
            AddFrameText("1", true);
            AddFrameText("2", true);
            AddFrameText("3", true);
        }
        AddFrameText("Total", true);


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
        var total = frames.Sum(x => x.totalScore);

        AddFrameText($"{ total }");


    }

    private void AddFrameText(string text, bool noBackground = false)
    {
        var frameObj = Instantiate(FrameText);
        if (text == "" || noBackground)
        {
            var img = frameObj.GetComponent<Image>();
            img.enabled = false;
        }
        var textObj = frameObj.GetComponentInChildren<TextMeshProUGUI>();
        textObj.SetText(text);
        frameObj.transform.parent = Grid.gameObject.transform;
    }
}
