using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AboutMenu : MonoBehaviour {

    Button btnBack;

	// Use this for initialization
	void Start () {
        btnBack = gameObject.FindComponentInChildren<Button>(true, "btn_back");
        btnBack.onClick.AddListener(DoBack);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void DoBack()
    {
        MenuManager.use.Goto(MenuManager.MenuType.MAINMENU);
    }
}
