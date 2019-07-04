using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ConfigController : MonoBehaviour {
		
	///*************************************************************************///
	/// Main Config Controller.
	/// This class provides the available settings to the players, like formations,
	/// gameDuration, etc... and then prepare the main game with the setting.
	/// 
	/// Important:
	/// Upon the addition of tournament mode, this config scene also manages to set the player team 
	/// for the tournament by hiding unnecessary options. But in normal gameplay, it shows the full settings.
	/// Example:
	/// in Tournament mode: We just show the Player 1 team and formation selection options.
	/// We also config the start button to load the tournament scene afterward.
	///*************************************************************************///

	private int isTournamentMode;
	//required game objects to active/deactive
	public GameObject p1TeamSel;		//p1 team selection settings
	public GameObject p1FormationSel;	//p1 formation selection settings
	public GameObject p2TeamSel;		//p2 team selection settings
	public GameObject p2FormationSel;	//p2 formation selection settings
	public GameObject timeSel;			//time selection settings


	private float buttonAnimationSpeed = 11;	//speed on animation effect when tapped on button
	private int barScaleDivider = 10;			//we divide actual time/power setting of each team by this number
	private bool  canTap = true;				//flag to prevent double tap
	public AudioClip tapSfx;					//tap sound for buttons click

    public GameObject National, IranFC, IranFCDisabled, WorldFC, WorldFCDisabled;
	public Texture2D[] availableTeams;			//just the images.
         //just the images.
    public string[] availableFormations;		//Just the string values. We setup actual values somewhere else.
	public string[] availableTimes;				//Just the string values. We setup actual values somewhere else.

	//Reference to gameObjects
	public GameObject p1Team;
	public GameObject p1PowerBar;
	public GameObject p1TimeBar;
	public GameObject p2Team;
	public GameObject p2PowerBar;
	public GameObject p2TimeBar;
	public GameObject p1FormationLabel;			//UI 3d text object
	public GameObject p2FormationLabel;			//UI 3d text object
	public GameObject gameTimeLabel;			//UI 3d text object
    public GameObject btnStart;
    int TeamUnlockTemp;
    int TeamUnlockTempReverse;

    private int p1FormationCounter = 0;			//Actual player-1 formation index
	private int p1TeamCounter = 0;				//Actual player-1 team index
	private int p2FormationCounter = 0;			//Actual player-2 formation index
	private int p2TeamCounter = 0;				//Actual player-2 team index
	private int timeCounter = 0;                //Actual game-time index

    //*****************************************************************************
    // Init. Updates the 3d texts with saved values fetched from playerprefs.
    //***************************************************************************
    void Awake()
    {
        ////MHD
        //if (!PlayerPrefs.HasKey("TeamClass"))
        //{
        //    PlayerPrefs.SetInt("TeamClass", 0);
        //}

        if (Application.loadedLevelName != "TeamChooser")
        {


            // Unlock Iran FC check
            if (PlayerPrefs.GetInt("FirstIranFCTeam") == 3)
            {
                IranFC.SetActive(true);
                IranFCDisabled.SetActive(false);
            }
            else
            {
                IranFC.SetActive(false);
                IranFCDisabled.SetActive(true);
            }
            if (PlayerPrefs.GetInt("FirstEroupFCTeam") == 3)
            {
                WorldFC.SetActive(true);
                WorldFCDisabled.SetActive(false);
            }
            else
            {
                WorldFC.SetActive(false);
                WorldFCDisabled.SetActive(true);
            }
            // PlayerPrefs.SetInt("Team" + availableTeams[0] + "LockState", 3);
            //check if this config scene is getting used for tournament or normal play mode
            isTournamentMode = PlayerPrefs.GetInt("IsTournament");

            if (isTournamentMode == 1)
            {

                //first of all, check if we are going to continue an unfinished tournament
                if (PlayerPrefs.GetInt("TorunamentLevel") > 0)
                {
                    //if so, there is no need for any configuration. load the next scene.
                    SceneManager.LoadScene("Tournament-c#");
                    return;
                }


                //disable unnecessary options
                p2TeamSel.SetActive(false);
                p2FormationSel.SetActive(false);
                timeSel.SetActive(false);

                p1TeamSel.transform.position = new Vector3(0, 4.5f, -1);
                p1FormationSel.transform.position = new Vector3(0, -4.3f, -1);
            }

            p1FormationLabel.GetComponent<TextMesh>().text = availableFormations[p1FormationCounter];   //loads default formation


            p2FormationLabel.GetComponent<TextMesh>().text = availableFormations[p2FormationCounter];   //loads default formation


            gameTimeLabel.GetComponent<TextMesh>().text = availableTimes[timeCounter].faConvert();              //loads default game-time
        }
    }
     void Start()
    {

        if (Application.loadedLevelName == "TeamChooser")
        {
            print("Team Class : " + PlayerPrefs.GetInt("TeamClass"));

            if (PlayerPrefs.GetInt("TeamClass") == 0)
                p1TeamCounter = 0;
            else if (PlayerPrefs.GetInt("TeamClass") == 1)
                p1TeamCounter = 28;
            else if (PlayerPrefs.GetInt("TeamClass") == 2)
                p1TeamCounter = 44;

            p1Team.GetComponent<Renderer>().material.mainTexture = availableTeams[p1TeamCounter];
            p1PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).x / barScaleDivider,
                              p1PowerBar.transform.localScale.y,
                              p1PowerBar.transform.localScale.z);
            p1TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).y / barScaleDivider,
                                                         p1TimeBar.transform.localScale.y,
                                                         p1TimeBar.transform.localScale.z);
        }
    }
    //*****************************************************************************
    // FSM
    //*****************************************************************************
    void Update (){
        //  print("TeamClass : " + PlayerPrefs.GetInt("TeamClass")+ " , " + "p1TeamCounter : " + p1TeamCounter);
        //  print("Reverse : "+ TeamUnlockTempReverse);
        if (Input.GetKey(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            print("PlayerPrefs.DeleteAll");
        }
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

                case "National":
                    //    print("National");
                    btnStart.SetActive(true);
                    PlayerPrefs.SetInt("TeamClass",0);
                    for (int i = 0; i < 28; i++)
                    {
                       if(PlayerPrefs.GetInt("Team" + availableTeams[i] + "LockState") == 3)
                        {
                            p1TeamCounter = i;
                            p1Team.GetComponent<Renderer>().material.mainTexture = availableTeams[p1TeamCounter];                     
                            p1PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).x / barScaleDivider,
                                              p1PowerBar.transform.localScale.y,
                                              p1PowerBar.transform.localScale.z);
                            p1TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).y / barScaleDivider,
                                                                         p1TimeBar.transform.localScale.y,
                                                                         p1TimeBar.transform.localScale.z);
                            p2TeamCounter = i;
                            p2Team.GetComponent<Renderer>().material.mainTexture = availableTeams[p2TeamCounter];
                            p2PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p2TeamCounter).x / barScaleDivider,
                                              p2PowerBar.transform.localScale.y,
                                              p2PowerBar.transform.localScale.z);
                            p2TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p2TeamCounter).y / barScaleDivider,
                                                                         p2TimeBar.transform.localScale.y,
                                                                         p2TimeBar.transform.localScale.z);
                            TeamUnlockTemp = i;
                            break;
                        }
                    }
                    for (int i = 27; i > 0; i--)
                    {
                        if (PlayerPrefs.GetInt("Team" + availableTeams[i] + "LockState") == 3)
                        {
                            TeamUnlockTempReverse = i;
                            break;
                        }
                    }


                     National.SetActive(false);
                    IranFC.SetActive(false);
                    IranFCDisabled.SetActive(false);
                    WorldFC.SetActive(false);
                    WorldFCDisabled.SetActive(false);

                    if (isTournamentMode == 1)
                    {
                        p1TeamSel.SetActive(true);
                        p1FormationSel.SetActive(true);
                        p2TeamSel.SetActive(false);
                        p2FormationSel.SetActive(false);
                        timeSel.SetActive(false);

                        p1TeamSel.transform.position = new Vector3(0, 4.5f, -1);
                        p1FormationSel.transform.position = new Vector3(0, -4.3f, -1);
                    }
                    else
                    {
                        p1TeamSel.SetActive(true);
                        p1FormationSel.SetActive(true);
                        p2TeamSel.SetActive(true);
                        p2FormationSel.SetActive(true);
                        timeSel.SetActive(true);   
                    }
                   
                    break;
                case "Iran FC":
                    //    print("Iran FC");
                    btnStart.SetActive(true);
                    PlayerPrefs.SetInt("TeamClass", 1);
                    for (int i = 28; i < 44; i++)
                    {
                        if (PlayerPrefs.GetInt("Team" + availableTeams[i] + "LockState") == 3)
                        {
                            p1TeamCounter = i;
                            p1Team.GetComponent<Renderer>().material.mainTexture = availableTeams[p1TeamCounter];
                            p1PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).x / barScaleDivider,
                                              p1PowerBar.transform.localScale.y,
                                              p1PowerBar.transform.localScale.z);
                            p1TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).y / barScaleDivider,
                                                                         p1TimeBar.transform.localScale.y,
                                                                         p1TimeBar.transform.localScale.z);
                            p2TeamCounter = i;
                            p2Team.GetComponent<Renderer>().material.mainTexture = availableTeams[p2TeamCounter];
                            p2PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p2TeamCounter).x / barScaleDivider,
                                              p2PowerBar.transform.localScale.y,
                                              p2PowerBar.transform.localScale.z);
                            p2TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p2TeamCounter).y / barScaleDivider,
                                                                         p2TimeBar.transform.localScale.y,
                                                                         p2TimeBar.transform.localScale.z);
                            TeamUnlockTemp = i;
                            break;
                        }
                    }
                    for (int i = 43; i > 27; i--)
                    {
                        if (PlayerPrefs.GetInt("Team" + availableTeams[i] + "LockState") == 3)
                        {
                            TeamUnlockTempReverse = i;
                            break;
                        }
                    }
                    
                    National.SetActive(false);
                     IranFC.SetActive(false);
                    IranFCDisabled.SetActive(false);
                      WorldFC.SetActive(false);
                    WorldFCDisabled.SetActive(false);

                    if (isTournamentMode == 1)
                    {
                        p1TeamSel.SetActive(true);
                        p1FormationSel.SetActive(true);
                        p2TeamSel.SetActive(false);
                        p2FormationSel.SetActive(false);
                        timeSel.SetActive(false);

                        p1TeamSel.transform.position = new Vector3(0, 4.5f, -1);
                        p1FormationSel.transform.position = new Vector3(0, -4.3f, -1);
                    }
                    else
                    {
                        p1TeamSel.SetActive(true);
                        p1FormationSel.SetActive(true);
                        p2TeamSel.SetActive(true);
                        p2FormationSel.SetActive(true);
                        timeSel.SetActive(true);
                    }
                    break;
                case "World FC":
                    //  print("World FC");
                    btnStart.SetActive(true);
                    PlayerPrefs.SetInt("TeamClass", 2);
                    for (int i = 44; i < availableTeams.Length; i++)
                    {
                        if (PlayerPrefs.GetInt("Team" + availableTeams[i] + "LockState") == 3)
                        {
                            p1TeamCounter = i;
                            p1Team.GetComponent<Renderer>().material.mainTexture = availableTeams[p1TeamCounter];
                            p1PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).x / barScaleDivider,
                                              p1PowerBar.transform.localScale.y,
                                              p1PowerBar.transform.localScale.z);
                            p1TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).y / barScaleDivider,
                                                                         p1TimeBar.transform.localScale.y,
                                                                         p1TimeBar.transform.localScale.z);
                            p2TeamCounter = i;
                            p2Team.GetComponent<Renderer>().material.mainTexture = availableTeams[p2TeamCounter];
                            p2PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p2TeamCounter).x / barScaleDivider,
                                              p2PowerBar.transform.localScale.y,
                                              p2PowerBar.transform.localScale.z);
                            p2TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p2TeamCounter).y / barScaleDivider,
                                                                         p2TimeBar.transform.localScale.y,
                                                                         p2TimeBar.transform.localScale.z);
                            TeamUnlockTemp = i;
                            break;
                        }
                    }
                    for (int i = availableTeams.Length - 1; i > 43; i--)
                    {
                        if (PlayerPrefs.GetInt("Team" + availableTeams[i] + "LockState") == 3)
                        {
                            TeamUnlockTempReverse = i;
                            break;
                        }
                    }

                    National.SetActive(false);
                     IranFC.SetActive(false);
                    IranFCDisabled.SetActive(false);
                      WorldFC.SetActive(false);
                    WorldFCDisabled.SetActive(false);

                    if (isTournamentMode == 1)
                    {
                        p1TeamSel.SetActive(true);
                        p1FormationSel.SetActive(true);
                        p2TeamSel.SetActive(false);
                        p2FormationSel.SetActive(false);
                        timeSel.SetActive(false);

                        p1TeamSel.transform.position = new Vector3(0, 4.5f, -1);
                        p1FormationSel.transform.position = new Vector3(0, -4.3f, -1);
                    }
                    else
                    {
                        p1TeamSel.SetActive(true);
                        p1FormationSel.SetActive(true);
                        p2TeamSel.SetActive(true);
                        p2FormationSel.SetActive(true);
                        timeSel.SetActive(true);
                    }
                    break;
                case "Confirm":
                    // Unlock A Team
                    playSfx(tapSfx);
                    StartCoroutine(animateButton(objectHit));   //button scale-animation to user input
                    yield return new WaitForSeconds(0.07f);
                    PlayerPrefs.SetInt("Team" + availableTeams[p1TeamCounter] + "LockState", 3); // Unlock A Team
                    PlayerPrefs.SetInt("Team" + availableTeams[p1TeamCounter] + "isTheFirstTeam", 3); // Unlock A Team
                    if (PlayerPrefs.GetInt("TeamClass") == 0)
                        PlayerPrefs.SetInt("FirstNationalTeam", 3); // national Team Unlocked
                    else if (PlayerPrefs.GetInt("TeamClass") == 1)
                        PlayerPrefs.SetInt("FirstIranFCTeam", 3); // IranFC Team Unlocked
                    else if (PlayerPrefs.GetInt("TeamClass") == 2)
                        PlayerPrefs.SetInt("FirstEroupFCTeam", 3); // EroupFC Team Unlocked
                    PlayerPrefs.SetInt("TeamClass",0);
                    Application.LoadLevelAsync("Menu-c#");
                    break;
                case "p1-TBR":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));   //button scale-animation to user input

                              p1TeamCounter++;        //cycle through available team indexs for this player. This is the main index value.
                              fixCounterLengths();        //when reached to the last option, start from the first index of the other side.
                    if (Application.loadedLevelName != "TeamChooser")
                    {
                        for (int i = p1TeamCounter; i < availableTeams.Length; i++)
                        {

                            if (PlayerPrefs.GetInt("Team" + availableTeams[i] + "LockState") == 3)
                            {
                                if (i <= TeamUnlockTempReverse)
                                {
                                    p1Team.GetComponent<Renderer>().material.mainTexture = availableTeams[i]; //set the flag on UI

                                    p1PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(i).x / barScaleDivider,
                                                      p1PowerBar.transform.localScale.y,
                                                      p1PowerBar.transform.localScale.z);

                                    p1TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(i).y / barScaleDivider,
                                                                                 p1TimeBar.transform.localScale.y,
                                                                                 p1TimeBar.transform.localScale.z);
                                    p1TeamCounter = i;

                                }
                                break;
                            }
                            if (p1TeamCounter > TeamUnlockTempReverse)
                            {
                                p1Team.GetComponent<Renderer>().material.mainTexture = availableTeams[TeamUnlockTemp]; //set the flag on UI

                                p1PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(TeamUnlockTemp).x / barScaleDivider,
                                                  p1PowerBar.transform.localScale.y,
                                                  p1PowerBar.transform.localScale.z);

                                p1TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(TeamUnlockTemp).y / barScaleDivider,
                                                                             p1TimeBar.transform.localScale.y,
                                                                             p1TimeBar.transform.localScale.z);
                                p1TeamCounter = TeamUnlockTemp;
                            }
                        }
                    }
                    else
                    {
                        p1Team.GetComponent<Renderer>().material.mainTexture = availableTeams[p1TeamCounter]; //set the flag on UI

                        p1PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).x / barScaleDivider,
                                          p1PowerBar.transform.localScale.y,
                                          p1PowerBar.transform.localScale.z);

                        p1TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).y / barScaleDivider,
                                                                     p1TimeBar.transform.localScale.y,
                                                                     p1TimeBar.transform.localScale.z);
                    }


                    yield return new WaitForSeconds(0.07f);
					StartCoroutine(animateButton(p1Team));
					break;

				case "p1-TBL":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));   //button scale-animation to user input
                    p1TeamCounter--;            //cycle through available team indexs for this player. This is the main index value.
                    fixCounterLengths();        //when reached to the last option, start from the first index of the other side.
                    if (Application.loadedLevelName != "TeamChooser")
                    {
                        for (int i = p1TeamCounter; i >= 0; i--)
                        {

                            if (PlayerPrefs.GetInt("Team" + availableTeams[i] + "LockState") == 3)
                            {
                                if (i >= TeamUnlockTemp)
                                {
                                    p1Team.GetComponent<Renderer>().material.mainTexture = availableTeams[i]; //set the flag on UI

                                    p1PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(i).x / barScaleDivider,
                                                      p1PowerBar.transform.localScale.y,
                                                      p1PowerBar.transform.localScale.z);

                                    p1TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(i).y / barScaleDivider,
                                                                                 p1TimeBar.transform.localScale.y,
                                                                                 p1TimeBar.transform.localScale.z);
                                    p1TeamCounter = i;

                                }
                                break;
                            }
                        }
                        if (p1TeamCounter < TeamUnlockTemp)
                        {
                            p1Team.GetComponent<Renderer>().material.mainTexture = availableTeams[TeamUnlockTempReverse]; //set the flag on UI

                            p1PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(TeamUnlockTempReverse).x / barScaleDivider,
                                              p1PowerBar.transform.localScale.y,
                                              p1PowerBar.transform.localScale.z);

                            p1TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(TeamUnlockTempReverse).y / barScaleDivider,
                                                                         p1TimeBar.transform.localScale.y,
                                                                         p1TimeBar.transform.localScale.z);
                            p1TeamCounter = TeamUnlockTempReverse;
                        }
                    }
                    else
                    {
                        p1Team.GetComponent<Renderer>().material.mainTexture = availableTeams[p1TeamCounter]; //set the flag on UI

                        p1PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).x / barScaleDivider,
                                          p1PowerBar.transform.localScale.y,
                                          p1PowerBar.transform.localScale.z);

                        p1TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(p1TeamCounter).y / barScaleDivider,
                                                                     p1TimeBar.transform.localScale.y,
                                                                     p1TimeBar.transform.localScale.z);
                    }

                    yield return new WaitForSeconds(0.07f);
					StartCoroutine(animateButton(p1Team));
					break;

				case "p2-TBR":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));	//button scale-animation to user input
					p2TeamCounter++;			//cycle through available team indexs for this player. This is the main index value.
					fixCounterLengths();        //when reached to the last option, start from the first index of the other side.
                    for (int i = p2TeamCounter; i < availableTeams.Length; i++)
                    {

                        if (PlayerPrefs.GetInt("Team" + availableTeams[i] + "LockState") == 3)
                        {
                            if (i <= TeamUnlockTempReverse)
                            {
                                p2Team.GetComponent<Renderer>().material.mainTexture = availableTeams[i]; //set the flag on UI

                                p2PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(i).x / barScaleDivider,
                                                  p2PowerBar.transform.localScale.y,
                                                  p2PowerBar.transform.localScale.z);

                                p2TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(i).y / barScaleDivider,
                                                                             p2TimeBar.transform.localScale.y,
                                                                             p2TimeBar.transform.localScale.z);
                                p2TeamCounter = i;

                            }
                            break;
                        }
                    }
                    if (p2TeamCounter > TeamUnlockTempReverse)
                    {
                        p2Team.GetComponent<Renderer>().material.mainTexture = availableTeams[TeamUnlockTemp]; //set the flag on UI

                        p2PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(TeamUnlockTemp).x / barScaleDivider,
                                          p2PowerBar.transform.localScale.y,
                                          p2PowerBar.transform.localScale.z);

                        p2TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(TeamUnlockTemp).y / barScaleDivider,
                                                                     p2TimeBar.transform.localScale.y,
                                                                     p2TimeBar.transform.localScale.z);
                        p2TeamCounter = TeamUnlockTemp;
                    }

                    yield return new WaitForSeconds(0.07f);
					StartCoroutine(animateButton(p2Team));
					break;

				case "p2-TBL":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));	//button scale-animation to user input
					p2TeamCounter--;			//cycle through available team indexs for this player. This is the main index value.
					fixCounterLengths();        //when reached to the last option, start from the first index of the other side.
                    for (int i = p2TeamCounter; i >= 0; i--)
                    {

                        if (PlayerPrefs.GetInt("Team" + availableTeams[i] + "LockState") == 3)
                        {
                            if (i >= TeamUnlockTemp)
                            {
                                p2Team.GetComponent<Renderer>().material.mainTexture = availableTeams[i]; //set the flag on UI

                                p2PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(i).x / barScaleDivider,
                                                  p2PowerBar.transform.localScale.y,
                                                  p2PowerBar.transform.localScale.z);

                                p2TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(i).y / barScaleDivider,
                                                                             p2TimeBar.transform.localScale.y,
                                                                             p2TimeBar.transform.localScale.z);
                                p2TeamCounter = i;

                            }
                            break;
                        }
                    }
                    if (p2TeamCounter < TeamUnlockTemp)
                    {
                        p2Team.GetComponent<Renderer>().material.mainTexture = availableTeams[TeamUnlockTempReverse]; //set the flag on UI

                        p2PowerBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(TeamUnlockTempReverse).x / barScaleDivider,
                                          p2PowerBar.transform.localScale.y,
                                          p2PowerBar.transform.localScale.z);

                        p2TimeBar.transform.localScale = new Vector3(TeamsManager.getTeamSettings(TeamUnlockTempReverse).y / barScaleDivider,
                                                                     p2TimeBar.transform.localScale.y,
                                                                     p2TimeBar.transform.localScale.z);
                        p2TeamCounter = TeamUnlockTempReverse;
                    }
                    yield return new WaitForSeconds(0.07f);
					StartCoroutine(animateButton(p2Team));
					break;
			
				case "p1-FBL":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));	//button scale-animation to user input
					p1FormationCounter--;		//cycle through available formation indexs for this player. This is the main index value.
					fixCounterLengths();		//when reached to the last option, start from the first index of the other side.
					p1FormationLabel.GetComponent<TextMesh>().text = availableFormations[p1FormationCounter]; //set the string on the UI
					break;
					
				case "p1-FBR":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					p1FormationCounter++;
					fixCounterLengths();
					p1FormationLabel.GetComponent<TextMesh>().text = availableFormations[p1FormationCounter];
					break;
					
				case "p2-FBL":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					p2FormationCounter--;
					fixCounterLengths();
					p2FormationLabel.GetComponent<TextMesh>().text = availableFormations[p2FormationCounter];
					break;
				
				case "p2-FBR":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					p2FormationCounter++;
					fixCounterLengths();
					p2FormationLabel.GetComponent<TextMesh>().text = availableFormations[p2FormationCounter];
					break;
					
				case "durationBtnLeft":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					timeCounter--;
					fixCounterLengths();
					gameTimeLabel.GetComponent<TextMesh>().text = availableTimes[timeCounter].faConvert();
					break;
					
				case "durationBtnRight":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					timeCounter++;
					fixCounterLengths();
					gameTimeLabel.GetComponent<TextMesh>().text = availableTimes[timeCounter].faConvert();
					break;
					
				case "Btn-Back":
                    btnStart.SetActive(false);
                    playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					yield return new WaitForSeconds(0.5f);
                    //No need to save anything
                    if (p1TeamSel.activeSelf)
                    {
                        p1TeamSel.SetActive(false);
                        p1FormationSel.SetActive(false);
                        p2TeamSel.SetActive(false);
                        p2FormationSel.SetActive(false);
                        timeSel.SetActive(false);
                        National.SetActive(true);
                        // Unlock Iran FC check
                        if (PlayerPrefs.GetInt("FirstIranFCTeam") == 3)
                        {
                            IranFC.SetActive(true);
                            IranFCDisabled.SetActive(false);
                        }
                        else
                        {
                            IranFC.SetActive(false);
                            IranFCDisabled.SetActive(true);
                        }
                        if (PlayerPrefs.GetInt("FirstEroupFCTeam") == 3)
                        {
                            WorldFC.SetActive(true);
                            WorldFCDisabled.SetActive(false);
                        }
                        else
                        {
                            WorldFC.SetActive(false);
                            WorldFCDisabled.SetActive(true);
                        }
                        p1TeamCounter = 0;
                        p2TeamCounter = 0;
                    }
                    else
                        SceneManager.LoadScene("Menu-c#");
					break;
					
				case "Btn-Start":
					playSfx(tapSfx);
					StartCoroutine(animateButton(objectHit));
					//Save configurations
					PlayerPrefs.SetInt("PlayerFormation", p1FormationCounter);		//save the player-1 formation index
					PlayerPrefs.SetInt("PlayerFlag", p1TeamCounter);				//save the player-1 team index

					PlayerPrefs.SetInt("Player2Formation", p2FormationCounter);		//save the player-2 formation index
					PlayerPrefs.SetInt("Player2Flag", p2TeamCounter);				//save the player-2 team index
					//Opponent uses the exact same settings as player-2, so:
					PlayerPrefs.SetInt("OpponentFormation", p2FormationCounter);	//save the Opponent formation index
					PlayerPrefs.SetInt("OpponentFlag", p2TeamCounter);				//save the Opponent team index

					PlayerPrefs.SetInt("GameTime", timeCounter);					//save the game-time value
					//** Please note that we just set the indexes here. We fetch the actual index values in the <<Game>> scene.
					
					yield return new WaitForSeconds(0.5f);

					//if we are using the config for a tournament, we should load it as the next scene.
					//otherwise we can instantly jump to the Game scene in normal play mode.
					if(isTournamentMode == 1)
						SceneManager.LoadScene("Tournament-c#");
					else
						SceneManager.LoadScene("Game-c#");

					break;
               

            }	
		}
	}

	//*****************************************************************************
	// When selection form available options, when player reaches the last option,
	// and still taps on the next option, this will cycle it again to the first element of options.
	// This is for p-1, p-2 and time settings.
	//*****************************************************************************
	void fixCounterLengths (){
		//set array counters limitations
		
		//Player-1 formation
		if(p1FormationCounter > availableFormations.Length - 1)
			p1FormationCounter = 0;
		if(p1FormationCounter < 0)
			p1FormationCounter = availableFormations.Length - 1;

        //Player-1 team
        if (PlayerPrefs.GetInt("TeamClass") == 0) // if is National
        {
            if (p1TeamCounter > 27)
                p1TeamCounter = 0;
            if (p1TeamCounter < 0)
                p1TeamCounter = 27;
        }
        else if (PlayerPrefs.GetInt("TeamClass") == 1) // if is Iran FC
        {
            if (p1TeamCounter > 43)
                p1TeamCounter = 28;
            if (p1TeamCounter < 28)
                p1TeamCounter = 43;
        }
        else if (PlayerPrefs.GetInt("TeamClass") == 2) // if is Eroupe FC
        {
            if (p1TeamCounter > availableTeams.Length - 1)
                p1TeamCounter = 44;
            if (p1TeamCounter < 44)
                p1TeamCounter = availableTeams.Length - 1;
        }



        //Player-2 formation
        if (p2FormationCounter > availableFormations.Length - 1)
			p2FormationCounter = 0;
		if(p2FormationCounter < 0)
			p2FormationCounter = availableFormations.Length - 1;

        ////Player-2 team
        //if (PlayerPrefs.GetInt("TeamClass") == 0) // if is National
        //{
        //    if (p2TeamCounter > 27)
        //        p2TeamCounter = 0;
        //    if (p2TeamCounter < 0)
        //        p2TeamCounter = 27;
        //}
        //else if (PlayerPrefs.GetInt("TeamClass") == 1) // if is Iran FC
        //{
        //    if (p2TeamCounter > 43)
        //        p2TeamCounter = 28;
        //    if (p2TeamCounter < 28)
        //        p2TeamCounter = 43;
        //}
        //else if (PlayerPrefs.GetInt("TeamClass") == 2) // if is World FC
        //{
        //    if (p2TeamCounter > availableTeams.Length - 1)
        //        p2TeamCounter = 44;
        //    if (p2TeamCounter < 44)
        //        p2TeamCounter = availableTeams.Length - 1;
        //}



        //GameTime
        if (timeCounter > availableTimes.Length - 1)
			timeCounter = 0;
		if(timeCounter < 0)
			timeCounter = availableTimes.Length - 1;
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