using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

    Button btnStart;
    Button btnOptions;
    Button btnAbout;

	// Use this for initialization
	void Start () {
        btnStart = gameObject.FindComponentInChildren<Button>(true, "btn_start");
        btnOptions = gameObject.FindComponentInChildren<Button>(true, "btn_options");
        btnAbout = gameObject.FindComponentInChildren<Button>(true, "btn_about");

        btnOptions.onClick.AddListener(DoOptions);
        btnStart.onClick.AddListener(DoStartGame);
        btnAbout.onClick.AddListener(DoAbout);
        
        LugusCamera.game.gameObject.FindComponentInChildren<MinimapCamera>(true).gameObject.SetActive(false);

        Time.timeScale = 1;
        CameraController.use.isIdleAnimating = true;
	}

    void OnEnable()
    {
        CameraController.use.isIdleAnimating = true;
    }
	
	// Update is called once per frame
	void Update () {	           
	}

    void DoOptions()
    {
        MenuManager.use.Goto(MenuManager.MenuType.OPTIONSMENU);
    }

    void DoAbout()
    {
        MenuManager.use.Goto(MenuManager.MenuType.ABOUTMENU);
    }

    void DoStartGame()
    {
        MenuManager.use.Goto(MenuManager.MenuType.LEVELSELECTMENU);
    }
}
