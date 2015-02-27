using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager : LugusSingletonExisting<EffectManager>
{
    public List<GameObject> EffectsList = null;

    public enum Effects
    {
        None = 0,
        Blood_a = 1,
        Blood_b = 2,
        Blood_c = 3,
        Blood_d = 4,

        Blood_drip = 5,
        Blood_drop = 6,
        Blood_splash = 7,
        Blood_splatter = 8,
        Blood_spurt = 9,

        Flash_1 = 10,
        Flash_2 = 11,
        Flash_3 = 12,
        Flash_4 = 13,

        Muzzle_front = 14,
        Muzzle_profile = 15
    }


    public void SpawnEffect(Effects type, Body body)
    {
        foreach (GameObject effect in EffectsList)
        {
            // TODO The sprites should be mirrored for the units coming from the left, but xMul(-1) is being a whiny little cunt nozzle atm
            if (type.ToString().ToLower() == effect.name.ToLower())
            {
                if (body.m_side == Side.Left)
                {
                    GameObject myEffect = GameObject.Instantiate(effect, body.transform.position.zAdd(-0.3f).xAdd(-.5f), Quaternion.identity) as GameObject;
                    StartCoroutine(DestroyEffect(myEffect));
                    return;
                }
                else
                {
                    GameObject myEffect = GameObject.Instantiate(effect, body.transform.position.zAdd(0.3f).xAdd(-.5f), Quaternion.identity) as GameObject;
                    StartCoroutine(DestroyEffect(myEffect));
                    return;
                }
            }
        }
        Debug.LogWarning("Can't find " + type.ToString() + " in the EffectsList");
    }

    IEnumerator DestroyEffect(GameObject effect)
    {
        yield return new WaitForSeconds(.5f);
        Destroy(effect);
    }
}
