using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Advertisements;

public class MainMenu : MonoBehaviour {

    Button btnStart;
    Button btnOptions;

	// Use this for initialization
	void Start () {
        btnStart = gameObject.FindComponentInChildren<Button>(true, "btn_start");
        btnOptions = gameObject.FindComponentInChildren<Button>(true, "btn_options");

        btnOptions.onClick.AddListener(DoOptions);
        btnStart.onClick.AddListener(DoStartGame);

        Advertisement.Initialize("24196");
	}
	
	// Update is called once per frame
	void Update () {	           
	}

    void DoOptions()
    {
        MenuManager.use.Goto(MenuManager.MenuType.OPTIONSMENU);
    }

    void DoStartGame()
    {
        MenuManager.use.Goto(MenuManager.MenuType.LEVELSELECTMENU);
    }
}
