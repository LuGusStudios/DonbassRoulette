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
        m_game.EndGame(m_side);        
	}
	
}
