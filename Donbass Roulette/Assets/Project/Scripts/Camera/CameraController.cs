using UnityEngine;
using System.Collections;

[RequireComponent(typeof(OrthographicCameraData))]
public class CameraController : LugusSingletonExisting<CameraController> {
	public LayerMask m_UILayer;
    public OrthographicCameraData m_cameraData;

	public Map m_map;
	public float m_arrowSpeed;
    public float m_dragSpeed;

	public Collider2D m_arrowLeft;
	public Collider2D m_arrowRight;

    protected Vector3 m_prvDragPos;
    protected bool m_uiEdit;

    public bool isIdleAnimating = true;
    public bool blockingInput = false;
    public float shakeFallOffPerMeter = 0.1f;


    protected bool movingToStartingPoint = false;
    protected Vector3 originalParentPosition = Vector3.zero;

    void Start()
    {
        m_cameraData = GetComponent<OrthographicCameraData>();
        // Automatically create a parent container for this object so that camera shakes can act on that 
        // instead of the camera directly (which disables movement during the shake).

        Transform cameraParent = new GameObject("CameraContainer").transform;
        cameraParent.transform.parent = this.transform.parent;
        cameraParent.transform.localPosition = this.transform.localPosition;
        cameraParent.transform.localRotation = this.transform.localRotation;

        this.transform.parent = cameraParent;

        originalParentPosition = cameraParent.transform.localPosition;
    }

    public void InitializeView()
    {
        if (GameData.use.player != null)
        {
            movingToStartingPoint = true;

            float sizeX = m_cameraData.GetSize().x;
            float xValue = GameData.use.player.m_spawner.position.x;

            xValue = Mathf.Clamp(xValue, m_map.m_minX + sizeX, m_map.m_maxX - sizeX); 

          // xValue = Mathf.Clamp(xValue, this.transform.position.x - m_map.m_minX + sizeX, this.transform.position.x + m_map.m_minX - sizeX);

            this.gameObject.MoveTo(this.transform.position.x(xValue)).Speed(15.0f).Execute();
        }
        else
        {
            Debug.LogError("CameraController: Player was null!");
        }
    }

    private void Move(float x)
    {
        this.transform.position = this.transform.position.xAdd(x);
        ClampToMap();
    }


    protected void ClampToMap()
    {
        float sizeX = m_cameraData.GetSize().x;
        this.transform.position = new Vector3(
            Mathf.Clamp(this.transform.position.x, m_map.m_minX + sizeX, m_map.m_maxX - sizeX), 
            this.transform.position.y, 
            this.transform.position.z);
    }

    public void ShakeCamera(Vector2 cameraShake)
    {
        // Camera shake diminshes as player is further from explosion.
        float distanceToCamera = Mathf.Abs(this.transform.position.x - LugusCamera.game.transform.position.x);

        // Shake camera parent so that camera itself can keep moving.
        if (LugusCamera.game.transform.parent != null)
        {
            iTween.ShakePosition(LugusCamera.game.transform.parent.gameObject,
            cameraShake * Mathf.Lerp(0.5f, 0.0f, distanceToCamera * shakeFallOffPerMeter),
            0.5f);
        }

        LugusCoroutines.use.StartRoutine(ReturnPositionAfterShake(0.5f), this.gameObject);
    }

    protected IEnumerator ReturnPositionAfterShake(float delay)
    { 
        yield return new WaitForSeconds(delay);

        if (LugusCamera.game.transform.parent != null)
        {
            LugusCamera.game.transform.parent.localPosition = originalParentPosition;
        }
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
                    ClampToMap();
                    return true;
                }
            
            }
        }
        return false;
    }

	void Update () {
        if (isIdleAnimating)
        {
            idleUpdate();
            return;
        }
        else if (blockingInput)
        {
            return;
        }

        if( LugusInput.use.down )
        {
            if (movingToStartingPoint)
            {
                movingToStartingPoint = false;
                iTween.Stop(gameObject);
            }

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
        }


        if (LugusInput.use.Key(KeyCode.LeftArrow))
        {
            if (movingToStartingPoint)
            {
                movingToStartingPoint = false;
                iTween.Stop(gameObject);
            }

            Move(-m_arrowSpeed);
        }
        else if (LugusInput.use.Key(KeyCode.RightArrow))
        {
            if (movingToStartingPoint)
            {
                movingToStartingPoint = false;
                iTween.Stop(gameObject);
            }

            Move(m_arrowSpeed);
        }
	}

    
    void idleUpdate()
    {
        float idleDist = 3;    
        float idleFreq = 0.3f;
        this.transform.position = new Vector3 (Mathf.Sin(Time.realtimeSinceStartup*idleFreq) * idleDist, transform.position.y, transform.position.z);        
    }
}
