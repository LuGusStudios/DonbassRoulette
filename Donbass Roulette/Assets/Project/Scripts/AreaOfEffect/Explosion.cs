using UnityEngine;
using System.Collections;


public class Explosion : AreaOfEffect {
	public int m_damage;

	override public SpellType GetSpellType()
	{
		return SpellType.Offensive;
	}


	override protected void ApplyEffect(Collider2D col)
	{
		Body body = col.GetComponent<Body>();
		if(body /*&& body.m_side != m_side*/)
		{
			body.ReduceHp(m_damage);
		}
	}

}
