using UnityEngine;
using System.Collections;

public class Base : Body {
    public GameData m_game;
	override protected void Start()
	{
		base.Start();
		m_delDeath += End;
        m_game = GameObject.FindObjectOfType<GameData>();
        if (!m_game)
            Debug.LogError("No Game existing !");
	}

	protected void End() {
		// TODO : this assume left = player, right = ai, modify it to increase modularity
		if(m_side == Side.Right)
		{
            Destroy(m_game.m_rightUser.gameObject);
		}
		else if(m_side == Side.Left)
		{
            Destroy(m_game.m_leftUser.gameObject);
		}
	}
	
}
