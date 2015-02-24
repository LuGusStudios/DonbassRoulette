using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMenu : MonoBehaviour {

    Button btnPause;

	// Use this for initialization
	void Start () {
        btnPause = gameObject.FindComponentInChildren<Button>(true, "btn_pause");
        btnPause.onClick.AddListener(DoPause);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void DoPause()
    {
        MenuManager.use.Goto(MenuManager.MenuType.PAUSEMENU);
    }
}
