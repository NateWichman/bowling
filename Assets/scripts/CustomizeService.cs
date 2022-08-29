using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public enum KeyEnum
{
    Bronze = 1,
    Silver = 2,
    Gold = 3,
    Moon = 4,
    Earth = 5,
    Ghost = 6,
    Core = 7,
    Diamond = 8,
    PoolBall = 9,
    Emerald = 10,
    Jupiter = 11,
    FishBowl = 12,
    Clark = 13
}

public class CustomizeService : MonoBehaviour
{
    public static CustomizeService Instance;

    public Dictionary<KeyEnum, bool> Unlocks = null;

    private bool isInitialLoad = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitUnlocks();
    }

    public void InitUnlocks()
    {
        int score = PlayerPrefs.GetInt("HIGH_SCORE", 0);
        int numGamesPlayed = PlayerPrefs.GetInt("NUM_GAMES_PLAYED", 0);
        int isGhost = PlayerPrefs.GetInt("IS_GHOST", 0);
        int isPoolBall = PlayerPrefs.GetInt("IS_POOL_BALL", 0);
        int strikesInARow = PlayerPrefs.GetInt("STRIKES_ROW", 0);


        var newUnlocks = new Dictionary<KeyEnum, bool>();

        if (isInitialLoad)
        {
            isInitialLoad = false;
            Unlocks = newUnlocks;
        }
        newUnlocks.Add(KeyEnum.Bronze, score >= 100);
        newUnlocks.Add(KeyEnum.Silver, score >= 150);
        newUnlocks.Add(KeyEnum.Gold, score >= 200);
        newUnlocks.Add(KeyEnum.Jupiter, strikesInARow >= 2);
        newUnlocks.Add(KeyEnum.FishBowl, strikesInARow >= 3);
        newUnlocks.Add(KeyEnum.Moon, numGamesPlayed >= 10);
        newUnlocks.Add(KeyEnum.Earth, numGamesPlayed >= 100);
        newUnlocks.Add(KeyEnum.Ghost, isGhost == 1);
        newUnlocks.Add(KeyEnum.Core, numGamesPlayed >= 30);
        newUnlocks.Add(KeyEnum.Diamond, score >= 300);
        newUnlocks.Add(KeyEnum.PoolBall, isPoolBall == 1);
        newUnlocks.Add(KeyEnum.Emerald, score >= 250);
        newUnlocks.Add(KeyEnum.Clark, true);


        CheckIfAnyNewUnlocks(newUnlocks);
        Unlocks = newUnlocks;
    }


    private void CheckIfAnyNewUnlocks(Dictionary<KeyEnum, bool> newUnlocks)
    {
        var dict3 = Unlocks.Where(entry => newUnlocks[entry.Key] != entry.Value);

        foreach (var val in dict3)
        {
            UiManager.Instance.ShowUnlocked(Enum.GetName(typeof(KeyEnum), val.Key) + " UNLOCKED");
        }
    }
}
