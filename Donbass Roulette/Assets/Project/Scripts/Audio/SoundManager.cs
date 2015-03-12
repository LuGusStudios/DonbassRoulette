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

    public static float maxMusicVolume = 0.4f;
    public static float maxAmbientVolume = 1f;
    public static float maxFXVolume = 1f;

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

    public void LoadLuGusAudio()
    {
        // Don't forget to change the slider in the options menu!
        float valMusic = LugusConfig.use.System.GetFloat("MusicVolume", maxMusicVolume);
        float valAmbient = LugusConfig.use.System.GetFloat("AmbientVolume", maxAmbientVolume);
        float valFX = LugusConfig.use.System.GetFloat("FXVolume", maxFXVolume);

        LugusAudio.use.Music().BaseTrackSettings = new LugusAudioTrackSettings().Volume(valMusic);
        LugusAudio.use.Ambient().BaseTrackSettings = new LugusAudioTrackSettings().Volume(valAmbient);
        LugusAudio.use.SFX().BaseTrackSettings = new LugusAudioTrackSettings().Volume(valFX);

        LugusAudio.use.Music().VolumePercentage = valMusic;
        LugusAudio.use.Ambient().VolumePercentage = valAmbient;
        LugusAudio.use.SFX().VolumePercentage = valFX;
    }

    public void SetupGlobal()
    {

    }

    public void FadeMenuMusic()
    {
        // Only crossfade if music isn't already playing
        if (LugusAudio.use.Music().GetActiveTrack() == null || LugusAudio.use.Music().GetActiveTrack().Source.clip != backgroundMenuClip)
        {
            LugusAudio.use.Music().CrossFade(backgroundMenuClip, 1.0f, new LugusAudioTrackSettings().Loop(true));
        }

        float valFX = LugusConfig.use.System.GetFloat("FXVolume", 1);
        LugusAudio.use.SFX().BaseTrackSettings = new LugusAudioTrackSettings().Volume(valFX);
        LugusAudio.use.SFX().VolumePercentage = valFX;
    }

    public void FadeGameOverMusic()
    {
        // Only crossfade if music isn't already playing
        if (LugusAudio.use.Music().GetActiveTrack() == null || LugusAudio.use.Music().GetActiveTrack().Source.clip != backgroundGameOverClip)
        {
            LugusAudio.use.Music().CrossFade(backgroundGameOverClip, 1.0f, new LugusAudioTrackSettings().Loop(false));
        }

        float valFX = 0;
        LugusAudio.use.SFX().BaseTrackSettings = new LugusAudioTrackSettings().Volume(valFX);
        LugusAudio.use.SFX().VolumePercentage = valFX;
    }

    public void FadeGameMusic()
    {
        // Only crossfade if music isn't already playing
        if (LugusAudio.use.Music().GetActiveTrack()== null || LugusAudio.use.Music().GetActiveTrack().Source.clip != backgroundMusicClip)
        {
            LugusAudio.use.Music().CrossFade(backgroundMusicClip, 1.0f, new LugusAudioTrackSettings().Loop(true));
        }

        float valFX = LugusConfig.use.System.GetFloat("FXVolume", 1);
        LugusAudio.use.SFX().BaseTrackSettings = new LugusAudioTrackSettings().Volume(valFX);
        LugusAudio.use.SFX().VolumePercentage = valFX;
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
