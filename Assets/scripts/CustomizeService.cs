using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum KeyEnum
{
    Bronze = 1,
    Silver = 2,
    Gold = 3,
    Moon = 4,
    Earth = 5,
    Ghost = 6
}

public class CustomizeService : MonoBehaviour
{
    public static CustomizeService Instance;

    public Dictionary<KeyEnum, bool> Unlocks;

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

        Unlocks = new Dictionary<KeyEnum, bool>();
        Unlocks.Add(KeyEnum.Bronze, score >= 100);
        Unlocks.Add(KeyEnum.Silver, score >= 150);
        Unlocks.Add(KeyEnum.Gold, score >= 200);
        Unlocks.Add(KeyEnum.Moon, numGamesPlayed >= 10);
        Unlocks.Add(KeyEnum.Earth, numGamesPlayed >= 100);
        Unlocks.Add(KeyEnum.Ghost, isGhost == 1);
    }
}
