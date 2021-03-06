﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using I2.Loc;

public class ShopManager : MonoBehaviour {

    private float buttonAnimationSpeed = 9; //speed on animation effect when tapped on button
    private bool canTap = true;         //flag to prevent double tap

    public AudioClip tapSfx;                //buy sfx
    public GameObject playerMoney;          //Reference to 3d text
    private int availableMoney;
    public GameObject firstMenu, NationalMenu, IranFCMenu, EroupeFCMenu;
    public GameObject[] TeamsLocked;
    public Texture2D[] TeamsFlag;
    public TextMesh[] TeamPrice;

    public GoogleAds GA;
    public int touchCount;
    public int maximumAdstouchcount;

    //*****************************************************************************
    // Init. 
    //*****************************************************************************
    void Awake (){
		//Updates 3d text with saved values fetched from playerprefs
		availableMoney = PlayerPrefs.GetInt("PlayerMoney");
		playerMoney.GetComponent<TextMesh>().text = "" + availableMoney;


       
    }

	//*****************************************************************************
	// FSM 
	//*****************************************************************************
	void Update (){
        // Ads Part
        if (Input.touchCount > 0)
        {
            if (!PlayerPrefs.HasKey("TouchCount"))
                PlayerPrefs.SetInt("TouchCount", 0);
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (SceneManager.GetActiveScene().name != "Menu - c#")
                    touchCount = PlayerPrefs.GetInt("TouchCount");

                touchCount++;
                PlayerPrefs.SetInt("TouchCount", touchCount);
                if (touchCount % maximumAdstouchcount == 0)
                {
                    GA.ShowInterstitialAd();
                    touchCount = 0;
                    PlayerPrefs.SetInt("TouchCount", touchCount);
                }
            }
        }
        // Ads Part
        if (canTap) {
			StartCoroutine(tapManager());
		}

		if(Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("Menu-c#");
	}

