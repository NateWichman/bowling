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
    public TextMeshProUGUI EndGameTotal;
    public TextMeshProUGUI HighScoreNum;
    public TextMeshProUGUI HighScoreText;

    public GameObject SpinDirectionBtn;

    public GameObject FrameText;
    public GameObject FrameText2;

    public Image LeftImage;

    public GridLayoutGroup Grid;

    public Slider slider;
    public Slider secondarySlider;

    void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 100;
        secondarySlider.minValue = 0;
        secondarySlider.maxValue = 100;
        HighScoreText.enabled = false;
        InputService.Instance.InputEvent.AddListener(ToggleSpinDirection);
    }

    void Destroy()
    {
        InputService.Instance.InputEvent.RemoveListener(ToggleSpinDirection);
    }

    public void SetEndGameTotal(int score)
    {
        EndGameTotal.SetText(score.ToString());
    }

    public void HideHighscoreText()
    {
        HighScoreText.enabled = false;
    }

    public void OnHighScore()
    {
        HighScoreText.enabled = true;
    }

    public void SetHighScore(int score)
    {
        HighScoreNum.SetText(score.ToString());
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
        /*  i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
          while (i.color.a > 0.0f)
          {
              i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
              yield return null;
          }
   */
        SetSubText("");
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
    }



    public void DisplayFrames(List<Frame> frames)
    {
        foreach (Transform child in Grid.gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        /*  var width = Grid.GetComponent<RectTransform>().rect.width;
          var gridSize = (width / 4);
          Grid.cellSize = new Vector2(gridSize, 50); */


        var scores = new List<string>();
        var texts = new List<string>();


        for (var i = 0; i < frames.Count; i++)
        {
            var frame = frames.ElementAt(i);

            texts.Add(frame.shotText);
            scores.Add($"{frame.totalScore}");
        }


        // find the current frame,
        var currentFrame = frames.Count;
        if (currentFrame >= 3)
        {
            for (var i = 2; i >= 0; i--)
            {
                AddFrameText($"{frames.Count - i}", true);
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
        GameObject frameObj;

        if (noBackground || text == "")
        {
            frameObj = Instantiate(FrameText2);
        }
        else
        {
            frameObj = Instantiate(FrameText);
        }
        var textObj = frameObj.GetComponentInChildren<TextMeshProUGUI>();
        textObj.SetText(text);
        frameObj.transform.parent = Grid.gameObject.transform;
        frameObj.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
    }


    private void ToggleSpinDirection(InputEventStruct dir)
    {
        if (dir.Type != InputType.TOGGLE_SPIN || dir.IsDown)
        {
            return;
        }

        var rightImg = SpinDirectionBtn.GetComponent<Image>();

        rightImg.enabled = InputService.Instance.SpinDirection == Direction.RIGHT;
        LeftImage.enabled = InputService.Instance.SpinDirection == Direction.LEFT;

        SpinDirectionBtn.GetComponentInChildren<TextMeshProUGUI>().SetText(
            InputService.Instance.SpinDirection == Direction.RIGHT ? "Right" : "Left"
        );
    }
}
