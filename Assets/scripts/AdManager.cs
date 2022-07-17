using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    string adUnitId = "ca-app-pub-1233908035609897/5126970497";
    string testAdUnitId = "ca-app-pub-3940256099942544/1033173712";

    public static AdManager Instance;

    private InterstitialAd interstitial;
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
            adUnitId = "ca-app-pub-1233908035609897/5126970497";
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
            adUnitId = "ca-app-pub-1233908035609897/7429480373";
        Instance = this;
        MobileAds.Initialize(initStatus => { });
        RequestIntersitialAd();
    }

    public void RequestIntersitialAd()
    {
        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void ShowIntersitialAd()
    {
        this.interstitial.Show();

        this.RequestIntersitialAd();
    }

    public void OnDestroy()
    {
        // Avoids memory leaks
        this.interstitial.Destroy();
    }
}