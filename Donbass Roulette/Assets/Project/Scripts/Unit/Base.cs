using UnityEngine;
using System.Collections;

public class Base : Body {
	new void Start()
	{
		base.Start();
		m_DelDeath += End;
	}

	protected void End() {
		// TODO : this assume left = player, right = ai, modify it to increase modularity
		if(m_side == Side.Right)
		{
			Debug.Log("Win");
		}
		else if(m_side == Side.Left)
		{
			Debug.Log("Game Over");
		}
	}
	
}
