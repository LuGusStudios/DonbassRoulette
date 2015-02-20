using UnityEngine;
using System.Collections;

// A singleton that needs to exist in the scene and cannot be created at runtime.
public class LugusSingletonExisting<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance = null;
	public static T use
	{
		get
		{
			CacheInstance();
			
			return _instance;
		}
	}
	
	protected static void CacheInstance()
	{
		if( _instance != null )
			return ;
		
		T[] instances = (T[]) GameObject.FindObjectsOfType( typeof(T) );
		
		if( instances.Length == 0 )
		{
			Debug.LogError("No " + typeof(T).Name + " found in this scene.");
			return;
		}
		else if( instances.Length > 1 )
		{
			Debug.LogError("Multiple (" + instances.Length + ") instances of object " + typeof(T).Name + " found in this scene. Returning the first.");
		}
		
		_instance = instances[0]; 

		if (_instance != null)
		{
			LugusSingletonExisting<T> singleton = _instance as LugusSingletonExisting<T>;
			
			// The as keyword will return if the instance does not directly inherit from LugusSingletonExisting (e.g; for a strategy design pattern such as in LugusResources).
			// The InitializeSingleton method can still be made to function by using the following inheritance pattern:
			// FunctionalityClass : LugusSingletonExisting<FunctionalityClassImplementationOne>
			// FunctionalityClassImplementationOne : LugusSingletonExisting<FunctionalityClassImplementationOne>
			// This way, the class containing actual functionality is also a singleton, but its use value should normally point to the same object.
			
			if (singleton != null)
				singleton.InitializeSingleton();
		}
	}

	// This method will be called upon the first retrieval of the singleton. Override to use.
	public virtual void InitializeSingleton()
	{
	}
	
	public static void Change(T newInstance)
	{
		_instance = newInstance;
	}
	
	protected void OnDisable()
	{
		_instance = null;
	}

	protected void OnDestroy()
	{
		this.enabled = false;
		_instance = null;
	}
	
	public static bool Exists()
	{
		if( _instance != null )
			return true;
		
		
		T[] instances = (T[]) GameObject.FindObjectsOfType( typeof(T) );
		return instances.Length != 0;
	}
}

// A singleton that can simply be created at runtime and doesn't need to exist earlier.
public class LugusSingletonRuntime<T> : MonoBehaviour where T : MonoBehaviour
{
	private static string containerName = "_SINGLETONCONTAINER";
	private static T[] instances = null;
	private static T _instance = null;
	public static T use
	{
		get
		{
			CacheInstance();
			
			return _instance;
		}
	}
	
	protected static void CacheInstance()
	{
		if( _instance != null )
			return;
		
		instances = (T[]) GameObject.FindObjectsOfType( typeof(T) );
		
		if( instances.Length == 0 )
		{
			Debug.Log("No " + typeof(T).Name + " found in this scene. Creating it.");
			
			GameObject scriptContainer = GameObject.Find(containerName);
			if( scriptContainer == null )
			{
				scriptContainer = new GameObject(containerName); 
			}
			
			_instance = scriptContainer.AddComponent<T>();
		}
		else if( instances.Length > 1 )
		{
			Debug.LogError("Multiple (" + instances.Length + ") instances of object " + typeof(T).Name + " found in this scene. Returning the first.");
			_instance = instances[0];
		}
		else
		{
			_instance = instances[0];
		}
		
		if (_instance != null)
		{
			LugusSingletonRuntime<T> singleton = _instance as LugusSingletonRuntime<T>;
			
			// The as keyword will return if the instance does not directly inherit from LugusSingletonRuntime (e.g; for a strategy design pattern such as in LugusInput).
			// The InitializeSingleton method can still be made to function by using the following inheritance pattern:
			// FunctionalityClass : LugusSingletonRuntime<FunctionalityClassImplementationOne>
			// FunctionalityClassImplementationOne : LugusSingletonRuntime<FunctionalityClassImplementationOne>
			// This way, the class containing actual functionality is also a singleton, but its use value should normally point to the same object.
			
			if (singleton != null)
				singleton.InitializeSingleton();
		}
	}

	// This method will be called upon the first retrieval of the singleton. Override to use.
	public virtual void InitializeSingleton()
	{
	}

	public static void Change(T newInstance)
	{
		_instance = newInstance;
	}
	
	protected void OnDisable()
	{
		_instance = null;
	}

	protected void OnDestroy()
	{
		this.enabled = false;
		_instance = null;
	}
}

// Will find the singleton in the scene everytime. Only use in very specific cases. Potentially quite unperformant.
public class SingletonVolatile<T> : MonoBehaviour where T : MonoBehaviour 
{
	public static T use 
	{ 
		get
		{
			return (T) GameObject.FindObjectOfType( typeof(T) ); 
		}
	}
}

// A singleton that is created when first needed and will persist across scenes from then on. Handy for cross-scene data (saves etc.)
public class LugusSingletonCrossScene<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T[] instances = null;
	private static T _instance = null;
	public static T use
	{
		get
		{
			CacheInstance();
			
			return _instance;
		}
	}
	
	protected static void CacheInstance()
	{
		if( _instance != null )
			return;
		
		instances = (T[]) GameObject.FindObjectsOfType( typeof(T) );
		
		if( instances.Length == 0 )
		{
			Debug.Log("No " + typeof(T).Name + " found in this scene. Creating it.");
			
			GameObject scriptContainer = new GameObject(typeof(T).Name); 
			
			_instance = scriptContainer.AddComponent<T>();
			
			DontDestroyOnLoad(scriptContainer);
		}
		else if( instances.Length > 1 )
		{
			Debug.LogError("Multiple (" + instances.Length + ") instances of object " + typeof(T).Name + " found in this scene. Returning the first.");
			_instance = instances[0];
		}
		
		if (_instance != null)
		{
			LugusSingletonCrossScene<T> singleton = _instance as LugusSingletonCrossScene<T>;
			
			// For more info, see LugusSingletonExisting or LugusSingletonRuntime.
			if (singleton != null)
				singleton.InitializeSingleton();
		}
	}
	
	public virtual void InitializeSingleton()
	{
	}
	
	public void Change(T newInstance)
	{
		_instance = newInstance;
	}
	
	public void Clear()
	{
		Destroy(this.gameObject);
	}
	
	protected void OnDisable()
	{
		_instance = null;
	}
	
	protected void OnDestroy()
	{
		this.enabled = false;
		_instance = null;
	}
}
