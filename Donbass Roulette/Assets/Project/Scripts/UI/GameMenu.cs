using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameMenu : MonoBehaviour {

    Button btnPause;

    public List<Button> unitButtons;
    public List<Button> powerButtons;
    public Button btnIncome;
    public Button btnTechnology;

    Player _player = null;

	// Use this for initialization
	void Start () {
        btnPause = gameObject.FindComponentInChildren<Button>(true, "btn_pause");
        btnPause.onClick.AddListener(DoPause);        
      
        _player = FindObjectOfType<Player>();

        btnIncome.onClick.AddListener(_player.DoBuyIncome);
        btnTechnology.onClick.AddListener(_player.DoBuyMana);
	}

    void OnEnable()
    {
        Time.timeScale = 1;

                
        SetupButtons();        
    }
	
	// Update is called once per frame
	void Update () {
        UpdateIcons();
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

    void UpdateIcons()
    {
        Color activeCol = new Color(1, 1, 1);
        Color inactiveCol = new Color(0.3f, 0.3f, 0.3f);

        // Update Units
        for (int i = 0; i < _player.m_factories.Count; i++)
        {
            Factory f = _player.m_factories[i];
            Button b = unitButtons[i];

            if (_player.m_money >= f.m_price)
                b.image.color = activeCol;
            else
                b.image.color = inactiveCol;

            b.image.fillAmount = f.GetTimerPercentage();
        }

        // Update powers
        for (int i = 0; i < _player.m_spells.Count; i++)
        {
            Spell s = _player.m_spells[i];
            Button b = powerButtons[i];

            if (s == _player.GetCastingSpell())
            {
                float glowVal = Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * 4));
                b.image.color = new Color(1, glowVal, glowVal);
            }
            else
            {
                if (_player.GetMana() >= s.m_cost)
                    b.image.color = activeCol;
                else
                    b.image.color = inactiveCol;
                b.image.fillAmount = s.GetTimerPercentage();
            }
        }

        // update income
        btnIncome.gameObject.FindComponentInChildren<Text>(true).text = _player.m_incomePrice.ToString();
        if (_player.m_money >= _player.m_incomePrice)        
            btnIncome.image.color = activeCol;       
        else
            btnIncome.image.color = inactiveCol;

        // update technology
        btnTechnology.gameObject.FindComponentInChildren<Text>(true).text = _player.m_manaRegenPrice.ToString();
        if (_player.m_money >= _player.m_manaRegenPrice)
            btnTechnology.image.color = activeCol;
        else
            btnTechnology.image.color = inactiveCol;
    }

    void DoPause()
    {
        MenuManager.use.Goto(MenuManager.MenuType.PAUSEMENU);
    }
    
}
