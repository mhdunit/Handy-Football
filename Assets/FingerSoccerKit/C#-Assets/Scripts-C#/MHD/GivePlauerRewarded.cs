using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlauerRewarded : MonoBehaviour
{
    public AudioSource Menu;
    public AudioClip RewardedvideoSFX;
    public TextMesh TotalCoin;

    public void RewardedVideoFunction()
    {
        Menu.PlayOneShot(RewardedvideoSFX);
        PlayerPrefs.SetInt("PlayerMoney", PlayerPrefs.GetInt("PlayerMoney") + 5);
        TotalCoin.text = PlayerPrefs.GetInt("PlayerMoney").ToString().faConvert();
    }
}
