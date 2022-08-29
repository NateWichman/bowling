using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    string adUnitId = "ca-app-pub-1233908035609897/5126970497";
    string rewardAdUnitId = "ca-app-pub-1233908035609897/6109641724";

    public static AdManager Instance;

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
  
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android) {
            adUnitId = "ca-app-pub-1233908035609897/5126970497";
            rewardAdUnitId = "ca-app-pub-1233908035609897/6109641724";
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer) {
            adUnitId = "ca-app-pub-1233908035609897/7429480373";
            rewardAdUnitId = "ca-app-pub-1233908035609897/8488393051";
        }

        Instance = this;
        MobileAds.Initialize(initStatus => { });
        RequestIntersitialAd();
        RequestRewardAd();
    }

    public void RequestIntersitialAd()
    {
        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void RequestRewardAd() {
        rewardedAd = new RewardedAd(rewardAdUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }

    public void OnRewardReceived(object sender, EventArgs args)
    {
        Debug.Log("Reward Received");
    }

    public void ShowIntersitialAd()
    {
        this.interstitial.Show();

        this.RequestIntersitialAd();
    }

    public void ShowRewardedAd()
    {
        rewardedAd.OnUserEarnedReward += OnRewardReceived;
        this.rewardedAd.Show();
    }

    public void OnDestroy()
    {
        // Avoids memory leaks
        this.interstitial.Destroy();
        this.rewardedAd.Destroy();
    }
}