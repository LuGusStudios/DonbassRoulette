using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Sprite))]
public class ScrollBackground : MonoBehaviour
{
	public List<Background> m_bgs = new List<Background>();
	public float m_speed;

	public void Move(float dir)
	{
		foreach(Background bg in m_bgs)
		{
			bg.Move(dir * m_speed);
		}
	}
}