﻿using UnityEngine;
using System.Collections;

public class MenuManager : LugusSingletonExisting<MenuManager> {

    public enum MenuType { 
        NONE = 0,
        MAINMENU = 1,
        GAMEMENU = 2,
        OPTIONSMENU = 3,
        PAUSEMENU = 4,
        GAMEOVERMENU = 5,
        SHAREMENU = 6,
        LEVELSELECTMENU = 7,
        ABOUTMENU = 8
    }

    private MainMenu _mainMenu;
    private OptionsMenu _optionsMenu;
    private GameMenu _gameMenu;
    private PauseMenu _pauseMenu;
    private GameOverMenu _gameOverMenu;
    private ShareMenu _shareMenu;
    private LevelSelectMenu _levelSelectMenu;
    private AboutMenu _aboutMenu;

    public MenuType currentMenu = MenuType.NONE;
    public MenuType previousMenu = MenuType.NONE;

    public void Goto(MenuType menu)
    {        
        ShowMenu(menu);
        previousMenu = currentMenu;
        currentMenu = menu;
    }

    public void GotoPrevious()
    {
        ShowMenu(previousMenu);
        currentMenu = previousMenu;        
    }

    void ShowMenu(MenuType menu)
    {
        //if (menu == currentMenu) return;

        HideAllMenus();

        switch (menu)
        { 
            case MenuType.MAINMENU:
                _mainMenu.gameObject.SetActive(true);
                break;
            case MenuType.GAMEMENU:
                _gameMenu.gameObject.SetActive(true);
                break;                
            case MenuType.OPTIONSMENU:
                _optionsMenu.gameObject.SetActive(true);
                break;
            case MenuType.PAUSEMENU:
                _pauseMenu.gameObject.SetActive(true);
                break;
            case MenuType.GAMEOVERMENU:
                _gameOverMenu.gameObject.SetActive(true);
                break;
            case MenuType.SHAREMENU:
                _shareMenu.gameObject.SetActive(true);
                break;
            case MenuType.LEVELSELECTMENU:
                _levelSelectMenu.gameObject.SetActive(true);
                break;
            case MenuType.ABOUTMENU:
                _aboutMenu.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    void HideAllMenus()
    {
        _mainMenu.gameObject.SetActive(false);
        _optionsMenu.gameObject.SetActive(false);
        _gameMenu.gameObject.SetActive(false);
        _pauseMenu.gameObject.SetActive(false);
        _gameOverMenu.gameObject.SetActive(false);
        _shareMenu.gameObject.SetActive(false);
        _levelSelectMenu.gameObject.SetActive(false);
        _aboutMenu.gameObject.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        _mainMenu = gameObject.FindComponentInChildren<MainMenu>(true);
        _optionsMenu = gameObject.FindComponentInChildren<OptionsMenu>(true);
        _gameMenu = gameObject.FindComponentInChildren<GameMenu>(true);
        _pauseMenu = gameObject.FindComponentInChildren<PauseMenu>(true);
        _gameOverMenu = gameObject.FindComponentInChildren<GameOverMenu>(true);
        _shareMenu = gameObject.FindComponentInChildren<ShareMenu>(true);
        _levelSelectMenu = gameObject.FindComponentInChildren<LevelSelectMenu>(true);
        _aboutMenu = gameObject.FindComponentInChildren<AboutMenu>(true);

        HideAllMenus();

        currentMenu = CrossSceneMenuInfo.use.nextMenuOnReload;

        ShowMenu(currentMenu);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
