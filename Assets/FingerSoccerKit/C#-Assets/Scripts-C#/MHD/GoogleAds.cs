using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
using GoogleMobileAds.Placement;
using I2.Loc;

public class GoogleAds : MonoBehaviour {

    public Animator RewardedAnim;
    public GameObject rewardedIcon, rewardedText;


    RewardedAdGameObject RewardedAd;


    // Use this for initialization


    void Start() {
        // Initialize the Mobile Ads SDK.
        MobileAds.Initialize((initStatus) =>
        {
            // SDK initialization is complete
        });

    }

    // Update is called once per frame
    void Update() {

    }
    public void ShowRewardedAd()
    {
        RewardedAd = MobileAds.Instance.GetAd<RewardedAdGameObject>("Rewarded Video Ads");

        // Display an interstitial ad
        RewardedAd.ShowIfLoaded();


    }


    public void RewardedAdsloaded()
    {
        rewardedIcon.GetComponent<BoxCollider>().enabled = true;
        rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
        rewardedText.GetComponent<TextMesh>().color = new Color(0, 1, 0, 1);
        rewardedText.GetComponent<TextMesh>().text = LocalizationManager.GetTranslation("UI.MainMenu.AdsAvalable");
    }

    public void RewardedAdsFailedToload()
    {
        rewardedIcon.GetComponent<BoxCollider>().enabled = false;
        rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
        rewardedText.GetComponent<TextMesh>().color = new Color(1, 0, 0, 0.5f);
        rewardedText.GetComponent<TextMesh>().text = LocalizationManager.GetTranslation("UI.MainMenu.AdsNotAvalable");
    }

    public void UserEarnRewarded()
    {
            RewardedAnim.SetTrigger("Set Reward Trigger");
            rewardedIcon.GetComponent<BoxCollider>().enabled = false;
            rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            rewardedText.GetComponent<TextMesh>().color = new Color(1, 1, 1, 0.5f);
    }
}
