using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapsellSDK;
using System;
using UnityEngine.UI;


public class TapSellUse : MonoBehaviour {
    TapsellAd ad;
    public string TapSellKey;
    public string VideoZoneID;
    //  public Menu2 m2;
    public Animator RewardedAnim;
    // Use this for initialization
    private static TapSellUse _instance;

    void Awake()
    {

        if (!_instance)
            _instance = this;
    }
    void OnDestroy()
    {
        Destroy(this.gameObject);
    }
    void Start () {
        Tapsell.initialize(TapSellKey);
        RequestTapSellVideo();
        print("RequestTapSellVideo");
    }

    // Update is called once per frame
    void Update () {


       
    }
    public void RequestTapSellVideo()
    {

        Tapsell.requestAd(VideoZoneID, false,
    (TapsellAd result) => {
        // onAdAvailable
        Debug.Log("Action: onAdAvailable");
        ad = result; // store this to show the ad later
    },

    (string zoneId) => {
        // onNoAdAvailable
        Debug.Log("No Ad Available");
    },

    (TapsellError error) => {
        // onError
        Debug.Log(error.error);
        RequestTapSellVideo();
    },

    (string zoneId) => {
        // onNoNetwork
        Debug.Log("No Network");
    },

    (TapsellAd result) => {
        // onExpiring
        
        Debug.Log("Expiring");
        // this ad is expired, you must download a new ad for this zone
        RequestTapSellVideo();
    }

);

        SetRewardVideo();
    }

    public void ShowTapSellVideo()
    {
        print("ShowTapSellVideo");
        TapsellShowOptions showOptions = new TapsellShowOptions();
        showOptions.backDisabled = false;
        showOptions.immersiveMode = false;
        showOptions.rotationMode = TapsellShowOptions.ROTATION_UNLOCKED;
        showOptions.showDialog = true;
        Tapsell.showAd(ad, showOptions);
        RequestTapSellVideo();
    }

    void SetRewardVideo()
    {
        Tapsell.setRewardListener((TapsellAdFinishedResult result) =>
        {
                if (result.completed == true)
                {
                if (Application.loadedLevelName == "Menu-c#")
                {
                    RewardedAnim.SetTrigger("Set Reward Trigger");
                    RequestTapSellVideo();
                } 
            }
            // you may give rewards to user if result.completed and
            // result.rewarded are both true
        }
);
    }
    

}
