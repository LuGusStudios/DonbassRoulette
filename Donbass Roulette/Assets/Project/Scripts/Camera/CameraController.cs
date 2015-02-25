using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public LayerMask m_UILayer;

	public Map m_map;
	public float m_speed;

	public Collider2D m_arrowLeft;
	public Collider2D m_arrowRight;

    private void Start()
    {
        // Automatically create a parent container for this object so that camera shakes can act on that 
        // instead of the camera directly (which disables movement during the shake).

        Transform cameraParent = new GameObject("CameraContainer").transform;
        cameraParent.transform.parent = this.transform.parent;
        cameraParent.transform.localPosition = this.transform.localPosition;
        cameraParent.transform.localRotation = this.transform.localRotation;

        this.transform.parent = cameraParent;
    }

	private void MoveLeft()
	{
		if(this.transform.position.x - m_speed < m_map.m_minX)
			this.transform.position = new Vector3(m_map.m_minX, this.transform.position.y, this.transform.position.z);
		else
			this.transform.Translate(new Vector3(-m_speed, 0));
	}
	private void MoveRight()
	{
		if(this.transform.position.x + m_speed > m_map.m_maxX)
			this.transform.position = new Vector3(m_map.m_maxX, this.transform.position.y, this.transform.position.z);
		else
			this.transform.Translate(new Vector3(m_speed, 0));
	}



	// Update is called once per frame
	void Update () {
        if (LugusInput.use.Key(KeyCode.Mouse0))
        {
            Transform t = LugusInput.use.RayCastFromMouse(LugusCamera.game);
            if (t)
            {
                if (t.collider2D == this.m_arrowLeft)
                    MoveLeft();
                else if (t.collider2D == this.m_arrowRight)
                    MoveRight();
            }
        }
        else if (LugusInput.use.Key(KeyCode.LeftArrow))
            MoveLeft();
        else if (LugusInput.use.Key(KeyCode.RightArrow))
            MoveRight();
	}

	void Move(bool left)
	{
		//this.transform.Translate(
	}
}
