using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {
	public Vector2 m_offset;
	public Transform m_object;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = m_object.position + (Vector3) m_offset;
	}
}
