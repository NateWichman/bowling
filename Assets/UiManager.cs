using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI SubText;

    public void SetScoreText(int score)
    {
        ScoreText.SetText(score.ToString());

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
