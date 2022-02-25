using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Unlocks
{
    public bool bronze = false;
    public bool silver = false;
    public bool gold = false;
    public bool emerald = false;
    public bool diamond = false;
}

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;

    public Unlocks Unlocks = new Unlocks();

    public UnityEvent<bool> UpdateEvent;

    void Awake()
    {
        Instance = this;
        LoadUnlocks();
    }

    public void LoadUnlocks()
    {
        Unlocks.bronze = CheckUnlock("BRONZE");
        Unlocks.silver = CheckUnlock("SILVER");
        Unlocks.gold = CheckUnlock("GOLD");
        Unlocks.emerald = CheckUnlock("EMERALD");
        Unlocks.diamond = CheckUnlock("DIAMOND");
    }

    private bool CheckUnlock(string key)
    {
        Debug.Log("check key: " + key + " RESULT: " + PlayerPrefs.GetString(key) == key + " TEST: " + PlayerPrefs.GetString(key));
        return PlayerPrefs.GetString(key) == key;
    }

    private void Unlock(string key)
    {
        PlayerPrefs.SetString(key, key);
    }


    public void UpdateUnlocks(Score score)
    {
        var total = score.GetTotal();

        if (total >= 100 && !this.Unlocks.bronze)
        {
            Unlock("BRONZE");
            Unlocks.bronze = true;
        }
        if (total >= 150 && !this.Unlocks.silver)
        {
            Unlock("SILVER");
            Unlocks.silver = true;
        }
        if (total >= 200 && !this.Unlocks.gold)
        {
            Unlock("GOLD");
            Unlocks.gold = true;
        }
        if (total >= 250 && !this.Unlocks.emerald)
        {
            Unlock("EMERALD");
            Unlocks.emerald = true;
        }
        if (total >= 300 && !this.Unlocks.diamond)
        {
            Unlock("DIAMOND");
            Unlocks.diamond = true;
        }

        this.UpdateEvent.Invoke(true);
    }
}
