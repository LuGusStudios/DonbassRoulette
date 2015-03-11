using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : LugusSingletonExisting<SoundManager> 
{
    public AudioClip backgroundAmbientClip = null;
    public AudioClip backgroundMusicClip = null;
    public AudioClip backgroundMenuClip = null;
    public AudioClip backgroundGameOverClip = null;

    public float volumeFallOffPerMeter = 0.1f;

    public AudioClip[] explosions;
    public AudioClip[] mortarImpacts;

    protected Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
    protected ILugusAudioTrack backgroundAmbient = null;
    protected ILugusAudioTrack backgroundMusic = null;    

    public void PlaySound(LugusAudioChannel channel, AudioClip sound)
    {
        channel.Play(sound);
    }

    protected void Awake()
    {
        SetupLocal();
    }

    protected void Start()
    {
        SetupGlobal();
    }

    public void SetupLocal()
    {
    }

    public void LoadLuGusAudio()
    {
        float valMusic = LugusConfig.use.System.GetFloat("MusicVolume", 0.2f);
        float valAmbient = LugusConfig.use.System.GetFloat("AmbientVolume", 1);
        float valFX = LugusConfig.use.System.GetFloat("FXVolume", 1);

        LugusAudio.use.Music().BaseTrackSettings = new LugusAudioTrackSettings().Volume(valMusic);
        LugusAudio.use.Ambient().BaseTrackSettings = new LugusAudioTrackSettings().Volume(valAmbient);
        LugusAudio.use.SFX().BaseTrackSettings = new LugusAudioTrackSettings().Volume(valFX);

        LugusAudio.use.Music().VolumePercentage = valMusic;
        LugusAudio.use.Ambient().VolumePercentage = valAmbient;
        LugusAudio.use.SFX().VolumePercentage = valFX;
    }

    public void SetupGlobal()
    {
        //LoadLuGusAudio();
        if (backgroundAmbient == null)
        {
            backgroundAmbient = LugusAudio.use.Ambient().GetTrack();
            backgroundAmbient.Claim();

            backgroundAmbient.Play(backgroundAmbientClip, new LugusAudioTrackSettings().Loop(true));
        }

        if (backgroundMusic == null)
        {
            backgroundMusic = LugusAudio.use.Music().GetTrack();
            backgroundMusic.Claim();

            //backgroundMusic.Play(backgroundMusicClip, new LugusAudioTrackSettings().Loop(true).MaxVolume(0.2f).Volume(0.2f));            
        }

        LoadLuGusAudio();

    }

    public void FadeMenuMusic()
    {
        LugusAudio.use.Music().CrossFade(backgroundMenuClip, 1.0f, new LugusAudioTrackSettings().Loop(true));
    }

    public void FadeGameOverMusic()
    {
        LugusAudio.use.Music().CrossFade(backgroundGameOverClip, 1.0f, new LugusAudioTrackSettings().Loop(false));
    }

    public void FadeGameMusic()
    {
        LugusAudio.use.Music().CrossFade(backgroundMusicClip, 1.0f, new LugusAudioTrackSettings().Loop(true));
    }

    public AudioClip GetRandomExplosionSound()
    {
        if (explosions.Length <= 0)
        {
            Debug.Log("SoundManager: No explosions defined.");
            return null;
        }
        return explosions[Random.Range(0, explosions.Length)];
    }

    public AudioClip GetRandomMortarImpactSound()
    {
        if (mortarImpacts.Length <= 0)
        {
            Debug.Log("SoundManager: No mortar impacts defined.");
            return null;
        }

        return mortarImpacts[Random.Range(0, mortarImpacts.Length)];
    }

    public AudioClip GetSound(string clipName, Lugus.LugusResourceCollectionType collectionType)
    {
        if (clipDictionary.ContainsKey(clipName))
            return clipDictionary[clipName];

        ILugusResourceCollection collection = LugusResources.use.GetCollectionOfType(collectionType);

        if (collection == null)
        {
            Debug.LogError("SoundManager: No collection found. Aborting.");
            return null;
        }

        AudioClip newClip = null;

        newClip = collection.GetAudio(clipName);

        if (newClip == LugusResources.use.errorAudio)
        {
            Debug.LogError("SoundManager: No sound found with the name: " + clipName);
            return null;
        }
        else
        {
            clipDictionary.Add(clipName, newClip);
            return newClip;
        }
    }
}
