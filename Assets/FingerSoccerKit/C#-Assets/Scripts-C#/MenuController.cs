using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
		
	///*************************************************************************///
	/// Main Menu Controller.
	/// This class handles all touch events on buttons, and also updates the 
	/// player status (wins and available-money) on screen.
	///*************************************************************************///

	private float buttonAnimationSpeed = 9;		//speed on animation effect when tapped on button
	public static bool canTap = false;			//flag to prevent double tap
	public AudioClip tapSfx;					//tap sound for buttons click

	//Reference to GameObjects
	public GameObject playerWins;				//UI 3d text object
	public GameObject playerMoney;              //UI 3d text object

    public Texture2D[] availableFlags;
    public TextMesh[] TeamPrice;
    public TapSellUse TPU;
    //*****************************************************************************
    // Init. Updates the 3d texts with saved values fetched from playerprefs.
    //*****************************************************************************
    void Awake() {
        //MHD

        if (PlayerPrefs.GetInt("FirstNationalTeam") != 3 || PlayerPrefs.GetInt("TeamClass") == 1 && PlayerPrefs.GetInt("TournomemtWinning") == 1 || PlayerPrefs.GetInt("TeamClass") == 2 && PlayerPrefs.GetInt("TournomemtWinning") == 2)
            SceneManager.LoadSceneAsync("TeamChooser");


        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;

        playerWins.GetComponent<TextMesh>().text = "" + PlayerPrefs.GetInt("PlayerWins");
        playerMoney.GetComponent<TextMesh>().text = "" + PlayerPrefs.GetInt("PlayerMoney");


        //Check For UnlockTeam
        for (int i = 28; i < TeamPrice.Length; i++)
        {
            if (PlayerPrefs.GetInt("ActiveTeamLock" + availableFlags[i]) == 3 && PlayerPrefs.GetInt("PlayerMoney") >= int.Parse(TeamPrice[i].text))
                 PlayerPrefs.SetInt("Team" + availableFlags[i] + "LockState", 3); // Unlock A National Team
        }
    }




	IEnumerator Start() {

       

        yield return new WaitForSeconds(1.0f);
		canTap = true;
	}
	//*****************************************************************************
	// FSM
	//*****************************************************************************
	void Update (){	
		if(canTap) {
			StartCoroutine(tapManager());
		}
	}

	//*****************************************************************************
	// This function monitors player touches on menu buttons.
	// detects both touch and clicks and can be used with editor, handheld device and 
	// every other platforms at once.
	//*****************************************************************************
	private RaycastHit hitInfo;
	private Ray ray;
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
			
				//Game Modes
				case "gameMode_1":								//player vs AI mode
					playSfx(tapSfx);							//play touch sound
					PlayerPrefs.SetInt("GameMode", 0);			//set game mode to fetch later in "Game" scene
					PlayerPrefs.SetInt("IsTournament", 0);		//are we playing in a tournament?
					PlayerPrefs.SetInt("IsPenalty", 0);			//are we playing penalty kicks?
					StartCoroutine(animateButton(objectHit));	//touch animation effect
					yield return new WaitForSeconds(1.0f);		//Wait for the animation to end
					SceneManager.LoadScene("Config-c#");		//Load the next scene
					break;

				case "gameMode_2":								//two player (human) mode
					playSfx(tapSfx);
					PlayerPrefs.SetInt("GameMode", 1);
					PlayerPrefs.SetInt("IsTournament", 0);
					PlayerPrefs.SetInt("IsPenalty", 0);
					StartCoroutine(animateButton(objectHit));
					yield return new WaitForSeconds(1.0f);
					SceneManager.LoadScene("Config-c#");
					break;	

				case "gameMode_3":
					playSfx(tapSfx);
					PlayerPrefs.SetInt("GameMode", 0);
					PlayerPrefs.SetInt("IsTournament", 1);
					PlayerPrefs.SetInt("IsPenalty", 0);
					StartCoroutine(animateButton(objectHit));
					yield return new WaitForSeconds(1.0f);

					//if we load the tournament scene directly, player won't get the chance to select a team
					//and has to play with the default team
					//SceneManager.LoadScene("Tournament-c#");

					//here we let the player to use to modified config scene to select a team
					SceneManager.LoadScene("Config-c#");

					break;	

				case "gameMode_4":								//Penalty Kicks
					playSfx(tapSfx);
					PlayerPrefs.SetInt("GameMode", 0);
					PlayerPrefs.SetInt("IsTournament", 0);
					PlayerPrefs.SetInt("IsPenalty", 1);
					StartCoroutine(animateButton(objectHit));
					yield return new WaitForSeconds(1.0f);
					SceneManager.LoadScene("Penalty-c#");
					break;
						
				//Option buttons	
				case "Btn-01":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					yield return new WaitForSeconds(1.0f);
					SceneManager.LoadScene("Shop-c#");
					break;

				case "Btn-02":
				case "Status_2":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					yield return new WaitForSeconds(0.7f);
                    TPU.ShowTapSellVideo();
                    print("Show Tapsell Video");
					break;

				case "Btn-03":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					yield return new WaitForSeconds(1.0f);
					Application.Quit();
					break;	

				case "FreeCoinsButton":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					yield return new WaitForSeconds(1.0f);
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