using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellMortarStrike : Spell 
{
    public GameObject mortarAmmoPrefab = null;
    public GameObject crosshairPrefab = null;

    protected GameObject currentProjectile = null;

    protected override void OnBegin(Vector2 position, Side side)
    {
        GameObject crossHair = Instantiate(crosshairPrefab) as GameObject;
        crossHair.transform.position = position;
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

}
