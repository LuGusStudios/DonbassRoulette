using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class UnitAnim : MonoBehaviour {
	public Unit m_unit;
	protected Animator m_animator;

	void Awake()
	{
		m_animator = this.GetComponent<Animator>();
	}
	void Start () {
		m_unit.m_DelAttack += Attack;
		m_unit.m_DelMove += Move;
		m_unit.m_DelDeath += Death;

	}


	void Move()
	{
		m_animator.SetInteger("AnimState", 0);
	}

	void Attack()
	{
		m_animator.SetInteger("AnimState", 1); // TODO : relaunch the attack animation after each attack
	}

	void Death()
	{

	}

	// Update is called once per frame
	void Update () {
	}
}