	//*****************************************************************************
	// This function monitors player touches on menu buttons.
	// detects both touch and clicks and can be used with editor, handheld device and 
	// every other platforms at once.
	//*****************************************************************************
	private RaycastHit hitInfo;
	private Ray ray;
	//private string saveName = "";
	IEnumerator tapManager (){

		//Mouse of touch?
		if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
			ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		else if(Input.GetMouseButtonUp(0))
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		else
			yield break;
			
		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			switch(objectHit.name) {
			
			case "shopItem_1":
				StartCoroutine(animateButton(objectHit));
				//play sfx
				playSfx(tapSfx);
				//Wait
				yield return new WaitForSeconds(0.5f);
				//load next level
				SceneManager.LoadScene("ShopFormation-c#");
				break;
					
			case "shopItem_2":
				StartCoroutine(animateButton(objectHit));
				//play sfx
				playSfx(tapSfx);
				//Wait
				yield return new WaitForSeconds(0.5f);
				//load next level
				SceneManager.LoadScene("ShopTeam-c#");
				break;

                case "National":
                    StartCoroutine(animateButton(objectHit));
                    //play sfx
                    playSfx(tapSfx);
                    //Wait
                    yield return new WaitForSeconds(0.5f);
                    //MHD
                    for (int i = 0; i < TeamsLocked.Length; i++)
                    {
                        if (PlayerPrefs.GetInt("Team" + TeamsFlag[i] + "LockState") == 3)
                            TeamsLocked[i].GetComponent<Renderer>().material.mainTexture = TeamsFlag[i];
                    }
                    firstMenu.SetActive(false);
                    NationalMenu.SetActive(true);
                    break;

                case "Iran FC":                   
                    StartCoroutine(animateButton(objectHit));
                    //play sfx
                    playSfx(tapSfx);
                    //Wait
                    yield return new WaitForSeconds(0.5f);
                    //MHD
                    firstMenu.SetActive(false);
                    IranFCMenu.SetActive(true);
                    for (int i = 28; i < TeamsLocked.Length -16; i++)
                    {
                        int teamPrice = int.Parse(TeamPrice[i].text);

                        if (PlayerPrefs.GetInt("Team" + TeamsFlag[i] + "isTheFirstTeam") == 3 && PlayerPrefs.GetInt("Team" + TeamsFlag[i] + "LockState") == 3)
                        {
                            TeamsLocked[i].GetComponent<Renderer>().material.mainTexture = TeamsFlag[i];
                            TeamPrice[i].text = LocalizationManager.GetTermTranslation("UI.GamePlay.Unlocked");
                            TeamPrice[i].color = Color.blue;
                        }
                        else if (PlayerPrefs.GetInt("ActiveTeamLock" + TeamsFlag[i]) == 3 && PlayerPrefs.GetInt("PlayerMoney") >= teamPrice)
                        {
                            TeamsLocked[i].GetComponent<Renderer>().material.mainTexture = TeamsFlag[i];
                            TeamPrice[i].text = LocalizationManager.GetTermTranslation("UI.GamePlay.Unlocked");
                            TeamPrice[i].color = Color.blue;
                        }
                    }
                    break;

                case "World FC":
                    StartCoroutine(animateButton(objectHit));
                    //play sfx
                    playSfx(tapSfx);
                    //Wait
                    yield return new WaitForSeconds(0.5f);
                    //MHD
                    firstMenu.SetActive(false);
                    EroupeFCMenu.SetActive(true);
                    for (int i = 44; i < TeamsLocked.Length; i++)
                    {
                        int teamPrice = int.Parse(TeamPrice[i].text);

                        if (PlayerPrefs.GetInt("Team" + TeamsFlag[i] + "isTheFirstTeam") == 3 && PlayerPrefs.GetInt("Team" + TeamsFlag[i] + "LockState") == 3)
                        {
                            TeamsLocked[i].GetComponent<Renderer>().material.mainTexture = TeamsFlag[i];
                            TeamPrice[i].text = LocalizationManager.GetTermTranslation("UI.GamePlay.Unlocked");
                            TeamPrice[i].color = Color.blue;
                        }
                        else if (PlayerPrefs.GetInt("ActiveTeamLock" + TeamsFlag[i]) == 3 && PlayerPrefs.GetInt("PlayerMoney") >= teamPrice)
                        {
                            TeamsLocked[i].GetComponent<Renderer>().material.mainTexture = TeamsFlag[i];
                            TeamPrice[i].text = LocalizationManager.GetTermTranslation("UI.GamePlay.Unlocked");
                            TeamPrice[i].color = Color.blue;
                        }
                    }
                    break;

                case "Btn-Back":
				StartCoroutine(animateButton(objectHit));
				playSfx(tapSfx);
                    if (!firstMenu.activeSelf)
                    {
                        NationalMenu.SetActive(false);
                        IranFCMenu.SetActive(false);
                        EroupeFCMenu.SetActive(false);
                        firstMenu.SetActive(true);
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.0f);
                        SceneManager.LoadScene("Menu-c#");
                    }              
				break;
			}	
		}
	}

	//*****************************************************************************
	// This function animates a button by modifying it's scales on x-y plane.
	// can be used on any element to simulate the tap effect.
	//*****************************************************************************
	IEnumerator animateButton ( GameObject _btn  ){
		canTap = false;
		Vector3 startingScale = _btn.transform.localScale;	//initial scale	
		Vector3 destinationScale = startingScale * 1.1f;		//target scale
		
		//Scale up
		float t = 0.0f; 
		while (t <= 1.0f) {
			t += Time.deltaTime * buttonAnimationSpeed;
			_btn.transform.localScale = new Vector3( Mathf.SmoothStep(startingScale.x, destinationScale.x, t),
			                                        Mathf.SmoothStep(startingScale.y, destinationScale.y, t),
			                                        _btn.transform.localScale.z);
			yield return 0;
		}
		
		//Scale down
		float r = 0.0f; 
		if(_btn.transform.localScale.x >= destinationScale.x) {
			while (r <= 1.0f) {
				r += Time.deltaTime * buttonAnimationSpeed;
				_btn.transform.localScale = new Vector3( Mathf.SmoothStep(destinationScale.x, startingScale.x, r),
				                                        Mathf.SmoothStep(destinationScale.y, startingScale.y, r),
				                                        _btn.transform.localScale.z);
				yield return 0;
			}
		}
		
		if(r >= 1)
			canTap = true;
	}

	//*****************************************************************************
	// Play sound clips
	//*****************************************************************************
	void playSfx ( AudioClip _clip  ){
		GetComponent<AudioSource>().clip = _clip;
		if(!GetComponent<AudioSource>().isPlaying) {
			GetComponent<AudioSource>().Play();
		}
	}

}