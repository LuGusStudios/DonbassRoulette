using UnityEngine;
using System.Collections;

public class LugusCamera 
{
	protected static Camera _gameCamera = null;
	public static Camera game
	{
		get
		{
			if( _gameCamera == null )
			{
				// First look for the camera we have explicitly called MainCamera.
				GameObject mainCam = GameObject.Find("MainCamera");
				
				if (mainCam != null)
				{
					_gameCamera = mainCam.GetComponentInChildren<Camera>();
				}
				
				// If that couldn't be retrieved, find the one Unity defaults to.
				if( _gameCamera == null )
				{
					_gameCamera =  Camera.main;
				}
				
				// If that also couldn't be retrieved, log error message.
				if (_gameCamera == null)
				{
					Debug.Log("LugusCamera: Missing main camera.");
				}
				
			}
			
			return _gameCamera;
		}
	}
	
	protected static Camera _uiCamera = null;
	public static Camera ui
	{
		get
		{
			if (_uiCamera == null)
			{
				// First look for the camera we have explicitly called UICamera.
				GameObject uiCamObject = GameObject.Find("UICamera");
				
				if (uiCamObject != null)
				{
					_uiCamera = uiCamObject.GetComponentInChildren<Camera>();
				}
				
				if (_uiCamera == null)
				{
					Debug.LogWarning("LugusCamera: Missing UI camera. Returning game camera instead from now on.");
					_uiCamera = game;
				}
			}
			
			return _uiCamera; 
		}
	}

	public static void SwitchMainTo(string targetCamName)
	{
		Camera[] allCameras = UnityEngine.GameObject.FindObjectsOfType<Camera>();
		
		foreach(Camera c in allCameras)
		{
			if (c.name == targetCamName)
			{
				SwitchMainTo(c);
				return;
			}
		}
		
		Debug.LogError("LugusCamera: No camera by name of " + targetCamName + " found.");
	}

	public static void SwitchMainTo(Camera targetCam)
	{
		Camera[] allCameras = UnityEngine.GameObject.FindObjectsOfType<Camera>();

		foreach(Camera c in allCameras)
		{
			if (c == targetCam)
			{
				if (c.tag != "MainCamera")
				{
					Debug.LogError("LuGusCamera: Target camera was not tagged MainCamera. Setting the tag now.");
					c.tag = "MainCamera";
				}

				c.enabled = true;
				c.gameObject.SetActive(true);
			}
			else
			{
				// By definition, Unity will only ever pick cameras with this tag as main camera.
				if (c.tag == "MainCamera")
				{
					c.enabled = false;
				}
			}
		}
	}
}

public static class LugusCameraExtensions
{
	public enum ShakeAmount
	{
		NONE,
		SMALL,
		MEDIUM,
		LARGE 
	}

	public static void Shake(this Camera camera, ShakeAmount amount, float time = 0.3f)
	{
		Vector3 displacement = Vector3.zero;
		if( amount == ShakeAmount.SMALL )	
			displacement =  new Vector3(0.1f, 0.0f, 0.05f);
		else if( amount == ShakeAmount.MEDIUM )
			displacement =  new Vector3(0.2f, 0.0f, 0.2f);
		else if( amount == ShakeAmount.LARGE )
			displacement =  new Vector3(0.5f, 0.0f, 0.5f);
			
		Shake (camera, displacement, time);	
	}

	public static void Shake(this Camera camera, Vector3 displacement, float time = 0.3f)
	{
		iTween.ShakePosition(camera.transform.parent.gameObject, displacement, time );		
	}
}
