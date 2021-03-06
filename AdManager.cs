﻿using System.Collections;
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
    private string adUnitID;
    string appId = "ca-app-pub-4711925247199151~1271893261";

    public void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void Start()
    {

        MobileAds.Initialize(appId);


        //testAd
        //adUnitID = "ca-app-pub-3940256099942544/5224354917";

        //realAd
        adUnitID = "ca-app-pub-4711925247199151/3139825936";


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


    public void Handleonadloaded(object sender, EventArgs args)
    {

    }
    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Try to Re-load
        LoadRewardBasedAd();
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
        LoadRewardBasedAd(); 
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

    public bool IsAdLoaded()
    {
        return rewardBasedVideo.IsLoaded();
    }

    
}
