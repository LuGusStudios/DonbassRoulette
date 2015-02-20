using UnityEngine;
using System.Collections;

public class Healing : AreaOfEffect
{
	public int m_heal;

	override public SpellType GetSpellType()
	{
		return SpellType.Healing;
	}

	protected override void ApplyEffect(Collider2D col)
	{
		Body body = col.GetComponent<Body>();
		if(body && body.m_side == m_side)
		{
			body.AddHp(m_heal);
		}
	}
}
