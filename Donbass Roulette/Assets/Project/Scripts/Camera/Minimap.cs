using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minimap : MonoBehaviour {
	public List<Miniature> m_elements = new List<Miniature>();
	public float m_minX;
	public float m_maxX;
	public Map m_map;

	public float GetLength()
	{
		return (m_maxX - m_minX);
	}

	virtual protected void Update () {
		foreach(Miniature element in m_elements)
		{
            if (element && element.m_ref)
            {
                Vector3 realPos = ConvertToMinimap(element.m_ref.transform.position);
                element.transform.localPosition = realPos.yAdd(-realPos.y);
            }
		}
	}


	public Vector3 ConvertToMinimap(Vector3 pos)
	{
        Vector3 center = m_map.GetCenterGround();
        return ((pos - center) * GetScalingDifference());
	}
    public Vector3 ConvertToWorld(Vector3 pos)
    {
        Vector3 center = m_map.GetCenterGround();
        return ((pos - center) / GetScalingDifference());
    }


	protected float GetScalingDifference()
	{
		float realLength = m_map.GetLength();
		float minimapLength = GetLength();
		return (minimapLength / realLength);
	}


	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;

		const float sizeY = 5;

		Vector3 pos = this.transform.position;
		// Top-Left corner
		Vector3 TL = pos + new Vector3(m_minX, sizeY / 2f);
		// Top-Right corner
		Vector3 TR = pos + new Vector3(m_maxX, sizeY / 2f);
		// Bot-Left corner
		Vector3 BL = pos + new Vector3(m_minX, -sizeY / 2f);
		// Bot-Right corner
		Vector3 BR = pos + new Vector3(m_maxX, -sizeY / 2f);


		Gizmos.DrawLine(TL, TR);
		Gizmos.DrawLine(TR, BR);
		Gizmos.DrawLine(BR, BL);
		Gizmos.DrawLine(BL, TL);
	}

}
