using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayService : MonoBehaviour {
    public bool isConnectToGooglePlayServices { get; set; }
    // Use this for initialization
    void Start () {
        // Activate the Google Play Games platform
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        // authenticate user:
        ConnectToGooglePlayService();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public bool ConnectToGooglePlayService()
    {
        // authenticate user:
        if (!isConnectToGooglePlayServices)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                isConnectToGooglePlayServices = success;
            });
        }
        return isConnectToGooglePlayServices;
    }
}
