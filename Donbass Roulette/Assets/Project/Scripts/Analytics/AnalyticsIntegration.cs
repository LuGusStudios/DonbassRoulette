using UnityEngine;
using UnityEngine.Cloud.Analytics;
using System.Collections;
using System.Collections.Generic;

public class AnalyticsIntegration : MonoBehaviour {

	// Use this for initialization
	void Start () {
        const string projectID = "b00ca4c4-4823-4203-a666-4f76fc8d5890";
        UnityAnalytics.StartSDK(projectID);        
	}


    public static void ReadAboutEvent(float time)
    {
        UnityAnalytics.CustomEvent("ReadAbout", new Dictionary<string, object>
            {
                {"time", time},                
            });
    }

    public static void StartGameEvent()
    {
        UnityAnalytics.CustomEvent("StartGame", null);
    }

    public static void PickTeamEvent(string team)
    {
        UnityAnalytics.CustomEvent("PickTeam", new Dictionary<string, object>
            {
                {"team", team},                
            });
    }

    public static void GameOverEvent(int score, float time)
    {        
        UnityAnalytics.CustomEvent("GameOver", new Dictionary<string, object>
            {
                {"score", score},
                {"time", time}
            });
    }

    public static void OpenOptionsEvent()
    {
        UnityAnalytics.CustomEvent("OpenOptions", null);
    }

    public static void ClickedSocialFacebookEvent()
    {
        UnityAnalytics.CustomEvent("ClickFacebook", null);
    }

    public static void ClickedSocialTwitterEvent()
    {
        UnityAnalytics.CustomEvent("ClickTwitter", null);
    }
 
}
