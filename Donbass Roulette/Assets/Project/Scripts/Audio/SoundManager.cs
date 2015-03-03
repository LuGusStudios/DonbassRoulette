using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : LugusSingletonExisting<SoundManager> 
{
    public float volumeFallOffPerMeter = 0.1f;

    public AudioClip[] explosions;
    public AudioClip[] mortarImpacts;
    public AudioClip[] ak47Sounds;

    protected Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();


    public void PlaySound(LugusAudioChannel channel, AudioClip sound)
    {
        channel.Play(sound);
    }

    //public void PlaySound(LugusAudioChannel channel, AudioClip sound, Vector3 position)
    //{
    //    float distanceToCamera = Mathf.Abs(position.x - LugusCamera.game.transform.position.x);
    //    float volume = Mathf.Lerp(1.0f, 0.0f, distanceToCamera * volumeFallOffPerMeter);
    //    print(volume);

    //    channel.Play(
    //        sound,
    //        false,
    //        new LugusAudioTrackSettings().Volume(Mathf.Lerp(1.0f, 0.0f, volume)));
    //}




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

    public AudioClip GetRandomAK47Sound()
    {
        if (ak47Sounds.Length <= 0)
        {
            Debug.Log("SoundManager: No AK47 impacts defined.");
            return null;
        }

        return ak47Sounds[Random.Range(0, ak47Sounds.Length)];
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
