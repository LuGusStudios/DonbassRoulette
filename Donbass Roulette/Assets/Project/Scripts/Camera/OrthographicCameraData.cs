using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Camera))]
public class OrthographicCameraData : MonoBehaviour {
    protected Camera m_camera;
    protected Vector2 m_cameraSize;

	// Use this for initialization
	void Start () {
        m_camera = GetComponent<Camera>();
        m_cameraSize = m_camera.transform.position - m_camera.ViewportToWorldPoint(new Vector3(0, 0.5f, m_camera.nearClipPlane));
    }
	
    public Camera GetObject()
    {
        return m_camera;
    }

    public Vector2 GetSize()
    {
        return m_cameraSize;
    }
}
