using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameMenu : MonoBehaviour {

    Button btnPause;

    public List<Button> unitButtons;
    public List<Button> powerButtons;

    Player _player = null;

	// Use this for initialization
	void Start () {
        btnPause = gameObject.FindComponentInChildren<Button>(true, "btn_pause");
        btnPause.onClick.AddListener(DoPause);        
	}

    void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        SetupButtons();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetupButtons()
    {
        if (_player == null) 
        {
            Debug.LogWarning("GameMenu: No player found");
            return;
        }
        if (_player.m_factories.Count > unitButtons.Count)
        {
            Debug.LogError("GameMenu: Not enough buttons for units");
            return;
        }
        if (_player.m_spells.Count > powerButtons.Count)
        {
            Debug.LogError("GameMenu: Not enough buttons for powers");
            return;
        }

        // Hide all buttons
        foreach (Button b in unitButtons)
            b.gameObject.SetActive(false);

        foreach(Button b in powerButtons)
            b.gameObject.SetActive(false);


        // Setup Unit Buttons 
        for (int i = 0; i < _player.m_factories.Count; i++)
        {
            Factory f = _player.m_factories[i];            
            Button b = unitButtons[i];            

            b.image.sprite = f.icon;
            b.onClick.AddListener(() => { _player.DoSpawnUnit(f); });
            b.gameObject.FindComponentInChildren<Text>(true).text = f.m_price.ToString();
            b.gameObject.SetActive(true);
        }

        // Setup Power Buttons 
        for (int i = 0; i < _player.m_spells.Count; i++)
        {
            Spell s = _player.m_spells[i];
            Button b = powerButtons[i];

            b.image.sprite = s.icon;
            b.onClick.AddListener(() => { _player.DoCastSpell(s); });
            b.gameObject.FindComponentInChildren<Text>(true).text = s.m_cost.ToString();
            b.gameObject.SetActive(true);
        }

    }

    void DoPause()
    {
        MenuManager.use.Goto(MenuManager.MenuType.PAUSEMENU);
    }
    
}
