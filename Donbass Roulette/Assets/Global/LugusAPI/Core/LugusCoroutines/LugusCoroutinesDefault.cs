using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// we didn't use LugusSingletons here, since LugusCoroutinesDefault is not a MonoBehaviour but a POC
public class LugusCoroutines
{
	private static LugusCoroutinesDefault _instance = null;
	
	public static LugusCoroutinesDefault use 
	{ 
		get 
		{
			if ( _instance == null )
			{
				_instance = new LugusCoroutinesDefault();
			}
			
			
			return _instance; 
		}
	}
	
	public static void Change(LugusCoroutinesDefault newInstance)
	{
		_instance = newInstance;
	}	
}

[Serializable]
public class LugusCoroutinesDefault
{
	public List<ILugusCoroutineHandle> handles = new List<ILugusCoroutineHandle>();
	public int initialPoolCount = 8;
	
	protected Transform handleHelperParent = null;
	
	// This has no effect on this class, since it's not a Monobehaviour. Check LugusCoroutinesClearer for a lengthy explanation of how this is solved.
	//	protected void OnDestroy()
	//	{
	//		LugusCoroutines.Change(null);
	//	}

	
	public LugusCoroutinesDefault()
	{
		FindReferences();

		// First find any handles that already exist in the scene.
		handles.AddRange( (LugusCoroutineHandleDefault[]) UnityEngine.Object.FindObjectsOfType(typeof(LugusCoroutineHandleDefault)) );

		// Then create a couple already to have an initial pool.
		if (handles.Count < initialPoolCount)
		{
			while (handles.Count < initialPoolCount)
			{
				CreateHandle();
			}
		}
	}
	
	protected void FindReferences()
	{
		if( handleHelperParent == null )
		{
			GameObject p = GameObject.Find("_LugusCoroutines");
			if( p == null )
			{
				p = new GameObject("_LugusCoroutines");
			}
			
			handleHelperParent = p.transform;

			//Check LugusCoroutinesClearer for a lengthy explanation of what this does.
			p.AddComponent<LugusCoroutinesClearer>();
		}
	}
	
	protected ILugusCoroutineHandle CreateHandle(GameObject runner = null)
	{
		GameObject handleGO = runner;

		if( handleGO == null )
		{
			handleGO = new GameObject("LugusCoroutineHandle");
			handleGO.transform.parent = handleHelperParent;
		}

		ILugusCoroutineHandle handle = handleGO.AddComponent<LugusCoroutineHandleDefault>();

		handles.Add( handle );

		return handle;
	}
	
	public ILugusCoroutineHandle GetHandle(GameObject runner = null) 
	{
		if (runner != null)
		{
			return CreateHandle(runner); 
		}
		else
		{
			foreach(ILugusCoroutineHandle handle in handles)
			{
				if (!handle.Running)
					return handle;
			}
			
			return CreateHandle(runner); 
		}
	}
	
	public ILugusCoroutineHandle StartRoutine( IEnumerator routine, GameObject runner = null )
	{
		ILugusCoroutineHandle handle = GetHandle(runner);

		Coroutine coroutine = handle.StartRoutine( routine );
		handle.Coroutine = coroutine;
		
		return handle;
	}

	// This just provides an easier way to have one coroutine yield to another than by writing
	// "yield return LuGusCoroutines.use.StartRoutine(function).Coroutine". You always forget the last bit anyway...
	public Coroutine YieldToRoutine( IEnumerator routine, GameObject runner = null )
	{
		return StartRoutine(routine, runner).Coroutine;
	}
	
	public void StopAllRoutines()
	{
		foreach( ILugusCoroutineHandle handle in handles )
		{
			handle.StopRoutine ();
		}
	}
}
