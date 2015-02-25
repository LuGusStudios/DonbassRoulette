using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitAnimMedic : UnitAnim 
{
    public string healAnimName = "Heal";

    protected UnitMedic medic = null;

    protected override void Awake()
    {
        base.Awake();

        medic = this.gameObject.FindComponentInParent<UnitMedic>(true);
    }

    protected override void Start()
    {
        base.Start();

        if (medic != null)
            medic.onHeal += OnHeal;
    }

    protected void OnHeal(Body target)
    {
        m_animator.Play(healAnimName);
    }
}
