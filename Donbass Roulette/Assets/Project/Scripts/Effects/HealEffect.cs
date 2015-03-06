using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealEffect : MonoBehaviour 
{
    protected Unit unit = null;
    protected ParticleSystem healParticles = null;

	public void SetupLocal()
	{
        gameObject.CacheComponent<ParticleSystem>(ref healParticles);
        unit = this.gameObject.FindComponentInParent<Unit>();

        if (unit != null)
        {
            unit.m_onGainHealth += OnGainHealth;
              unit.m_delDeath += OnDeath;
        }
	}
	
	public void SetupGlobal()
	{
	}
	
	protected void Awake()
	{
		SetupLocal();
	}

	protected void Start() 
	{
		SetupGlobal();
	}

    protected void Update()
    { 
    
    }

    protected void OnGainHealth()
    {
        healParticles.Play();
    }
        	
	protected void OnDeath() 
	{
        healParticles.Stop();
        healParticles.Clear();
	}
}
