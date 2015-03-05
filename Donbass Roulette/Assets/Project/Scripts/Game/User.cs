using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class User : MonoBehaviour {
	[Header("User Settings")]

	public Side m_side;
    public Faction faction = Faction.None;

	public int m_money;
	public int m_manaMax;

	protected int m_mana;

	public int m_manaRegen;
	public float m_manaRegenCooldown;

	public float m_manaRegenMultiplicator;
	public int m_manaRegenPrice;
	public float m_manaRegenPriceMultiplicator;
	
	public int m_income;
	public float m_incomeCooldown;
	
	public float m_incomeMultiplicator;
	public int m_incomePrice;
	public float m_incomePriceMultiplicator;


	public float m_incomeRateMultiplicator;
	public int m_incomeRatePrice;
	public float m_incomeRatePriceMultiplicator;


	public List<Spell> m_spells = new List<Spell>();
	public List<Factory> m_factories = new List<Factory>();
	public Transform m_spawner;

	protected bool BuyIncome()
	{
		if(m_money >= m_incomePrice)
		{
			m_money -= m_incomePrice;
			m_income = (int)(m_income * m_incomeMultiplicator);
			m_incomePrice = (int)(m_incomePrice * m_incomePriceMultiplicator);
            return true;
		}
        return false;
	}

    protected void GetMillionDollars()
    {
        m_money += 1000000;
    }

	protected bool BuyManaRegen()
	{
		if(m_money >= m_manaRegenPrice)
		{
			m_money -= m_manaRegenPrice;
			m_manaRegen = (int)(m_manaRegen * m_manaRegenMultiplicator);
			m_manaRegenPrice = (int)(m_manaRegenPrice * m_manaRegenMultiplicator);
            return true;
		}
        return false;
	}

	protected bool BuyIncomeRate()
	{
		if(m_money >= m_incomeRatePrice)
		{
			m_money -= m_incomeRatePrice;
			m_incomeCooldown *= m_incomeRateMultiplicator;
			m_incomeRatePrice = (int)(m_incomeRatePrice * m_incomeRatePriceMultiplicator);
            return true;
		}
        return false;
	}




	public int GetMana()
	{
		return m_mana;
	}


	virtual public void SetComponents()
	{
		for(int i = 0; i < m_factories.Count; i++)
		{
			string name = m_factories[i].name;
			m_factories[i] = Instantiate(m_factories[i]) as Factory;
			m_factories[i].name = name;
			m_factories[i].transform.parent = this.transform;
		}
		for(int i = 0; i < m_spells.Count; i++)
		{
			string name = m_spells[i].name;
			m_spells[i] = Instantiate(m_spells[i]) as Spell;
			m_spells[i].name = name;
			m_spells[i].transform.parent = this.transform;
		}
	}

	virtual protected void Start()
	{
		if(m_factories.Count > 0)
			SetComponents();
		StartCoroutine(RegenMP());
		StartCoroutine(Income());
	}

	public void Initialize()
	{
		m_mana = m_manaMax;
	}


	IEnumerator RegenMP()
	{
		while(true)
		{
			yield return new WaitForSeconds(m_manaRegenCooldown);
			m_mana += m_manaRegen;
			if(m_mana > m_manaMax)
				m_mana = m_manaMax;
		}

	}
	IEnumerator Income()
	{
		while(true)
		{
			yield return new WaitForSeconds(m_incomeCooldown);
			m_money += m_income;
		}
	}



	protected bool SpawnUnit(Factory factory, Side side)
	{
		if(m_money >= factory.m_price)
		{
			if(factory.Spawn(m_spawner.position.x, side))
			{
				m_money -= factory.m_price;
				return true;
			}
		}
		return false;
	}

	protected bool SummonSpell(Vector2 pos, Spell spell, Side side)
	{
		if(m_mana >= spell.m_cost)
		{
            if (spell.Summon(pos, side))
			    m_mana -= spell.m_cost;
			return true;
		}
		return false;
	}



	public List<Spell> GetSpells(AreaOfEffect.SpellType type)
	{
		List<Spell> outputSpells = new List<Spell>();

		foreach(Spell spell in m_spells)
		{
			if(spell.m_prefabEffect.GetSpellType() == type)
			{
				outputSpells.Add(spell);
			}
		}
		return outputSpells;
	}

	public Spell GetRandomSpell(AreaOfEffect.SpellType type)
	{
		List<Spell> typeSpells = GetSpells(type);
		if(typeSpells.Count > 0)
		{
			int rand = Random.Range(0, typeSpells.Count - 1);
			return typeSpells[rand];
		}
		return null;
	}

	





}
