using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splash : MonoBehaviour {
    
    // Use this for initialization
    public Text SplashText;
    public AudioClip BackGroundSFX;
    public RectTransform Back, Text;

    private void Start()
    {
        GetComponent<Animation>().Play("Splash");
        


    }
    private void Update()
    {
        
    }


    public void PlayBackGroundMusic()
    {
        GetComponent<AudioSource>().PlayOneShot(BackGroundSFX);
    }
   
    public void ScreenOrienationCheck()
    {
        if (Screen.width < Screen.height)
        {
            Back.transform.localPosition = new Vector3(0, 29, 0);
            Back.transform.localScale = new Vector3(0.28f, 0.28f, 0.28f);
            Text.transform.localPosition = new Vector3(0, -38, 0);
            Text.transform.localScale = new Vector3(0.24f, 0.24f, 0.24f);
        }
        else
        {
            Back.transform.localPosition = new Vector3(0, 29, 0);
            Back.transform.localScale = new Vector3(0.42f, 0.42f, 0.42f);
            Text.transform.localPosition = new Vector3(0, -83, 0);
            Text.transform.localScale = Back.transform.localScale;
        }
    }

    public void TitleEnd(string Level)
    {
        Application.LoadLevel(Level);
    }
}
