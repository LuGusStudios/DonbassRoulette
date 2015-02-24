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

     
}
