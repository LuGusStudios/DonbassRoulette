using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Side
{
	None = 0,
	Left = 1,
	Right = 2,
};

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




public class GameData : MonoBehaviour {
    public Camera m_camera;
	public User m_leftUser;
	public User m_rightUser;
	public List<Factory> m_factories = new List<Factory>();
	public List<Spell> m_spells = new List<Spell>();
	public List<GameObject> m_structures = new List<GameObject>();
    public Map m_map;


	void Awake()
	{
		m_leftUser.m_side = Side.Left;
		m_rightUser.m_side = Side.Right;
	}




	public GameObject FindStructure(string name)
	{
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


	public Factory FindFactory(string name)
	{
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
		m_leftUser.SetComponents();
		m_rightUser.SetComponents();
	}
}
