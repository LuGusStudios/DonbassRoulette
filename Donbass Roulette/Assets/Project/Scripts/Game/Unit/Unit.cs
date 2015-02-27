using UnityEngine;
using System.Collections;

public class Unit : Entity {
	// TODO : implement horizontal collision (walking on slope)
	public float m_speed;

	public delegate void Delegate();
	public Delegate m_DelMove;


	public void Initialize( Side side )
	{
		m_side = side;
	}


	override protected void Update () {
        if (m_hp <= 0)
            return;
        
        base.Update();
        
		if(m_attackTimer <= 0)
		{
			Move();
		}
	}

	protected void Move()
	{
		if( m_DelMove != null )
			m_DelMove();

		int dir = 0;
		if(m_side == Side.Left)
			dir = 1;
		else if(m_side == Side.Right)
			dir = -1;
		else
			Debug.LogError("Unit has an unused Side variable");        

		this.transform.position = this.transform.position.xAdd(dir * m_speed * Time.deltaTime);
	}
}
