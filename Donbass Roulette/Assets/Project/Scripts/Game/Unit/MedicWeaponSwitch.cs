using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MedicWeaponSwitch : UnitWeaponSwitch 
{
    public Sprite healSprite = null;

    protected UnitMedic medic = null;

    public override void SetupLocal()
    {
        base.SetupLocal();

        medic = this.gameObject.FindComponentInParent<UnitMedic>();

        if (medic != null)
        {
            medic.onHeal += OnHeal;
        }
    }

    protected void OnHeal(Body body)
    {
        ChangeSprite(healSprite);
    }
}
