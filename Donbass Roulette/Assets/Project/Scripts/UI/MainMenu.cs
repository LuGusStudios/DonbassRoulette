using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

    Button btnStart;
    Button btnOptions;

	// Use this for initialization
	void Start () {
        btnStart = gameObject.FindComponentInChildren<Button>(true, "btn_start");
        btnOptions = gameObject.FindComponentInChildren<Button>(true, "btn_options");

        btnOptions.onClick.AddListener(GoToOptions);
	}
	
	// Update is called once per frame
	void Update () {	           
	}

    void GoToOptions()
    {
        MenuManager.use.Goto(MenuManager.MenuType.OPTIONSMENU);
    }

}
