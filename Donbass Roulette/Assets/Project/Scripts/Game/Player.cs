﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : User {

	protected Spell m_spellCasting;
	

	void Update () {
		if(LugusInput.use.down && m_spellCasting != null)
		{
			SummonSpell(LugusCamera.game.ScreenToWorldPoint(LugusInput.use.currentPosition), m_spellCasting, this.m_side);
			m_spellCasting = null;
		}
	}


	void OnGUI()
	{
		Vector2 size = Vector2.zero;
		if(m_factories.Count > 0)
		{
			size = new Vector2(Screen.width / m_factories.Count, 40);
		}

		if(size.x > 130)
		{
			size.x = 130;
		}

        Rect millionDollars = new Rect(Screen.width - 100, 0, 100, 50);
        if (GUI.Button(millionDollars, "Million Dollars!"))
            GetMillionDollars();


        //Rect buyIncomeButton = new Rect(0, Screen.height - 4 * size.y, size.x, size.y);
        //if(GUI.Button(buyIncomeButton, "$+\n" + m_incomePrice + "$"))
        //    BuyIncome();

		

        //Rect buyIncomeRateButton = new Rect(1 * size.x, Screen.height - 4 * size.y, size.x, size.y);
        //if(GUI.Button(buyIncomeRateButton, "$/s+\n" + m_incomeRatePrice + "$"))
        //    BuyIncomeRate();


        //Rect buyManaRegenButton = new Rect(2 * size.x, Screen.height - 4 * size.y, size.x, size.y);
        //if(GUI.Button(buyManaRegenButton, "MP+\n" + m_manaRegenPrice + "$"))
        //    BuyManaRegen();


        //for(int i = 0; i < m_factories.Count; i++)
        //{
        //    Rect factoryButton = new Rect(i * size.x, Screen.height - size.y, size.x, size.y);
        //    if(GUI.Button(factoryButton, m_factories[i].name + " : " + m_factories[i].m_price + "$\n" + m_factories[i].GetTimer()))
        //    {
        //        SpawnUnit(m_factories[i], this.m_side);
        //    }
        //}
        //for(int i = 0; i < m_spells.Count; i++)
        //{
        //    Rect spellButton = new Rect(i * size.x, Screen.height - 2 * size.y, size.x, size.y);
        //    if(GUI.Button(spellButton, m_spells[i].name + ": " + m_spells[i].m_cost + "MP\n" + m_spells[i].GetTimer()))
        //    {
        //        if(m_mana >= m_spells[i].m_cost)
        //            m_spellCasting = m_spells[i];
        //        else
        //            Debug.Log("Not enough MP got " + m_mana + ", requires " + m_spells[i].m_cost);
        //    }
        //}
	}

    public void DoSpawnUnit(Factory f)
    {        
        SpawnUnit(f, this.m_side);
    }

    public void DoCastSpell(Spell s)
    {
        if (m_mana >= s.m_cost)
            m_spellCasting = s;
        else
            Debug.Log("Not enough MP got " + m_mana + ", requires " + s.m_cost);
    }

    public Spell GetCastingSpell()
    {
        return m_spellCasting;
    }

    public void DoBuyIncome()
    {
        BuyIncome();
    }

    public void DoBuyMana()
    {
        BuyManaRegen();
    }
}
