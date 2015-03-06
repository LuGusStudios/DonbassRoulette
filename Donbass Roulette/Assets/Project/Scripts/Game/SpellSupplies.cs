using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellSupplies : Spell 
{
    public GameObject supplyPackagePrefab = null;
    public GameObject crosshairPrefab = null;


    protected GameObject currentProjectile = null;

    protected override void OnBegin(Vector2 targetPosition, Side side)
    {
        GameObject crossHair = Instantiate(crosshairPrefab) as GameObject;
        crossHair.transform.position = targetPosition;
        crossHair.transform.position = crossHair.transform.position.zAdd(Map.use.m_maxZ);
        Destroy(crossHair, instantiateDelay);

        GameObject supplyPackage = Instantiate(supplyPackagePrefab) as GameObject;

        float xOffset = Random.Range(-10, -2);

        if (side == Side.Right)
            xOffset = Random.Range(2, 10);

        supplyPackage.transform.position = targetPosition.yAdd(16); // 16 = roughly screen height
        supplyPackage.transform.position = supplyPackage.transform.position.zAdd(Map.use.GetDepthByPercentage(0.5f));

        List<Vector3> path = new List<Vector3>();

        Vector3 start = supplyPackage.transform.position;
        Vector3 end = targetPosition.ToVector3().zAdd((Map.use.GetDepthByPercentage(0.5f)));


        path.Add(supplyPackage.transform.position);

        //for (int i = 1; i <= 3; i++)
        //{
        //    Vector3 point = Vector3.Lerp(start, targetPosition, (float)i / 5.0f);
        //    point = point.xAdd(Random.Range(-3.0f, 3.0f));
        //    path.Add(point);
        //}

        path.Add(Vector3.Lerp(start, end, 0.5f).xAdd(Random.Range(-1f, 1f) * 6));

        path.Add(end);

        supplyPackage.MoveTo(path.ToArray()).
            Time(instantiateDelay).
            EaseType(iTween.EaseType.linear).
            Execute();

        StartCoroutine(RemoveParachute(supplyPackage));
    }

    protected IEnumerator RemoveParachute(GameObject supplyPackage)
    {
        yield return new WaitForSeconds(instantiateDelay);

        Transform parachute = supplyPackage.transform.FindChild("Parachute");
        parachute.gameObject.SetActive(false);

        yield return new WaitForSeconds(2.0f);

        Destroy(supplyPackage, instantiateDelay);
    }
   



    protected override void OnInstantiate(AreaOfEffect aoe)
    {
        //GameObject impact = Instantiate(impactPrefab) as GameObject;
        //impact.transform.position = aoe.transform.position.z(Map.use.GetDepthByPercentage(1f));

        //SoundManager.use.PlaySound(LugusAudio.use.SFX(), SoundManager.use.GetRandomExplosionSound());
    }

}
