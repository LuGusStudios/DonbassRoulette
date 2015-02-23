using UnityEngine;
using System.Collections;

public class Healing : AreaOfEffect
{
	override public SpellType GetSpellType()
	{
		return SpellType.Healing;
	}

	protected override void ApplyEffect(Collider2D col)
	{
		Body body = col.GetComponent<Body>();
		if(body && body.m_side == m_side)
		{
			body.AddHp(m_value);
		}
	}
}
