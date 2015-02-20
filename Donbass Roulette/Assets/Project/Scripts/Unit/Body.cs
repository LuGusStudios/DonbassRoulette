using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour {
	public float m_hpMax;
	protected float m_hp;

	public float m_deathDestroyTime;
	public Side m_side;
	
	public delegate void Delegate();
	public Delegate m_DelDeath;

	protected void Start()
	{
		m_hp = m_hpMax;
		if(m_side == Side.Right)
			this.transform.localScale = this.transform.localScale.xMul(-1);
	}


	protected IEnumerator Death()
	{
		if(m_DelDeath != null)
			m_DelDeath();
		yield return new WaitForSeconds(m_deathDestroyTime); // death animation time
		Destroy(this.transform.parent.gameObject);
	}


	public void ReduceHp( float value )
	{
		m_hp -= value;
		if(m_hp < 0)
		{
			m_hp = 0;
			StartCoroutine(Death());
		}
	}

	public void AddHp( float value )
	{
		m_hp += value;
		if(m_hp > m_hpMax)
			m_hp = m_hpMax;
	}


	public float GetHpRatio()
	{
		return (m_hp / m_hpMax);
	}



}
