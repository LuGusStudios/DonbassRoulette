﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : LugusSingletonExisting<SoundManager> 
{
    public AudioClip backgroundAmbientClip = null;
    public AudioClip backgroundMusicClip = null;

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

    public void SetupGlobal()
    {
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

            backgroundMusic.Play(backgroundMusicClip, new LugusAudioTrackSettings().Loop(true).Volume(0.2f));
        }

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