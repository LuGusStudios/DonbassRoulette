﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    Button btnResume;
    Button btnMainMenu;
    Button btnOptions;

	// Use this for initialization
	void Start () {
        btnResume = gameObject.FindComponentInChildren<Button>(true, "btn_resume");
        btnMainMenu = gameObject.FindComponentInChildren<Button>(true, "btn_mainmenu");
        btnOptions = gameObject.FindComponentInChildren<Button>(true, "btn_options");

        btnResume.onClick.AddListener(DoResume);
        btnMainMenu.onClick.AddListener(DoMainMenu);
        btnOptions.onClick.AddListener(DoOptions);            

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void DoResume()
    {
        MenuManager.use.Goto(MenuManager.MenuType.GAMEMENU);
    }

    void DoMainMenu()
    {
        MenuManager.use.Goto(MenuManager.MenuType.MAINMENU);
    }

    void DoOptions()
    {
        MenuManager.use.Goto(MenuManager.MenuType.OPTIONSMENU);
    }
}
