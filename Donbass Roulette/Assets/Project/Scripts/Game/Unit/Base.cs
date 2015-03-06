﻿using UnityEngine;
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
        
        CrossSceneMenuInfo.use.isPlayerWinner = GameData.use.player.m_side != m_side;

        m_game.ai.StopAllCoroutines();
        m_game.player.StopAllCoroutines();

		if(m_side == Side.Right)
		{
            Destroy(m_game.ai.gameObject);
		}
		else if(m_side == Side.Left)
		{
            Destroy(m_game.player.gameObject);
		}

        MenuManager.use.Goto(MenuManager.MenuType.GAMEOVERMENU);
	}
	
}
