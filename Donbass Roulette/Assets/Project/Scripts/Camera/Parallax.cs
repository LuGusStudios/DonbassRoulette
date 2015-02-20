using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class Parallax : MonoBehaviour {
	public List<ScrollBackground> m_scrollBgs = new List<ScrollBackground>();
	private float m_prvPos;




	void Start () {
		m_prvPos = this.transform.position.x;
	}
	
	void Update () {
		float curPos = this.transform.position.x;

		float diff = curPos - m_prvPos;
		if(diff != 0)
		{
			foreach(ScrollBackground scrollBg in m_scrollBgs)
			{
				scrollBg.Move(-diff);
			}
		}

		m_prvPos = curPos;
	}


}
