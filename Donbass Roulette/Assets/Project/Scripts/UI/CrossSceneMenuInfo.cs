using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrossSceneMenuInfo : LugusSingletonCrossScene<CrossSceneMenuInfo> {

    public MenuManager.MenuType nextMenuOnReload = MenuManager.MenuType.MAINMENU;
    public bool isPlayerWinner = false;
    public float lvlDuration = 0;
    public Dictionary<string, int> unitsSpawned = new Dictionary<string, int>();

    public void AddUnitToDict(Factory f)
    {
        if (unitsSpawned.Count == 0)
        {
            resetDict();
        }

        string unitName = f.m_prefabUnit.name;

        if (GameData.use.player.m_side == Side.Left)
        {
            unitName = unitName.Replace("Rebel", "Player");
            unitName = unitName.Replace("Ukraine", "AI");
        }
        else
        {
            unitName = unitName.Replace("Rebel", "AI");
            unitName = unitName.Replace("Ukraine", "Player");
        }

        if (unitsSpawned.ContainsKey(unitName))
            unitsSpawned[unitName] += 1;       
        else
            unitsSpawned.Add(unitName, 1);        
    }

    public void resetDict()
    {
        string pMedic = "PlayerMedic";
        string pRPG = "PlayerRPG";
        string pSoldier = "PlayerSoldier";
        string pTank = "PlayerTank";

        string aMedic = "AIMedic";
        string aRPG = "AIRPG";
        string aSoldier = "AISoldier";
        string aTank = "AITank";

        string[] keys = new string[] {
            pMedic, pRPG, pSoldier, pTank, 
            aMedic, aRPG, aSoldier, aTank};

        unitsSpawned = new Dictionary<string,int>();

        for (int i = 0; i < keys.Length; i++)
        {
            unitsSpawned.Add(keys[i], 0);
        }
    }

    // Use this for initialization
    void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
