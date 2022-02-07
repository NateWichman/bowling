using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Frame
{
    public List<int> shots;
    public int totalScore;
    public bool isStrike;
    public bool isSpare;
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

        if (current.shots.Count == 2 && (!current.isSpare || !current.isStrike)) return true;

        return false;
    }

    public void OnShot(int numPins)
    {
        if (IsGameOver()) throw new System.Exception("GAME OVER");

        // first shot of game
        if (_frames.Count == 0)
        {
            _frames.Add(new Frame
            {
                shots = new List<int> { numPins },
                totalScore = numPins,
                isStrike = numPins == 10,
                isSpare = false
            });
            return;
        }

        var currentFrame = _frames.Last();

        // First shot in frame
        if ((currentFrame.shots.Count == 2 || currentFrame.isStrike) && _frames.Count != 10)
        {
            _frames.Add(new Frame
            {
                shots = new List<int> { numPins },
                totalScore = numPins,
                isStrike = numPins == 10
            });

            // if previous frame was strike
            if (_frames.Count >= 2)
            {
                var prevFrame = _frames.ElementAt(_frames.Count - 2);
                if (prevFrame.isSpare || prevFrame.isStrike)
                {
                    prevFrame.totalScore += numPins;
                }

                if (prevFrame.isStrike && _frames.Count >= 3)
                {
                    var prevPrevFrame = _frames.ElementAt(_frames.Count - 3);
                    if (prevPrevFrame.isStrike)
                    {
                        prevPrevFrame.totalScore += numPins;
                    }
                }
            }
        }

        // Second shot in frame
        if (currentFrame.shots.Count == 1 && (currentFrame.isStrike == false || _frames.Count == 10))
        {
            currentFrame.shots.Add(numPins);
            currentFrame.totalScore += numPins;

            if (currentFrame.totalScore == 10)
            {
                currentFrame.isSpare = true;
            }

            // if previous frame was a strike, add this score.
            if (_frames.Count >= 2)
            {
                var prevFrame = _frames.ElementAt(_frames.Count - 2);
                if (prevFrame.isStrike)
                {
                    // got a strike
                    prevFrame.totalScore += numPins;
                }
            }
        }


        // final shot
        if (currentFrame.shots.Count == 2 && _frames.Count == 10)
        {
            if (currentFrame.isStrike || currentFrame.isSpare)
            {
                currentFrame.shots.Add(numPins);
            }
        }



    }
}
