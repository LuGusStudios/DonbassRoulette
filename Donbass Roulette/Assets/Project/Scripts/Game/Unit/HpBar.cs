using UnityEngine;
using System.Collections;

public class HpBar : ValueBar {
	public Body m_body;

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
    }
}
