using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManagerScript : MonoBehaviour
{
    private string APP_ID = "ca-app-pub-2269501974622283~7704014791";

    private BannerView bannerAD;
    private InterstitialAd interstitialAd;
    private RewardBasedVideoAd rewardBasedVideoAd;

    public void Start()
    {
        //MobileAds.Initialize(APP_ID);

        //RequestBanner();
        RequestInterstitial();
        //RequestVideoAD();
    }

    void RequestBanner()
    {
#if UNITY_ANDROID
        string bannerID = "ca-app-pub-3940256099942544/6300978111";
#else
        string bannerID = "unexpected_platform";
#endif

        bannerAD = new BannerView(bannerID, AdSize.SmartBanner, AdPosition.Top);

        AdRequest adRequest = new AdRequest.Builder()
            //.AddTestDevice("33BE2250B43518CCDA7DE426D04EE231")
            .Build();

        bannerAD.LoadAd(adRequest);
    }

    void RequestInterstitial()
    {
#if UNITY_ANDROID
        string interstitialID = "ca-app-pub-3940256099942544/1033173712";
#else
        string interstitialID = "unexpected_platform";
#endif

        interstitialAd = new InterstitialAd(interstitialID);

        HandleinterstitialAdEvents(true);

        AdRequest adRequest = new AdRequest.Builder()
            //.AddTestDevice("33BE2250B43518CCDA7DE426D04EE231")
            .Build();

        interstitialAd.LoadAd(adRequest);
    }

    void RequestVideoAD()
    {
#if UNITY_ANDROID
        string videoID = "ca-app-pub-3940256099942544/5224354917";
#else
        string videoID = "unexpected_platform";
#endif
        rewardBasedVideoAd = RewardBasedVideoAd.Instance;

        AdRequest adRequest = new AdRequest.Builder()
            //.AddTestDevice("33BE2250B43518CCDA7DE426D04EE231")
            .Build();

        rewardBasedVideoAd.LoadAd(adRequest, videoID);
    }

    public void DisplayBanner()
    {
        bannerAD.Show();
    }

    public void DisplayInterstitialAD()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
    }

    public void DisplayRewardVideo()
    {
        if (rewardBasedVideoAd.IsLoaded())
        {
            rewardBasedVideoAd.Show();
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("CloseAd");

        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Destroy();
        }

        GetComponent<GameManager>().ReturnOurPersonToLife();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    void HandleBannerAdEvents(bool subscribe)
    {
        if (subscribe)
        {
            // Called when an ad request has successfully loaded.
            this.bannerAD.OnAdLoaded += this.HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.bannerAD.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            this.bannerAD.OnAdOpening += this.HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            this.bannerAD.OnAdClosed += this.HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.bannerAD.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;
        }
        else
        {
            // Called when an ad request has successfully loaded.
            this.bannerAD.OnAdLoaded -= this.HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.bannerAD.OnAdFailedToLoad -= this.HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            this.bannerAD.OnAdOpening -= this.HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            this.bannerAD.OnAdClosed -= this.HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.bannerAD.OnAdLeavingApplication -= this.HandleOnAdLeavingApplication;
        }
    }

    void HandleinterstitialAdEvents(bool subscribe)
    {
        if (subscribe)
        {
            this.interstitialAd.OnAdLoaded += HandleOnAdLoaded;
            this.interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            this.interstitialAd.OnAdOpening += HandleOnAdOpened;
            this.interstitialAd.OnAdClosed += HandleOnAdClosed;
            this.interstitialAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        }
        else
        {
            this.interstitialAd.OnAdLoaded -= HandleOnAdLoaded;
            this.interstitialAd.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
            this.interstitialAd.OnAdOpening -= HandleOnAdOpened;
            this.interstitialAd.OnAdClosed -= HandleOnAdClosed;
            this.interstitialAd.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
        }
    }

    private void OnEnable()
    {
        //HandleBannerAdEvents(true);
    }

    private void OnDisable()
    {
        //HandleBannerAdEvents(false);
        //HandleinterstitialAdEvents(false);
    }
}
