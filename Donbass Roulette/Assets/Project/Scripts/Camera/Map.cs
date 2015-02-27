using UnityEngine;
using System.Collections;

public class Map : LugusSingletonExisting<Map> {
	public float m_minX;
	public float m_maxX;

    public float m_minY_ground;
    public float m_maxY_ground;
    public float m_minY_air;
    public float m_maxY_air;

    public float m_maxZ = 100;


	public float GetLength()
	{
		return (m_maxX - m_minX);
	}

    public Vector3 GetCenterGround()
    {
        return new Vector3((m_minX + m_maxX) / 2, (m_minY_ground + m_maxY_ground) / 2, 0);
    }

    public Vector3 GetCenterAir()
    {
        return new Vector3((m_minX + m_maxX) / 2, (m_minY_air+ m_maxY_air) / 2, 0);
    }

    public Vector3 GetRandomStartingGroundPos()
    {
        float randVal = Random.Range(m_minY_ground, m_maxY_ground);
        return (this.transform.position.yAdd(randVal).zAdd(randVal));
    }
    public Vector3 GetRandomStartingAirPos()
    {
        float randVal = Random.Range(m_minY_air, m_maxY_air);
        return (this.transform.position.yAdd(randVal).zAdd(randVal));
    }

    public bool IsPointOnGround(Vector2 point)
    {
        Rect newRect = new Rect(
            this.transform.position.x + m_minX, 
            this.transform.position.y + m_maxY_ground, 
            Mathf.Abs(m_minX) + Mathf.Abs(m_maxX),  
            Mathf.Abs(m_minY_ground) + Mathf.Abs(m_maxY_ground));

        return newRect.Contains(point);
    }


	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;

		Vector3 pos = this.transform.position;
		// Top-Left corner
		Vector3 TL = pos + new Vector3(m_minX, m_maxY_ground);
		// Top-Right corner
		Vector3 TR = pos + new Vector3(m_maxX, m_maxY_ground);
		// Bot-Left corner
		Vector3 BL = pos + new Vector3(m_minX, m_minY_ground);
		// Bot-Right corner
		Vector3 BR = pos + new Vector3(m_maxX, m_minY_ground);


		Gizmos.DrawLine(TL, TR);
		Gizmos.DrawLine(TR, BR);
		Gizmos.DrawLine(BR, BL);
		Gizmos.DrawLine(BL, TL);
	}
}
