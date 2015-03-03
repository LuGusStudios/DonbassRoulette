using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitSounds : MonoBehaviour 
{
    public AudioClip[] attackSounds;
    public AudioClip[] deathSounds;
    public AudioClip[] spawnSounds;
    

    protected Unit unit = null;


	public void SetupLocal()
	{
        unit = gameObject.FindComponent<Unit>();

        if (unit != null)
        {
            unit.m_delAttack += OnAttack;
            unit.m_delDeath += OnDeath;
            unit.m_delSpawn += OnSpawn;
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

    protected void OnAttack()
    {
        if (attackSounds.Length > 0)
            SoundManager.use.PlaySound(LugusAudio.use.SFX(), attackSounds[Random.Range(0, attackSounds.Length)]);
    }

    protected void OnDeath()
    {
        if (deathSounds.Length > 0)
            SoundManager.use.PlaySound(LugusAudio.use.SFX(), deathSounds[Random.Range(0, deathSounds.Length)]);
    }

    protected void OnSpawn()
    {
        //if (unit != null )
        //{
        //    if (spawnSounds.Length > 0)
        //        SoundManager.use.PlaySound(LugusAudio.use.SFX(), spawnSounds[Random.Range(0, spawnSounds.Length)]);
        //}
    }

    
}
