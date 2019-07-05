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
    public GameObject rewardedIcon, rewardedText;

    void Awake()
    {

        if (!_instance)
            _instance = this;
        if (Application.loadedLevelName == "Menu-c#")
        {
            rewardedIcon.GetComponent<BoxCollider>().enabled = false;
            rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            rewardedText.GetComponent<TextMesh>().color = new Color(1, 1, 0, 0.5f);
            rewardedText.GetComponent<TextMesh>().text = "در حال بارگذاری ...".faConvert();
        }
       
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
        if (Application.loadedLevelName == "Menu-c#")
        {
            rewardedIcon.GetComponent<BoxCollider>().enabled = true;
            rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
            rewardedText.GetComponent<TextMesh>().color = new Color(0, 1, 0, 1);
            rewardedText.GetComponent<TextMesh>().text = "تبلیغ ببین پول بگیر".faConvert();
        }
        ad = result; // store this to show the ad later
    },

    (string zoneId) => {
        // onNoAdAvailable
        Debug.Log("No Ad Available");
        if (Application.loadedLevelName == "Menu-c#")
        {
            rewardedIcon.GetComponent<BoxCollider>().enabled = false;
            rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            rewardedText.GetComponent<TextMesh>().color = new Color(1, 0, 0, 0.5f);
            rewardedText.GetComponent<TextMesh>().text = "نبلیغ موجود نیست".faConvert();
        }
    },

    (TapsellError error) => {
        // onError
        Debug.Log(error.error);
        RequestTapSellVideo();
        Debug.Log("No Ad Available");
        if (Application.loadedLevelName == "Menu-c#")
        {
            rewardedIcon.GetComponent<BoxCollider>().enabled = false;
            rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            rewardedText.GetComponent<TextMesh>().color = new Color(1, 0, 0, 0.5f);
            rewardedText.GetComponent<TextMesh>().text = "تبلیغ در دسترس نیست".faConvert();
        }
    },

    (string zoneId) => {
        // onNoNetwork
        Debug.Log("No Network");
        rewardedIcon.GetComponent<BoxCollider>().enabled = false;
        rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
        rewardedText.GetComponent<TextMesh>().color = new Color(1, 1, 0, 0.5f);
        rewardedText.GetComponent<TextMesh>().text = "شبکه را بررسی کنید".faConvert();
    },

    (TapsellAd result) => {
        // onExpiring
        
        Debug.Log("Expiring");
        // this ad is expired, you must download a new ad for this zone
        RequestTapSellVideo();
        Debug.Log("No Ad Available");
        if (Application.loadedLevelName == "Menu-c#")
        {
            rewardedIcon.GetComponent<BoxCollider>().enabled = false;
            rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            rewardedText.GetComponent<TextMesh>().color = new Color(1, 1, 0, 0.5f);
            rewardedText.GetComponent<TextMesh>().text = "منقضی شد".faConvert();
        }
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

                        rewardedIcon.GetComponent<BoxCollider>().enabled = false;
                        rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
                        rewardedText.GetComponent<TextMesh>().color = new Color(1, 1, 1, 0.5f);
                } 
            }
            // you may give rewards to user if result.completed and
            // result.rewarded are both true
        }
);
    }
    

}
