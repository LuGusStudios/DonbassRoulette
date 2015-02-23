// comment this in games that don't use any of the unity3d 4.3 2D features (Physics2D raycasts basically)
#define Physics2D

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LugusInput : LugusSingletonRuntime<LugusInputDefault>
{

}

public class LugusInputDefault : LugusSingletonRuntime<LugusInputDefault>
{
	protected bool acceptInput = true;
	
	public bool dragging = false;
	public bool down = false;
	public bool up = false;

	public bool mouseMoving
	{
		get
		{
			if( dragging )
				return true;
			else
			{
				if( dragPoints.Count < 2 )
					return false;
				else
				{
					Vector3 previousPoint = dragPoints[ dragPoints.Count - 2 ];
					if( Vector3.Distance(currentPosition, previousPoint) > 2 )
						return true;
					else
						return false;
				}
			}
		}
	}
	
	public List<Vector3> dragPoints = new List<Vector3>();
	public Vector3 currentPosition;

	// See RaycastFromAllTouches()
	protected class TouchRayCastCollection
	{
		public Camera originCamera = null;
		public int lastUpdated = -1;
		public List<Transform> hits = new List<Transform>();
	}
	
	protected List<TouchRayCastCollection> touchRayCastCollections = new List<TouchRayCastCollection>();
	protected int mainTouchID = -1;
    
	//UPDATE AND DOWN/UP EVENTS	=====================================================================================================================================
	protected void Update () 
	{
		ProcessInput();
		
		if( Input.GetKeyDown(KeyCode.Tab) )
		{
			LugusDebug.debug = !LugusDebug.debug;
		}
	}
	
	protected void ProcessInput()
	{
		if( !acceptInput )
			return;
		
		down = false;
		up = false;
		
		// code for a single touch (mouse-like behaviour on touch)

		if (Input.touchCount > 0)
		{
			ReadTouches();
		}
		else
		{
			ReadMouse();
		}
	}	

	protected void ReadMouse()
	{
		currentPosition = Input.mousePosition;

		if (Input.GetMouseButtonDown(0)) // 0 = left click
		{
			down = true;
			dragging = true;
			OnMouseDown();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			up = true;
			dragging = false;
			OnMouseUp();
		}

		if (dragging)
			OnMouseDrag();
	}
	
	protected void ReadTouches()
	{
		if (Input.touchCount < 1)
		{
			Debug.LogError("LugusInput: ReadTouches is being called without touches!");
			return;
		}
            
        // By default, use the first touch.
		Touch currentMainTouch = Input.touches[0];

		// Try to retrieve the same touch as last frame. Failing that, the first touch in the last remains default.
		if (Input.touchCount > 1)
		{
			for (int i = 0; i < Input.touchCount; i++) 
			{
				Touch touch = Input.GetTouch(i);

				if (touch.fingerId == mainTouchID)
				{
					currentMainTouch = touch;
					break;
				}
	    	}
		}
     
        mainTouchID = currentMainTouch.fingerId;

		currentPosition = currentMainTouch.position;;

		if (currentMainTouch.phase == TouchPhase.Began)
		{
			// Down event will only be called for the main touch. Main finger will obviously change
			// if the first finger is released and another is still pressed down, but that touch
			// will no longer be in TouchPhase.Began state by that time.
            down = true;
			dragging = true;
		
			dragPoints.Clear(); // reset the inputPoints array

			dragPoints.Add( currentPosition );
		}
		else if( currentMainTouch.phase == TouchPhase.Ended )
		{
			// Only call up event when this is the last finger released.
			if (Input.touchCount <= 1)
			{
                up = true;
				dragging = false;
			}

			dragPoints.Add( currentPosition );
		}

		if ( currentMainTouch.deltaPosition != Vector2.zero ) 
		{
			// Limit length of drag points
			if( dragPoints.Count > 1000 )
				dragPoints.Clear();

			dragPoints.Add( currentPosition );
		}
	}
	
	protected void OnMouseDown()
	{	
		dragPoints.Clear(); // reset the inputPoints array
		dragPoints.Add( currentPosition );
	}

	protected void OnMouseUp()
	{
		dragPoints.Add( currentPosition );
	}

	protected void OnMouseDrag()
	{
		// Limit length of drag points
		if( dragPoints.Count > 1000 )
			dragPoints.Clear();
		
		dragPoints.Add( currentPosition );
	}
		


	//RAYCAST METHODS	=====================================================================================================================================

	public Transform RayCastFromMouse()
	{
		return RayCastFromMouse(LugusCamera.ui);
	}

	public Transform RayCastFromMouse(Camera camera)
	{
		return RaycastFromScreenPoint( camera, currentPosition );
	}
	
	public Transform RaycastFromScreenPoint(Vector3 screenPoint)
	{
		return RaycastFromScreenPoint(LugusCamera.ui, screenPoint);
	}
	
	public Transform RaycastFromScreenPoint(Camera camera, Vector3 screenPoint)
	{
		Ray ray = camera.ScreenPointToRay( screenPoint );
		RaycastHit hit;
		if ( Physics.Raycast(ray, out hit) )
		{
			return hit.collider.transform;
		}
		#if Physics2D
		else
		{
			RaycastHit2D hit2 = Physics2D.Raycast( camera.ScreenToWorldPoint(screenPoint), Vector2.zero );
			if( hit2.collider != null )
			{
				return hit2.collider.transform; 
			}
		}
		#endif
		
		return null;
	}
	
