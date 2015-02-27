using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour 
{
    protected Body body = null;
    public EffectManager.Effects hitType = EffectManager.Effects.None;
    public EffectManager.Effects hitType2 = EffectManager.Effects.None;

    void Start () 
    {
        body = gameObject.FindComponentInParent<Body>();

        if (body != null)
            body.m_onLoseHealth += OnLoseHealth;
	}

    void OnLoseHealth()
    {
        //Generate random number to no always get the same effect
        int index = Random.Range(0, 1);

        if (index == 0)
            EffectManager.use.SpawnEffect(hitType, body);
        else
            EffectManager.use.SpawnEffect(hitType2, body);
    }
}
