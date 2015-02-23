using UnityEngine;
using System.Collections;

public class Entity : Body {
	public float m_damage;
	public float m_range;
	public Projectile m_projectile;

	public delegate void Delegate();
	public Delegate m_DelAttack;
	

	public float m_attackCouldown = 2;
	protected float m_attackTimer = 0;

	// Update is called once per frame
	virtual protected void Update () {
		if(m_attackTimer > 0)
			m_attackTimer -= Time.deltaTime;

		if(m_attackTimer <= 0)
		{
            Body onRangeEnemy = GetNearestSideBody(this.m_side.GetOpposite(), m_range);
			if(onRangeEnemy != null)
				Attack(onRangeEnemy);
		}
	}

	void Attack(Body body)
	{
		if( m_DelAttack != null )
			m_DelAttack();

		if(m_projectile != null)
		{// range attack
            
			Projectile projectile = Instantiate(m_projectile,this.transform.position, Quaternion.identity) as Projectile;
			projectile.Initialize(this.m_side, this.m_damage, body.transform.position);
		}
		else
		{// melee attack (immediate)
			body.ReduceHp(m_damage);
		}

		m_attackTimer = m_attackCouldown;
		
	}

	protected Body GetNearestSideBody(Side side, float range, Composition composition = Composition.None)
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll( new Vector2( this.transform.position.x, 0 ), range);

		if(cols.Length > 0)
		{
			// process the exceptions non-attackable
			float smallestDist = float.MaxValue;
			Body nearest = null;
			foreach(Collider2D col in cols)
			{
				if(col != this.collider2D)
				{
					Body body = col.GetComponent<Body>();
					if(	body != null && body.m_side == side && (composition == Composition.None || body.m_composition == composition) )
					{
						float dist = Vector2.Distance(col.transform.position, this.transform.position);
						if(dist < smallestDist)
						{
							smallestDist = dist;
							nearest = body;
						}
					}
				}
			}
			if(smallestDist != float.MaxValue)
			{
				return nearest;
			}
		}
		return null;
	}

	virtual protected void OnDrawGizmos()
	{
		// draw circle2D
		Gizmos.color = Color.blue;
		const int sections = 12;
		float radius = m_range;

		Vector3 center = this.transform.position;

		Vector3 prvPos = new Vector3(center.x + radius, center.y, center.z);
		for(float f = 0; f < 2 * Mathf.PI; f += (2 * Mathf.PI) / sections)
		{
			float x = center.x + radius * Mathf.Cos(f);
			float y = center.y - radius * Mathf.Sin(f);

			Vector3 newPos = new Vector3(x, y, center.z);
			Gizmos.DrawLine(prvPos, newPos);
			prvPos = newPos;
		}
	}
}
