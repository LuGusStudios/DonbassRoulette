﻿//#define DEBUG_RESOURCES

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lugus
{
	public delegate void OnResourcesReloaded();
}

public class LugusResources : LugusSingletonExisting<LugusResourcesDefault>
{	
	[System.Diagnostics.Conditional("DEBUG_RESOURCES")] 
	public static void LogResourceLoad(string text)
	{
		Debug.Log ("LOAD : " + text);
	}
}

public class LugusResourcesDefault : LugusSingletonExisting<LugusResourcesDefault>
{
	public event Lugus.OnResourcesReloaded onResourcesReloaded;
	
	public List<ILugusResourceCollection> collections = null;
	
	public LugusResourceCollectionDefault Shared = null;
	public LugusResourceCollectionLocalized Localized = null;
	
	public Texture2D errorTexture = null;
	public AudioClip errorAudio = null;
	public Sprite errorSprite = null;
	public TextAsset errorTextAsset = null;

	protected string languageTemp = ""; 


	public override void InitializeSingleton ()
	{
		base.InitializeSingleton ();

		LoadDefaultCollections();
	}

    public ILugusResourceCollection GetCollectionOfType(Lugus.LugusResourceCollectionType type)
    {
        if (type == Lugus.LugusResourceCollectionType.None)
        {
            Debug.LogError("LugusResourcesDefault: Resource collection type was None!");
            return null;
        }
        else if (type == Lugus.LugusResourceCollectionType.Shared)
        {
            return Shared;
        }
        else if (type == Lugus.LugusResourceCollectionType.Localized)
        {
            return Localized;
        }
        else
        {
            Debug.LogError("LugusResourcesDefault: Resource collection type was not recognized: " + type.ToString());
            return null;
        }
    }

	protected void LoadDefaultCollections()
	{ 
		collections = new List<ILugusResourceCollection>();
		
		this.Shared = new LugusResourceCollectionDefault("Shared/");
		this.Localized = new LugusResourceCollectionLocalized("Languages/");
		
		collections.Add ( Localized );
		collections.Add ( Shared );
		
		foreach( ILugusResourceCollection collection in collections )
		{
			collection.onResourcesReloaded += CollectionReloaded;
		}
		
		if( errorTexture == null )
			errorTexture = Shared.GetTexture("error");
		
		if( errorSprite == null )
			errorSprite = Shared.GetSprite("error");
		
		if( errorAudio == null )
			errorAudio = Shared.GetAudio("error");
		
		if( errorTextAsset == null )
			errorTextAsset = Shared.GetTextAsset("error");

		if( !string.IsNullOrEmpty(languageTemp) )
			ChangeLanguage( languageTemp );
	}

	public void ChangeLanguage(string langKey)
	{
		// Quick and dirty fix. We want to set the language key as soon as possible, but this means Awake()s on other objects might run before
		// the collections below have been initialized. If this is the case, the setting of the langID on the collections is delayed until
		// LoadDefaultCollections().
		// TODO: Fix this with InitializeSingleton?
		if(collections == null || collections.Count == 0)
		{
			languageTemp = langKey;
			return;
		}
		
		foreach( ILugusResourceCollection collection in collections )
		{
			if( collection is LugusResourceCollectionLocalized )
			{
				( (LugusResourceCollectionLocalized) collection).LangID = langKey;
			}
		}
	}

	public string GetLocalizedLangID()
	{
		if(collections == null || collections.Count == 0)
		{
			Debug.LogError("LugusResources: Localized resources collected has not yet been initialized.");
			return "";
		}
		
		foreach( ILugusResourceCollection collection in collections )
		{
			if( collection is LugusResourceCollectionLocalized )
			{
				return ( (LugusResourceCollectionLocalized) collection).LangID;
			}
		}
		
		Debug.LogError("LugusResources: No localized resource collection available!");
		return "";
	}

	// Translates system language string to two-character language id.
	// System language can be used as fallback language setting if no language setting has been saved yet.
	public string GetSystemLanguageID()
	{
		switch ( Application.systemLanguage )
		{
			case SystemLanguage.English:
				return "en";

			case SystemLanguage.Dutch:
				return "nl";
				
			default:			// English seems like a sensible pick for a potential international product if the system language isn't supported.
				return "en";
		}
	}
	
	protected void CollectionReloaded()
	{
		if( onResourcesReloaded != null )
			onResourcesReloaded();
	}
	
//	public void Awake()
//	{
//		LoadDefaultCollections();
//	}
	
	public Texture2D GetTexture(string key)
	{	
		Texture2D output = null;
		
		foreach( ILugusResourceCollection collection in collections )
		{
			output = collection.GetTexture(key);
			if( output != errorTexture )
				break;
		}
		
		if( output == errorTexture )
		{
			Debug.LogError(name + " : Texture " + key + " was not found!");
		}
		
		return output;
	}
	
	public Sprite GetSprite(string key)
	{	
		Sprite output = null;
		
		foreach( ILugusResourceCollection collection in collections )
		{
			output = collection.GetSprite(key);
			if( output != errorSprite )
				break;
		}
		
		if( output == errorSprite )
		{
			Debug.LogError(name + " : Texture " + key + " was not found!");
		}
		
		return output;
	}
	
	public AudioClip GetAudio(string key)
	{
		AudioClip output = null;
		
		foreach( ILugusResourceCollection collection in collections )
		{
			output = collection.GetAudio(key);
			if( output != errorAudio )
				break;
		}
		
		if( output == errorAudio )
		{
			Debug.LogError(name + " : AudioClip " + key + " was not found!");
		}
		
		return output;
	}
	
	public string GetText(string key)
	{
		string output = null; 
		
		foreach( ILugusResourceCollection collection in collections )
		{
			output = collection.GetText(key);
			if( output != ("[" + key + "]") )
				break;
		}
		
		if( output == ("[" + key + "]") )
		{
			Debug.LogError(name + " : Text " + key + " was not found!");
		}
		
		return output;
	}

}
