using UnityEngine;
using System.Collections;

public enum LugusResourceType
{
	None = -1,
	Shared = 0,
	Localized = 1
}

public class LugusAudioSource : MonoBehaviour 
{
	public string key = "";
	public Lugus.AudioChannelType channelType = Lugus.AudioChannelType.NONE;
	public bool stopOthers = false;
	public bool playOnStart = true;
	public bool loop = false;
	public bool preload = true;


	public LugusResourceType resourceType = LugusResourceType.None;

	protected void AssignKey()
	{
		if( string.IsNullOrEmpty(key) )
		{
			if( this.audio != null )
			{
				if( this.audio.clip != null )
				{
					key = this.audio.clip.name;
					Debug.LogWarning(name + " : key was empty! Using audio clip name instead. Key was: " + key );
				}
			}
		}
	}
	
	protected AudioClip clip = null;
	
	public void Play()
	{
		if( clip == null )
		{
			FetchClip(); // here it's really needed, so no check for preload there
		}
		
		if( clip == null )
		{
			Debug.LogError(name + " : audioClip " + key + " not found");
			return;
		}
		
		LugusAudioChannel channel = LugusAudio.use.GetChannel( channelType );
		// TODO: best cache the GetAudio result and only re-fetch (and re-cache) when we receive callback from LugusResources) 
		channel.Play( clip, this.stopOthers, new LugusAudioTrackSettings().Loop(this.loop)); 
	}
	
	// Use this for initialization
	void Start () 
	{
		AssignKey();
		
		LugusResources.use.onResourcesReloaded += UpdateClip;
		
		if( preload )
			UpdateClip();

		if (playOnStart)
			Play();
	}
	
	public void UpdateClip()
	{
		if( preload )
			FetchClip();
	}

	// The resourceType allows a more directed search through LugusResources. If set to None, will default to still checking both Shared and Localized.
	protected void FetchClip()
	{
		if (resourceType == LugusResourceType.None)
			clip = LugusResources.use.GetAudio(key);
		else if (resourceType == LugusResourceType.Shared)
			clip = LugusResources.use.Shared.GetAudio(key);
		else if (resourceType == LugusResourceType.Localized)
			clip = LugusResources.use.Localized.GetAudio(key);
	}
}
