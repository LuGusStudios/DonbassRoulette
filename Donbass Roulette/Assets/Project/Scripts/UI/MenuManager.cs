using UnityEngine;
using System.Collections;

public class MenuManager : LugusSingletonExisting<MenuManager> {

    public enum MenuType { 
        NONE = 0,
        MAINMENU = 1,
        GAMEMENU = 2,
        OPTIONSMENU = 3
    }

    private MainMenu _mainMenu;
    private OptionsMenu _optionsMenu;

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
                
                break;
            case MenuType.OPTIONSMENU:
                _optionsMenu.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    void HideAllMenus()
    {
        _mainMenu.gameObject.SetActive(false);
        _optionsMenu.gameObject.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        _mainMenu = gameObject.FindComponentInChildren<MainMenu>(true);
        _optionsMenu = gameObject.FindComponentInChildren<OptionsMenu>(true);

        HideAllMenus();
        ShowMenu(currentMenu);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
