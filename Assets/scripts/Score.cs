using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Frame
{
    public List<int> shots;
    public string shotText;
    public int totalScore;
    public bool isStrike;
    public bool isSpare;

    public Frame(int numPins)
    {
        this.shots = new List<int>();
        this.shots.Add(numPins);
        this.totalScore = numPins;
        this.isStrike = numPins == 10;
        this.isSpare = false;

        shotText = this.isStrike ? "X  " : numPins.ToString() + "  ";
    }
}

public class Score
{
    private float score = 0;
    private List<Frame> _frames = new List<Frame>();

    public List<Frame> GetFrames()
    {
        return _frames;
    }

    public bool IsGameOver()
    {
        if (_frames.Count != 10) return false;

        var current = _frames.Last();

        if (current.shots.Count == 3) return true;

        if (current.shots.Count == 1) return false;

        if (current.shots.Count == 2)
        {
            return current.shots.Sum(x => x) < 10;
        }

        return false;
    }

    public void OnShot(int numPins)
    {
        if (IsGameOver()) throw new System.Exception("GAME OVER");

        Frame currentFrame = CurrentFrame(numPins);
        CalcPreviousStrikeSpare();
    }

    public int GetTotal()
    {
        return _frames.Sum(x => x.totalScore);
    }
    private void CalcPreviousStrikeSpare()
    {
        /* + Strikes get the score of the next two shots added to that frame's score.
           + Spares get the score of only the next shot added to that frame's score.
           + These are true, except in the tenth (last frame) where strikes / spares do 
           not double any scores. They are just face value */

        // There is not a previous shot to check for strikes / spares
        if (_frames.Count < 2) return;

        var prevFrame = _frames.ElementAt(_frames.Count - 2);
        var currentFrame = _frames.Last();

        // strike doubles next two shots. (gotta make sure its not the 3rd shot of the last frame, because that is more than 2 shots away from the last strike)
        if (prevFrame.isStrike && currentFrame.shots.Count <= 2)
        {
            prevFrame.totalScore += currentFrame.shots.Last(); ;

            // if its a strike, got to check the frame before as well
            if (_frames.Count < 3) return;

            var prevPrevFrame = _frames.ElementAt(_frames.Count - 3);

            if (prevPrevFrame.isStrike && currentFrame.shots.Count < 2)
            {
                prevPrevFrame.totalScore += currentFrame.shots.Last();
            }

            return;
        }
        else if (prevFrame.isSpare && currentFrame.shots.Count == 1)
        {
            // spares double only the next shot (not two).
            prevFrame.totalScore += currentFrame.shots.Last();
        }


    }

    private Frame CurrentFrame(int numPins)
    {
        // first shot of the game
        if (_frames.Count == 0)
        {
            return AddFrame(numPins);
        }

        var currentFrame = _frames.Last();

        // last frame acts differently
        if (_frames.Count != 10)
        {
            if (currentFrame.shots.Count == 2 || currentFrame.isStrike)
            {
                // frame is over.
                return AddFrame(numPins);
            }

            // frame is not over, add a shot
            currentFrame.shots.Add(numPins);
            currentFrame.totalScore += numPins;
            currentFrame.isSpare = currentFrame.shots.Sum(x => x) == 10;
            currentFrame.shotText += currentFrame.isSpare ? "/" : numPins.ToString();
            return currentFrame;
        }
        else
        {
            // is in last frame
            if (currentFrame.shots.Count >= 3) throw new System.Exception("Trying to add score to a completed game");

            // frame is not over, add a shot
            currentFrame.shots.Add(numPins);
            currentFrame.totalScore += numPins;

            if (numPins == 10)
            {
                currentFrame.shotText += "X";
            }
            else if (currentFrame.shots.Count == 2)
            {
                if (currentFrame.shots.ElementAt(1) != 10 && (numPins + currentFrame.shots.ElementAt(1)) == 10)
                {
                    currentFrame.shotText += "/  ";
                }
                else
                {
                    currentFrame.shotText += numPins.ToString() + "  ";
                }
            }
            else
            {
                currentFrame.shotText += numPins.ToString();
            }
            // tenth frame does not count spares/strikes
            return currentFrame;
        }
    }

    private Frame AddFrame(int numPins)
    {
        var frame = new Frame(numPins);
        _frames.Add(frame);
        return frame;
    }



    public static void RunTests()
    {
        var threeHundred = new List<int> { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
        Score.RunTest(threeHundred, 300);

        var TwoOTwo = new List<int> {
            10, 8, 2, 9, 1, 8, 0, 10, 10 ,9 ,1, 9, 1, 10, 10, 9, 1
        };
        Score.RunTest(TwoOTwo, 202);

        var OneSixtyFour = new List<int> {
            7, 3, 10, 10, 8, 1, 9, 1, 8, 1, 10, 9, 1, 8, 2, 6, 1
        };
        Score.RunTest(OneSixtyFour, 164);

        var TwoSeventySix = new List<int> {
            10, 10, 10, 10, 10, 10, 10, 10, 6, 4, 10, 10,10
        };
        Score.RunTest(TwoSeventySix, 276);
    }

    private static void RunTest(List<int> shots, int expectedScore)
    {
        var score = new Score();

        foreach (var shot in shots)
        {
            score.OnShot(shot);
        }

        if (score.GetTotal() == expectedScore)
        {
            Debug.Log($"Passed test expecting a score of: {expectedScore}");
        }
        else
        {
            Debug.Log($"FAILED test for: {expectedScore}. Got A score of: {score.GetTotal()}");
        }
    }
}