	public Vector3 ScreenTo3DPoint( Transform referenceObject )
	{
		return ScreenTo3DPoint( Input.mousePosition, referenceObject );
	}
	
	public Vector3 ScreenTo3DPoint( Vector3 screenPoint, Transform referenceObject )
	{
		return ScreenTo3DPoint( screenPoint, referenceObject.position );
	}
	
	public Vector3 ScreenTo3DPoint( Vector3 screenPoint, Vector3 referencePosition )
	{
		Ray ray = LugusCamera.ui.ScreenPointToRay( screenPoint );
		Vector3 output = ray.GetPoint( Vector3.Distance(referencePosition, LugusCamera.ui.transform.position) );
		return output.z(referencePosition.z);
	}
	
	public Vector3 ScreenTo3DPoint( Vector3 screenPoint, Vector3 referencePosition, Camera camera )
	{
		Ray ray = camera.ScreenPointToRay( screenPoint );
		Vector3 output = ray.GetPoint( Vector3.Distance(referencePosition, camera.transform.position) );
		return output.z(referencePosition.z);
	}
	
	public Vector3 ScreenTo3DPointOnPlane( Vector3 screenPoint, Plane plane)
	{
		float distance;
		Ray ray = LugusCamera.ui.ScreenPointToRay( screenPoint );
		
		if( plane.Raycast(ray, out distance) )
		{
			return ray.GetPoint(distance);
		}
		
		return Vector3.zero;
	}
	
	public Transform RayCastFromMouseDown()
	{
		if( down )
			return RayCastFromMouse();
		else
			return null;
	}
	
	public Transform RayCastFromMouseUp()
	{
		if( up )
			return RayCastFromMouse();
		else
			return null;
	}
	
	public Transform RayCastFromMouseDown(Camera camera)
	{
		if( down )
			return RayCastFromMouse(camera);
		else
			return null;
	}
	
	public Transform RayCastFromMouseUp(Camera camera)
	{
		if( up )
			return RayCastFromMouse(camera);
		else
			return null;
	}
	
	public List<Transform> RayCastFromAllTouchesDown()
	{
		return RayCastFromAllTouchesDown(LugusCamera.ui);
	}
	
	public List<Transform> RayCastFromAllTouchesDown(Camera targetCamera)
	{
		if (down)
			return RayCastFromAllTouches(targetCamera);
		else
			return new List<Transform>();
	}
	
	public List<Transform> RayCastFromAllTouchesUp()
	{
		return RayCastFromAllTouchesDown(LugusCamera.ui);
	}
	
	public List<Transform> RayCastFromAllTouchesUp(Camera targetCamera)
	{
		if (up)
			return RayCastFromAllTouches(targetCamera);
		else
			return new List<Transform>();
	}
	
	public List<Transform> RayCastFromAllTouches()
	{
		return RayCastFromAllTouches(LugusCamera.ui);
	}
	
	// Returns a list that contains all transforms under all touches.
	// For performance reasons, this list is cached once per frame for every requested camera.
	// To see if an object is under any of multiple touches, use RayCastFromAllTouches(myCamera).Contains(myTransform).
	public List<Transform> RayCastFromAllTouches(Camera targetCamera)
	{
		if (targetCamera == null)
		{
			Debug.LogError("LugusInput: Target camera was null!");
			return new List<Transform>();
		}
		
		TouchRayCastCollection requestedCollection = null;
		
		foreach (TouchRayCastCollection collection in touchRayCastCollections)
		{
			if (collection.originCamera == targetCamera)
			{
                requestedCollection = collection;
                
                if (requestedCollection.lastUpdated == Time.frameCount)
                {
                    return requestedCollection.hits;
                }
                
                break;
            }
        }
        
        if (requestedCollection == null)
        {
            TouchRayCastCollection newCollection = new TouchRayCastCollection();
            newCollection.originCamera = targetCamera;
            
            touchRayCastCollections.Add(newCollection);
            requestedCollection = newCollection;
        }
        
        requestedCollection.lastUpdated = Time.frameCount;
        requestedCollection.hits.Clear();
        
        for (int i = 0; i < Input.touchCount; i++) 
        {
            Transform t = RaycastFromScreenPoint(requestedCollection.originCamera, Input.GetTouch(i).position);
            
            if (t != null)
                requestedCollection.hits.Add(t);
        }
        
        return requestedCollection.hits;
    }
    
    
    
    
	//KEY INPUT	=====================================================================================================================================
    public bool KeyDown(KeyCode key)
	{
		return Input.GetKeyDown(key); 
	}
	
	public bool KeyUp(KeyCode key)
	{
		return Input.GetKeyUp(key);
	}
	
	public bool Key(KeyCode key)
	{
		return Input.GetKey(key);
	}
	


	void OnGUI()
	{	
		if( !LugusDebug.debug )
			return;
		
		if( GUI.Button( new Rect(200, Screen.height - 30, 200, 30), "Toggle debug") )
		{
			LugusDebug.debug = !LugusDebug.debug;
		}
	}
}
