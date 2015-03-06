using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellMortarStrike : Spell 
{
    public GameObject mortarAmmoPrefab = null;
    public GameObject mortarAmmoBackgroundPrefab = null;

    public GameObject crosshairPrefab = null;
    public GameObject impactPrefab = null;
    public ParticleSystem impactBackgroundPrefab = null;

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

        // spawn a number of simpler mortar strikes in the background
        int randomBackgroundStrikes = Random.Range(2,4);

        for (int i = 0; i < randomBackgroundStrikes; i++)
        {
            ShowBackgroundMortars(position, side);
        }
    }

    protected void ShowBackgroundMortars(Vector2 position, Side side)
    {
        // all of this is... nasty...

        GameObject mortarStrike = Instantiate(mortarAmmoBackgroundPrefab) as GameObject;

        // give some idea of direction, but bigger range than with actual strike
        float xOffset = Random.Range(-30, 0);

        if (side == Side.Right)
            xOffset = Random.Range(0, 30);


        float depthFactor = Random.Range(0.05f, 1.0f);

        // background mortar strikes are always behind playing field
        // e.g. max depth + buffer (playing field is not at max depth) + variation (value 0-1 * eyeballed leveldepth)
        float depth = Map.use.GetDepthByPercentage(1) +  40 + depthFactor * 180;

        // further back, they get smaller (about 50 percent for max depth)
        mortarStrike.transform.localScale = mortarStrike.transform.localScale * (1.1f - depthFactor);
        mortarStrike.transform.position = position.xAdd(xOffset).yAdd(16); // 16 = roughly screen height
        mortarStrike.transform.position = mortarStrike.transform.position.z(depth);

        // randomize target position too, for random wanton destruction in background
       // Vector3 targetPosition = position.xAdd(Random.Range(-10, 10));

        Vector3 targetPosition = mortarStrike.transform.position.y(position.y);

        if (side == Side.Left)
            targetPosition = targetPosition.xAdd(Random.Range(5, 20));
        else
            targetPosition = targetPosition.xAdd(Random.Range(-5, -20));


        float randomTime = 0.5f + instantiateDelay + Random.value;

        mortarStrike.LobTo(mortarStrike.transform.position, targetPosition, 2.0f).
            Time(randomTime).
            OrientToPath(true).
            EaseType(iTween.EaseType.easeInQuart).
            Execute();

        Destroy(mortarStrike, randomTime);
  
        StartCoroutine(BackgroundFlashRemove(targetPosition, randomTime, depthFactor));
    }

    protected IEnumerator BackgroundFlashRemove(Vector3 position, float delay, float depthFactor)
    {
        yield return new WaitForSeconds(delay);

        ParticleSystem impact = Instantiate(impactBackgroundPrefab) as ParticleSystem;
        impact.transform.position = position.yAdd(5f * depthFactor);
        impact.Play();

        yield return new WaitForSeconds(3.0f);

        if (impact != null)
            Destroy(impact.gameObject);


        yield break;
    }

    protected override void OnInstantiate(AreaOfEffect aoe)
    {
        GameObject impact = Instantiate(impactPrefab) as GameObject;
        impact.transform.position = aoe.transform.position.z(Map.use.GetDepthByPercentage(1f));

        SoundManager.use.PlaySound(LugusAudio.use.SFX(), SoundManager.use.GetRandomExplosionSound());
    }

}
