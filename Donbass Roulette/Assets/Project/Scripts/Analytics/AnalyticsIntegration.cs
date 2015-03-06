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

    public static void GameOverEvent(Dictionary<string, int> units, float time)
    {
        string pMedic = "PlayerMedic";
        string pRPG = "PlayerRPG";
        string pSoldier = "PlayerSoldier";
        string pTank = "PlayerTank";

        string aMedic = "AIMedic";
        string aRPG = "AIRPG";
        string aSoldier = "AISoldier";
        string aTank = "AITank";

        UnityAnalytics.CustomEvent("GameOver", new Dictionary<string, object>
            {
                {pSoldier, units[pSoldier]},
                {pMedic, units[pMedic]},
                {pRPG, units[pRPG]},
                {pTank, units[pTank]},

                {aSoldier, units[aSoldier]},
                {aMedic, units[aMedic]},
                {aRPG, units[aRPG]},
                {aTank, units[aTank]},

                {"time", time}
            });

        Debug.Log("SentGameOverEvent");
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
