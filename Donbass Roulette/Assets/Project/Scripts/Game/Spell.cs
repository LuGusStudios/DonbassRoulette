using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {
	public AreaOfEffect m_prefabEffect;
	public int m_cost;
    public float instantiateDelay = 0.0f;
    public Sprite icon;

	public float m_cooldown;
	private float m_timer = 0;

	public bool Summon( Vector2 pos, Side side )
	{
		if(m_timer <= 0)
		{
            LugusCoroutines.use.StartRoutine(AreaAppear(pos, side)); 
            return true;
		}
        return false;
	}
    protected IEnumerator AreaAppear(Vector2 position, Side side)
    {
        OnBegin(position, side);

        yield return new WaitForSeconds(instantiateDelay);

        AreaOfEffect aoe = Instantiate(m_prefabEffect, position, Quaternion.identity) as AreaOfEffect;
        aoe.Initialize(side);
        m_timer = m_cooldown;

        OnInstantiate(aoe);
    }

    // Override, for instance to have a projectile travel at the beginning of the spell.
    protected virtual void OnBegin(Vector2 position, Side side)
    { 
    }

    protected virtual void OnInstantiate(AreaOfEffect aoe)
    { 
    }

    public float GetTimerPercentage()
    {
        return 1.0f - (m_timer / m_cooldown);
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
