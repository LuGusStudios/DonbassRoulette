using UnityEngine;
using System.Collections;

[RequireComponent(typeof(OrthographicCameraData))]
public class CameraController : MonoBehaviour {
	public LayerMask m_UILayer;
    public OrthographicCameraData m_cameraData;

	public Map m_map;
	public float m_arrowSpeed;
    public float m_dragSpeed;

	public Collider2D m_arrowLeft;
	public Collider2D m_arrowRight;

    protected Vector3 m_prvDragPos;
    protected bool m_uiEdit;


    void Start()
    {
        m_cameraData = GetComponent<OrthographicCameraData>();
    }



    private void Move(float x)
    {
        this.transform.position = this.transform.position.xAdd(x);
    }


    protected void ClampToMap()
    {
        float sizeX = m_cameraData.GetSize().x;
        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, m_map.m_minX + sizeX, m_map.m_maxX - sizeX), this.transform.position.y, this.transform.position.z);
    }

    protected bool MoveWithUI()
    {
        Camera camera = m_cameraData.GetObject();
        Transform t = LugusInput.use.RayCastFromMouse(camera);
        if (t)
        {
            if (t.collider2D == this.m_arrowLeft)
            {
                Move(-m_arrowSpeed);
                return true;
            }
            else if (t.collider2D == this.m_arrowRight)
            {
                Move(m_arrowSpeed);
                return true;
            }
            else
            {
                MinimapCamera minimap = t.GetComponent<MinimapCamera>();
                if(minimap)
                {
                    Vector3 hit = camera.ScreenToWorldPoint(LugusInput.use.currentPosition);
                    Vector3 correctPos = hit - minimap.transform.position;
                    camera.transform.position = new Vector3(minimap.ConvertToWorld(correctPos).x, camera.transform.position.y, camera.transform.position.z);
                    return true;
                }
            
            }
        }
        return false;
    }

	void Update () {
        if( LugusInput.use.down )
        {
            m_uiEdit = MoveWithUI();
            m_prvDragPos = LugusInput.use.currentPosition;
        }
        else if (LugusInput.use.dragging)
        {
            if (m_uiEdit)
            {
                MoveWithUI();
            }
            else
            {
                Vector3 curDragPos = LugusInput.use.currentPosition;
                Move((m_prvDragPos.x - curDragPos.x) * m_dragSpeed);
                m_prvDragPos = curDragPos;
            }
            ClampToMap();
        }


        if (LugusInput.use.Key(KeyCode.LeftArrow))
        {
            Move(-m_arrowSpeed);
            ClampToMap();
        }
        else if (LugusInput.use.Key(KeyCode.RightArrow))
        {
            Move(m_arrowSpeed);
            ClampToMap();
        }
	}
}
