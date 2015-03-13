using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Side
{
	None = 0,
	Left = 1,
	Right = 2,
};

public enum Faction
{ 
    None = 0,
    Rebel = 1,
    Ukraine = 2
}

public static class SideExtension
{
	public static Side GetOpposite(this Side side)
	{
		switch(side)
		{
			case Side.Left : return Side.Right;
			case Side.Right : return Side.Left;
			default: return Side.None;
		}
	}
}




public class GameData : LugusSingletonExisting<GameData> {
    public Camera m_camera;
	public User player;
	public User ai;
	public List<Factory> m_factories = new List<Factory>();
	public List<Spell> m_spells = new List<Spell>();
	public List<GameObject> m_structures = new List<GameObject>();
    public Map m_map;

    protected string factionReplaceString = "Faction";

    private bool isBattleStarted = false;
    public delegate void StartBattleEvent();
    public StartBattleEvent startBattleEvent;

    public bool ceasefireBroken = false;
    public float ceasefireDuration = 60.0f;

	void Awake()
	{
		player.m_side = Side.Left;
		ai.m_side = Side.Right;

        CrossSceneMenuInfo.use.lvlDuration = 0;
	}

    void Update()
    {
        CheckCeasefire();
    }

    public void CheckCeasefire()
    {
        if (!player) return;
        if (!ai) return;

        if (!ceasefireBroken && CrossSceneMenuInfo.use.lvlDuration > ceasefireDuration)
        {
            EndGame(ai.m_side);
        }
    }

    public void EndGame(Side side)
    {
        CrossSceneMenuInfo.use.isPlayerWinner = player.m_side != side;

        ai.StopAllCoroutines();
        player.StopAllCoroutines();

        if (side == Side.Right)
        {
            Destroy(ai.gameObject);
        }
        else if (side == Side.Left)
        {
            Destroy(player.gameObject);
        }

        MenuManager.use.Goto(MenuManager.MenuType.GAMEOVERMENU);
    }


	public GameObject FindStructure(string name, Faction faction)
	{
        name = name.Replace(factionReplaceString, faction.ToString());

        foreach (GameObject obj in m_structures)
        {
            if (obj.name == name)
            {
                return obj;
            }
        }


        Debug.LogError("GameData: Could not find structure named: " + name);
		return null;
	}


	public Factory FindFactory(string name, Faction faction)
	{
        name = name.Replace(factionReplaceString, faction.ToString());

        foreach (Factory factory in m_factories)
        {
            if (factory.name == name)
            {
                return factory;
            }
        }

        Debug.LogError("GameData: No factory found with name: " + name + ".");
		return null;
	}

	public Spell FindSpell(string name)
	{
		foreach(Spell spell in m_spells)
			if(spell.name == name)
				return spell;

        Debug.LogError("GameData: Could not find spelll named: " + name);
		return null;
	}

	public void InstantiateUsersComponents()
	{
		player.SetComponents();
		ai.SetComponents();
	}

    public void BeginBattle()
    {
        if (isBattleStarted) return;        
        isBattleStarted = true;

        ceasefireBroken = true;
        (ai as AI).StartAiBehaviour();
        startBattleEvent();
    }
}
