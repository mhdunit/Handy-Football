using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;
using I2.Loc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoogleAds : MonoBehaviour
{

    public Animator RewardedAnim;
    public GameObject rewardedIcon, rewardedText;


    RewardedAdGameObject RewardedAd;

    InterstitialAdGameObject interstitialAd;


    // Use this for initialization


    void Start()
    {
        // Initialize the Mobile Ads SDK.
        MobileAds.Initialize((initStatus) =>
        {
            // SDK initialization is complete
        });

        LoadingAdsUI();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowRewardedAd()
    {
        RewardedAd = MobileAds.Instance.GetAd<RewardedAdGameObject>("Rewarded Video Ads");

        // Display an interstitial ad
        RewardedAd.ShowIfLoaded();
    }

    public void ShowInterstitialAd()
    {
        interstitialAd = MobileAds.Instance.GetAd<InterstitialAdGameObject>("Interstitial Ads");

        // Display an interstitial ad
        interstitialAd.ShowIfLoaded();
    }
    public void RewardedAdsloaded()
    {
        if (SceneManager.GetActiveScene().name == "Menu-c#")
        {
            rewardedIcon.GetComponent<BoxCollider>().enabled = true;
            rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
            rewardedText.GetComponent<TextMesh>().color = new Color(0, 1, 0, 1);
            rewardedText.GetComponent<TextMesh>().text = LocalizationManager.GetTranslation("UI.MainMenu.AdsAvalable");
        }
    }

    public void RewardedAdsFailedToload()
    {
        if (SceneManager.GetActiveScene().name == "Menu-c#")
        {
            rewardedIcon.GetComponent<BoxCollider>().enabled = false;
            rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            rewardedText.GetComponent<TextMesh>().color = new Color(1, 0, 0, 0.5f);
            rewardedText.GetComponent<TextMesh>().text = LocalizationManager.GetTranslation("UI.MainMenu.AdsNotAvalable");
        }
    }

    public void UserEarnRewarded()
    {
        if (SceneManager.GetActiveScene().name == "Menu-c#")
        {
            RewardedAnim.SetTrigger("Set Reward Trigger");
            rewardedIcon.GetComponent<BoxCollider>().enabled = false;
            rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            rewardedText.GetComponent<TextMesh>().color = new Color(1, 1, 1, 0.5f);
        }
    }
    public void ChangeUITextAdsLocalization()
    {
        if (SceneManager.GetActiveScene().name == "Menu-c#")
        {
            if (rewardedText.GetComponent<TextMesh>().text == LocalizationManager.GetTranslation("UI.MainMenu.AdsNotAvalable"))
                rewardedText.GetComponent<TextMesh>().text = LocalizationManager.GetTranslation("UI.MainMenu.AdsNotAvalable");
            else if (rewardedText.GetComponent<TextMesh>().text == LocalizationManager.GetTranslation("UI.MainMenu.LoadingAds"))
                rewardedText.GetComponent<TextMesh>().text = LocalizationManager.GetTranslation("UI.MainMenu.LoadingAds");
            else
                rewardedText.GetComponent<TextMesh>().text = LocalizationManager.GetTranslation("UI.MainMenu.AdsNotAvalable");
        }
    }
    public void LoadingAdsUI()
    {
        if (SceneManager.GetActiveScene().name == "Menu-c#")
        {
            rewardedIcon.GetComponent<BoxCollider>().enabled = false;
            rewardedIcon.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
            rewardedText.GetComponent<TextMesh>().color = new Color(1, 1, 1, 0.5f);
            rewardedText.GetComponent<TextMesh>().text = LocalizationManager.GetTranslation("UI.MainMenu.LoadingAds");
        }
    }
}
