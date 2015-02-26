using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HpBar : ValueBar {
	public Body m_body;
    public SpriteRenderer[] m_sprites;

    void Start()
    {
        m_sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        SetSprites(false);
    }


	public override float GetRefValue()
	{
		return m_body.GetHpRatio();
	}

    protected override void OnValueChanged(float value)
    {
        if (value <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else if( value < 1 )
        {
            SetSprites(true);
        }
    }

    protected void SetSprites(bool active)
    {
        foreach(SpriteRenderer spr in m_sprites)
        {
            spr.enabled = active;
        }
    }

}
