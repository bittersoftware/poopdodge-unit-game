using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { set; get; }
    public string rewardType;
    private RewardBasedVideoAd rewardBasedVideo;
    private string adUnitID = "ca-app-pub-3940256099942544/5224354917";

    public void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        rewardBasedVideo = RewardBasedVideoAd.Instance;

        rewardBasedVideo.OnAdLoaded += HandleOnAdLoaded;
        rewardBasedVideo.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        rewardBasedVideo.OnAdOpening += HandleOnAdOpening;
        rewardBasedVideo.OnAdStarted += HandleOnAdStarted;
        rewardBasedVideo.OnAdRewarded += HandleOnAdRewarded;
        rewardBasedVideo.OnAdClosed += HandleOnAdClosed;
        rewardBasedVideo.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        LoadRewardBasedAd();
    }

    private void LoadRewardBasedAd()
    {
//#if UNITY_EDITOR
//        string adUnitID = "usused";
//#elif UNITY_ANDROID
//        string adUnitID = "ca-app-pub-3940256099942544/5224354917";
//#else
//        string adUnitID = "unexpected_platform";
//#endif

        rewardBasedVideo.LoadAd(new AdRequest.Builder().Build(), adUnitID);
    }

    public void ShowRewardBasedAd()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
        else
        {
            Debug.Log("AD NOT LOADED!");
        }
    }

    // These are the ad callback events that can be hooked into.
    public event EventHandler<EventArgs> HandleOnAdLoaded;

    public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

    public event EventHandler<EventArgs> OnAdOpening;

    public event EventHandler<EventArgs> OnAdStarted;

    public event EventHandler<EventArgs> OnAdClosed;

    public event EventHandler<Reward> OnAdRewarded;

    public event EventHandler<EventArgs> OnAdLeavingApplication;

    public event EventHandler<EventArgs> OnAdCompleted;


    //public void HandleOnAdLoaded(object sender, EventArgs args)
    //{
    //}
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Try to Re-load
        rewardBasedVideo.LoadAd(new AdRequest.Builder().Build(), adUnitID);
    }
    public void HandleOnAdOpening(object sender, EventArgs args)
    {
    }
    public void HandleOnAdStarted(object sender, EventArgs args)
    {
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        //The OnAdClosed event is a handy place to load a new rewarded video ad after displaying the previous one
        rewardBasedVideo.LoadAd(new AdRequest.Builder().Build(), adUnitID);
    }
    public void HandleOnAdRewarded(object sender, Reward args)
    {
        //reward user
        string type = args.Type;
        double amount = args.Amount;
        print("User rewarded with: " + amount.ToString() + " " + type);

        if (rewardType == "bullet")
        {
            FindObjectOfType<GameManager>().setRewardBullet();
        }
        else if (rewardType == "time")
        {
            FindObjectOfType<GameManager>().setRewardTime();
        }
        else {
            Debug.Log("Unkwown RewardType");
        }

        rewardType = "default";
    }
    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
    }

    
}
