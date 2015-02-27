using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {
	public AreaOfEffect m_prefabEffect;
	public int m_cost;
    public Sprite icon;

	public float m_couldown;
	private float m_timer = 0;

	public bool Summon( Vector2 pos, Side side )
	{
		if(m_timer <= 0)
		{
			AreaOfEffect aoe = Instantiate(m_prefabEffect, pos, Quaternion.identity) as AreaOfEffect;
			aoe.Initialize(side);
            m_timer = m_couldown;
            return true;
		}
        return false;
	}


	public float GetTimer()
	{
		return m_timer;
	}

	void Update()
	{
		if(m_timer > 0)
		{
			m_timer -= Time.deltaTime;
		}
		else
		{
			m_timer = 0;
		}
	}
}
