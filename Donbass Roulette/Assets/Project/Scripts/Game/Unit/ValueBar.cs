using UnityEngine;
using System.Collections;

public abstract class ValueBar : MonoBehaviour {
	public GameObject m_bar;

	public void UpdateValue(float value)
	{
		m_bar.transform.localScale = new Vector3(value, 1, 1);
	}

	public abstract float GetRefValue();

	void Update()
	{
		float curValue = GetRefValue();
		if(m_bar.transform.localScale.x != curValue)
		{
			UpdateValue(curValue);
		}
	}
}
