using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellMortarStrike : Spell 
{
    public GameObject mortarAmmoPrefab = null;
    public GameObject crosshairPrefab = null;
    public GameObject impactPrefab = null;

    protected GameObject currentProjectile = null;

    protected override void OnBegin(Vector2 position, Side side)
    {
        GameObject crossHair = Instantiate(crosshairPrefab) as GameObject;
        crossHair.transform.position = position;
        crossHair.transform.position = crossHair.transform.position.zAdd(Map.use.m_maxZ);
        Destroy(crossHair, instantiateDelay);

        GameObject mortarStrike = Instantiate(mortarAmmoPrefab) as GameObject;

        float xOffset = Random.Range(-10, -2);

        if (side == Side.Right)
            xOffset = Random.Range(2, 10);

        mortarStrike.transform.position = position.xAdd(xOffset).yAdd(16); // 16 = roughly screen height

        mortarStrike.LobTo(mortarStrike.transform.position, position, 2.0f).
            Time(instantiateDelay).
            OrientToPath(true).
            EaseType(iTween.EaseType.easeInQuart).
            Execute();

        Destroy(mortarStrike, instantiateDelay);
    }

    protected override void OnInstantiate(AreaOfEffect aoe)
    {
        GameObject impact = Instantiate(impactPrefab) as GameObject;
        impact.transform.position = aoe.transform.position.zAdd(Map.use.m_maxZ);
    }

}
