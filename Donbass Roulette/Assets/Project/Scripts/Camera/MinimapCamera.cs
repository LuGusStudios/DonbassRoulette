using UnityEngine;
using System.Collections;


public class MinimapCamera : Minimap {
	protected Minimap m_minimap;
	public OrthographicCameraData m_cameraData;
	public SpriteRenderer m_cameraLeft;
	public SpriteRenderer m_cameraRight;
	public SpriteRenderer m_cameraTop;
	public SpriteRenderer m_cameraBot;
   

	void Awake()
	{
		m_minimap = GetComponent<Minimap>();
	}

	// Use this for initialization
	void Start () {

        Vector2 cameraSize = new Vector2(m_cameraData.GetSize().x, 5);
        // TODO : find the correct scale of the lines according to the dimension of the camera on the map
        // the square ratio only work with 5 right now
        // camera size is supposed to be part of the calcul for the scaling !!!?
        m_cameraLeft.transform.localScale = new Vector3(m_cameraLeft.transform.localScale.x, GetScalingDifference() * 1000, m_cameraLeft.transform.localScale.z);
        m_cameraRight.transform.localScale = new Vector3(m_cameraRight.transform.localScale.x, GetScalingDifference() * 1000, m_cameraRight.transform.localScale.z);
        float ratio = (cameraSize.x / cameraSize.y);
        m_cameraTop.transform.localScale = new Vector3(GetScalingDifference() * ratio * 1000, m_cameraTop.transform.localScale.y, m_cameraTop.transform.localScale.z);
        m_cameraBot.transform.localScale = new Vector3(GetScalingDifference() * ratio * 1000, m_cameraBot.transform.localScale.y, m_cameraBot.transform.localScale.z);        

	}
	
	// Update is called once per frame
	override protected void Update () {
		base.Update();

        Vector2 cameraSize = new Vector2(m_cameraData.GetSize().x, 5);
        m_cameraLeft.transform.localPosition = ConvertToMinimap(m_cameraData.transform.position.xAdd(-cameraSize.x));
		m_cameraRight.transform.localPosition = ConvertToMinimap(m_cameraData.transform.position.xAdd(cameraSize.x));
		m_cameraBot.transform.localPosition = ConvertToMinimap(m_cameraData.transform.position.yAdd(-cameraSize.y));
		m_cameraTop.transform.localPosition = ConvertToMinimap(m_cameraData.transform.position.yAdd(cameraSize.y));
	}
}
