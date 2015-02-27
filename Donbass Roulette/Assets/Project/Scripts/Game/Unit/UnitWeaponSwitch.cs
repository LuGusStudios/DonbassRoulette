using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitWeaponSwitch : MonoBehaviour 
{
    protected SpriteRenderer attachedRenderer = null;
    protected Unit unit = null;

    public Sprite attackSprite = null;

	public virtual void SetupLocal()
	{
        attachedRenderer = this.gameObject.FindComponent<SpriteRenderer>();

        unit = this.gameObject.FindComponentInParent<Unit>();

        if (unit != null)
        {
            unit.m_delAttack += OnAttack;
        }
	}
	
	public virtual void SetupGlobal()
	{
		// lookup references to objects / scripts outside of this script
	}
	
	protected void Awake()
	{
		SetupLocal();
	}

	protected void Start() 
	{
		SetupGlobal();
	}

    protected void ChangeSprite(Sprite newSprite)
    {
        attachedRenderer.sprite = newSprite;
    }

    protected virtual void OnAttack()
    {
        ChangeSprite(attackSprite);
    }

}
