﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class UnitAnim : MonoBehaviour 
{
	// Change these default values on the unit if needed.
	public string walkAnimName = "Walking";
	public string attackAnimName = "Firing";
	public string deathAnimName = "Death";

	protected Unit m_unit;
	protected Animator m_animator;

	protected virtual void Awake()
	{
		m_unit =  this.gameObject.FindComponentInParent<Unit>(true);
		m_animator = this.gameObject.FindComponent<Animator>();
	}

    protected virtual void Start() 
	{
		if (m_unit != null)
		{
			m_unit.m_delAttack += PlayAttackAnim;
			m_unit.m_DelMove += PlayWalkAnim;
			m_unit.m_delDeath += PlayDeathAnim;
		}
		else
		{
			Debug.LogError("UnitAnim: " + this.transform.Path() +  " did not find Unit controller script! Disabling animations.");
			this.enabled = false;
		}

		if (m_animator == null)
		{
			Debug.LogError("UnitAnim: " + this.transform.Path() +  " did not find anim controller! Disabling animations.");
			this.enabled = false;
		}
	
	}

    protected virtual void PlayWalkAnim()
	{
		m_animator.Play(walkAnimName);
	}

    protected virtual void PlayAttackAnim()
	{
		m_animator.Play(attackAnimName);
	}

    protected virtual void PlayDeathAnim()
	{
		m_animator.Play(deathAnimName);
	}
}
