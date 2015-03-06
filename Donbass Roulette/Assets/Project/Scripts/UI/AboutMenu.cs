using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AboutMenu : MonoBehaviour {

    Button btnBack;
    float timer = 0;

	// Use this for initialization
	void Start () {
        btnBack = gameObject.FindComponentInChildren<Button>(true, "btn_back");
        btnBack.onClick.AddListener(DoBack);
	}

    void OnEnable()
    {
        timer = Time.realtimeSinceStartup;
    }

	// Update is called once per frame
	void Update () {
	
	}

    void DoBack()
    {
        AnalyticsIntegration.ReadAboutEvent(Time.realtimeSinceStartup - timer);
        MenuManager.use.Goto(MenuManager.MenuType.MAINMENU);
    }
}
