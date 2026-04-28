using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    string adUnitId = "ca-app-pub-1233908035609897/7214429270";
    string rewardAdUnitId = "ca-app-pub-1233908035609897/2174194890";

    public static AdManager Instance;

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            adUnitId = "ca-app-pub-1233908035609897/7214429270";
            rewardAdUnitId = "ca-app-pub-1233908035609897/2174194890";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            adUnitId = "ca-app-pub-1233908035609897/7429480373";
            rewardAdUnitId = "ca-app-pub-1233908035609897/8488393051";
        }

        Instance = this;
        MobileAds.Initialize(initStatus =>
        {
            RequestIntersitialAd();
            RequestRewardAd();
        });
    }

    public void RequestIntersitialAd()
    {
        // Destroy old ad if exists
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }

        AdRequest request = new AdRequest();

        InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load: " + error);
                return;
            }
            interstitial = ad;
        });
    }

    public void RequestRewardAd()
    {
        // Destroy old ad if exists
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        AdRequest request = new AdRequest();

        RewardedAd.Load(rewardAdUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error);
                return;
            }
            rewardedAd = ad;
        });
    }

    public void ShowIntersitialAd()
    {
        if (interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();
            RequestIntersitialAd(); // preload next one
        }
        else
        {
            Debug.LogWarning("Interstitial ad not ready.");
            RequestIntersitialAd();
        }
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                GameManager.Instance.ReceiveAdReward();
            });
            RequestRewardAd();
        }
        else
        {
            Debug.LogWarning("Rewarded ad not ready.");
            RequestRewardAd();
        }
    }

    public void OnDestroy()
    {
        if (interstitial != null) interstitial.Destroy();
        if (rewardedAd != null) rewardedAd.Destroy();
    }
}