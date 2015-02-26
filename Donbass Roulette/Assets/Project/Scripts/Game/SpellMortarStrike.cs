using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellMortarStrike : Spell 
{
    public GameObject mortarAmmoPrefab = null;
    protected GameObject currentProjectile = null;

    protected override void OnBegin(Vector2 position)
    {
        GameObject mortarStrike = Instantiate(mortarAmmoPrefab) as GameObject;

        mortarStrike.transform.position = position.xAdd(Random.Range(-8, 8)).yAdd(16); // 16 = roughly screen height

        mortarStrike.LobTo(mortarStrike.transform.position, position, 2.0f).
            Time(instantiateDelay).
            OrientToPath(true).
            EaseType(iTween.EaseType.easeInQuart).
            Execute();

        Destroy(mortarStrike, instantiateDelay);
    }

}
