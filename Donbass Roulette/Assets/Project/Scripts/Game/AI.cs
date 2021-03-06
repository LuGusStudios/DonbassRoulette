﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : User
{
	[Header("AI Settings")]
	public float m_trySpawnCooldown;
	public float m_forcedSpawnCooldown;

	public float m_spellCastCooldown;
	public float m_spellCastRange;
	public float m_safetyRange;
	public float m_safetyMana;

	public List<float> m_factoriesWeight = new List<float>();

    private bool isStarted = false;

	public override void SetComponents()
	{
		base.SetComponents();
		if(m_factories.Count != m_factoriesWeight.Count)
		{
			Debug.LogError("Factories Weight hasn't all been set");
		}
	}


	override protected void Start()
	{
		base.Start();		
	}

    // This function allows us to keep the AI inactive untill the player has made the first move.
    public void StartAiBehaviour()
    {
        if (isStarted) return;
        isStarted = true;

        StartCoroutine(TryWeightSpawn());
        StartCoroutine(ForcedWeightSpawn());
        StartCoroutine(Spellcast());
    }

	private float GetFactoryChances(int id)
	{
		float chancesValue = 0;
		for(int i = 0; i <= id ; i++)
		{
			chancesValue += m_factoriesWeight[i];
		}
		return chancesValue;
	}

	private float GetTotalChances()
	{
		float sum = 0;
		foreach(float f in m_factoriesWeight)
			sum += f;
		return sum;
	}


	private IEnumerator TryWeightSpawn()
	{
		while(true)
		{

			yield return new WaitForSeconds(m_trySpawnCooldown);
			float spawnChance = Random.Range(0f, GetTotalChances());

			for(int i = 0; i < m_factories.Count; i++)
			{
				if(spawnChance <= GetFactoryChances(i))
				{
					SpawnUnit(m_factories[i], this.m_side);
                    break;
				}
			}
		}
	}
	private IEnumerator ForcedWeightSpawn()
	{
		while(true)
		{
			yield return new WaitForSeconds(m_forcedSpawnCooldown);

			float spawnChance = Random.Range(0f, GetTotalChances());


			for(int i = 0; i < m_factories.Count; i++)
			{
				if(spawnChance <= GetFactoryChances(i))
				{
					while(SpawnUnit(m_factories[i], this.m_side) != true)
					{
						yield return null;
					}
                    break;
				}
			}
		}
	}

	private IEnumerator Spellcast()
	{
		while(true)
		{
			yield return new WaitForSeconds(m_spellCastCooldown);

            if (GameData.use.ai == null || GameData.use.player == null)
                yield break;

            if(Random.Range(0,2) == 0)
                HealingSpellCastCheck(m_safetyRange);
            else
			    OffensiveSpellCastRoutine(m_safetyRange);

            //if(Random.Range(0, 10) == 0) // irregular value factor (frequency)
            //{
            //    if(Random.Range(0, 1) == 0)
            //        OffensiveSpellCastRoutine(m_spellCastRange, m_safetyMana); // TODO : bug, spellCastRange isn't correct (why?)
            //    else
            //        HealingSpellCastCheck(m_spellCastRange, m_safetyMana);
            //}
		}
	}


	public delegate bool UnitCheckFunc(Unit unit);


	public bool UnitCheckHpIsNotFull(Unit unit)
	{
		if(unit.GetHpRatio() < 1)
			return true;
		return false;
	}

	private void OffensiveSpellCastRoutine(float range, float reservedMana = 0)
	{
        Spell spell = GetRandomSpell(AreaOfEffect.SpellType.Offensive);
        if(spell && m_mana - spell.m_cost > reservedMana && spell.GetTimer() <= 0)
		{
            Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, range);
            if (cols.Length > 0)
            {// check if there is any enemy near the base
				
                Unit nearest = NearestSideUnit(this.m_spawner.position, cols, this.m_side.GetOpposite());
				if(nearest)
				{
					Collider2D[] colsAround = Physics2D.OverlapCircleAll(nearest.transform.position, spell.m_prefabEffect.m_range);
					if(colsAround.Length > 0)
					{// check if there is any enemy near the enemy to aim at the same time
						Unit farthest = FarthestSideUnit(nearest.transform.position, colsAround, this.m_side.GetOpposite());
						if(farthest != null)
							SummonSpell(Vector2.Lerp(nearest.transform.position, farthest.transform.position, .5f), spell, this.m_side);
						else
							SummonSpell(nearest.transform.position, spell, this.m_side);
					}
					else
					{
						SummonSpell(nearest.transform.position, spell, this.m_side);
					}
				}
			}
		}
	}

	private void HealingSpellCastCheck(float range, float reservedMana = 0)
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, range);
		if(cols.Length > 0)
		{// check if there is any enemy near the base
			Spell spell = GetRandomSpell(AreaOfEffect.SpellType.Healing);
            if (spell && m_mana - spell.m_cost > reservedMana && spell.GetTimer() <= 0)
			{
				Unit nearest = NearestSideUnit(this.m_spawner.position, cols, this.m_side, UnitCheckHpIsNotFull);

				if(nearest)
				{
					Collider2D[] colsAround = Physics2D.OverlapCircleAll(nearest.transform.position, spell.m_prefabEffect.m_range);
					if(colsAround.Length > 0)
					{// check if there is any enemy near the enemy to aim at the same time
						Unit farthest = FarthestSideUnit(nearest.transform.position, colsAround, this.m_side, UnitCheckHpIsNotFull);
						if(farthest != null)
							SummonSpell(Vector2.Lerp(nearest.transform.position, farthest.transform.position, .5f), spell, this.m_side);
						else
							SummonSpell(nearest.transform.position, spell, this.m_side);
					}
					else
					{
						SummonSpell(nearest.transform.position, spell, this.m_side);
					}
				}
			}
		}
	}

	private Unit NearestSideUnit(Vector2 pos, Collider2D[] cols, Side side, UnitCheckFunc function = null)
	{
		Unit nearest = null;
		float smallestDist = float.MaxValue;

		foreach(Collider2D col in cols)
		{
			Unit unit = col.GetComponent<Unit>();
			if(unit && unit.m_side == side && (function == null || function(unit)))
			{
				float dist = Vector2.Distance(pos, col.transform.position);
				if(dist < smallestDist)
				{
					smallestDist = dist;
					nearest = unit;
				}
			}
		}
		return nearest;
	}
	private Unit FarthestSideUnit(Vector2 pos, Collider2D[] cols, Side side, UnitCheckFunc function = null)
	{
		Unit farthest = null;
		float biggestDist = 0;

		foreach(Collider2D col in cols)
		{
			Unit unit = col.GetComponent<Unit>();
			if(unit && unit.m_side == side && (function == null || function(unit)))
			{
				float dist = Vector2.Distance(pos, col.transform.position);
				if(dist > biggestDist)
				{
					biggestDist = dist;
					farthest = unit;
				}
			}
		}
		return farthest;
	}

	void Update()
	{
        IncomeRubberBanding();       
	}

    float _realIncome = 0;

    void IncomeRubberBanding()
    {

        // Rubber banding
        float playerIncome = GameData.use.player.m_income;

        if (_realIncome == 0)
            _realIncome = m_income;

        if (_realIncome < playerIncome)
            _realIncome += Time.deltaTime * 2;

        if (_realIncome > m_income)
            m_income = Mathf.FloorToInt(_realIncome);

        //Debug.LogWarning("Enemy income: " + m_income);
    }


	void OnDrawGizmos()
	{
		if(m_spawner != null)
		{
			{
				// draw circle2D
				Gizmos.color = Color.yellow;
				const int sections = 12;
				float radius = m_safetyRange;

				Vector3 center = this.m_spawner.position;

				Vector3 prvPos = new Vector3(center.x + radius, center.y, center.z);
				for(float f = 0; f < 2 * Mathf.PI; f += (2 * Mathf.PI) / sections)
				{
					float x = center.x + radius * Mathf.Cos(f);
					float y = center.y - radius * Mathf.Sin(f);

					Vector3 newPos = new Vector3(x, y, center.z);
					Gizmos.DrawLine(prvPos, newPos);
					prvPos = newPos;
				}
			}

			{
				// draw circle2D
				Gizmos.color = Color.white;
				const int sections = 12;
				float radius = m_spellCastRange;
				Vector3 center = this.m_spawner.position;
				Vector3 prvPos = new Vector3(center.x + radius, center.y, center.z);
				for(float f = 0; f < 2 * Mathf.PI; f += (2 * Mathf.PI) / sections)
				{
					float x = center.x + radius * Mathf.Cos(f);
					float y = center.y - radius * Mathf.Sin(f);

					Vector3 newPos = new Vector3(x, y, center.z);
					Gizmos.DrawLine(prvPos, newPos);
					prvPos = newPos;
				}
			}
		}
	}
}
