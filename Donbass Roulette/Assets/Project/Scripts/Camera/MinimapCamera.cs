using UnityEngine;
using System.Collections;


public class MinimapCamera : Minimap {
	protected Minimap m_minimap;
	public Camera m_camera;
	public SpriteRenderer m_cameraLeft;
	public SpriteRenderer m_cameraRight;
	public SpriteRenderer m_cameraTop;
	public SpriteRenderer m_cameraBot;
	protected float m_cameraSize;

	void Awake()
	{
		m_minimap = GetComponent<Minimap>();
	}

	// Use this for initialization
	void Start () {
		m_cameraSize = m_camera.orthographicSize;

        // TODO : find the correct scale of the lines according to the dimension of the camera on the map
		m_cameraLeft.transform.localScale =  new Vector3(m_cameraLeft.transform.localScale.x, 100, m_cameraLeft.transform.localScale.z);
		m_cameraRight.transform.localScale = new Vector3(m_cameraRight.transform.localScale.x, 100, m_cameraRight.transform.localScale.z);

		m_cameraTop.transform.localScale = new Vector3(100, m_cameraTop.transform.localScale.y, m_cameraTop.transform.localScale.z);
		m_cameraBot.transform.localScale = new Vector3(100, m_cameraBot.transform.localScale.y, m_cameraBot.transform.localScale.z);
	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();
		m_cameraLeft.transform.localPosition = ConvertToMinimap(m_camera.transform.position.xAdd(-m_cameraSize));
		m_cameraRight.transform.localPosition = ConvertToMinimap(m_camera.transform.position.xAdd(m_cameraSize));
		m_cameraBot.transform.localPosition = ConvertToMinimap(m_camera.transform.position.yAdd(-m_cameraSize));
		m_cameraTop.transform.localPosition = ConvertToMinimap(m_camera.transform.position.yAdd(m_cameraSize));
		
	}
}
