using UnityEngine;
using System.Collections;

public class Miniature : MonoBehaviour {
	public GameObject m_ref;
	public Minimap m_minimap;


	void Awake()
	{
		m_minimap = GameObject.FindObjectOfType<Minimap>();
		if(m_minimap)
		{
			this.transform.parent = m_minimap.transform;
			m_minimap.m_elements.Add(this);
		}
		else
		{
			Debug.LogError("No Minimap detected although a Miniature exist");
		}
	}


	void Update()
	{
		if(m_ref == null)
		{
			m_minimap.m_elements.Remove(this);
			Destroy(this.gameObject);
		}

	}









}
