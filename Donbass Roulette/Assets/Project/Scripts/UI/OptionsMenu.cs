using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

    Slider slMusic = null;
    Slider slFX = null;
    Slider slAmbient = null;
    Button btnBack = null;    

	// Use this for initialization
	void Start () {
        Debug.Log("Start");

        slMusic = gameObject.FindComponentInChildren<Slider>(true, "slider_music");
        slFX = gameObject.FindComponentInChildren<Slider>(true, "slider_FX");
        slAmbient = gameObject.FindComponentInChildren<Slider>(true, "slider_ambient");
        btnBack = gameObject.FindComponentInChildren<Button>(true, "btn_back");

        slMusic.onValueChanged.AddListener(SliderMusicChanged);
        slFX.onValueChanged.AddListener(SliderFXChanged);
        slAmbient.onValueChanged.AddListener(SliderAmbientChanged);
        btnBack.onClick.AddListener(DoBack);

        slMusic.maxValue = SoundManager.maxMusicVolume;
        slAmbient.maxValue = SoundManager.maxAmbientVolume;
        slFX.maxValue = SoundManager.maxFXVolume;

        LoadStoredSettings();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        CameraController.use.blockingInput = true;
        AnalyticsIntegration.OpenOptionsEvent();
        //SoundManager.use.FadeGameOverMusic();
    }

    void LoadStoredSettings()
    {

        float valMusic = LugusConfig.use.System.GetFloat("MusicVolume", SoundManager.maxMusicVolume);
        float valAmbient = LugusConfig.use.System.GetFloat("AmbientVolume", SoundManager.maxAmbientVolume);
        float valFX = LugusConfig.use.System.GetFloat("FXVolume", SoundManager.maxFXVolume);

        slMusic.value = valMusic;
        slFX.value = valFX;
        slAmbient.value = valAmbient;

        //LugusAudio.use.Music().BaseTrackSettings =      new LugusAudioTrackSettings().Volume(valMusic);
        //LugusAudio.use.Ambient().BaseTrackSettings =    new LugusAudioTrackSettings().Volume(valAmbient);
        //LugusAudio.use.SFX().BaseTrackSettings =        new LugusAudioTrackSettings().Volume(valFX);
    }

    void SliderMusicChanged(float val)
    {                
        LugusConfig.use.System.SetFloat("MusicVolume", val, true);
        LugusAudio.use.Music().BaseTrackSettings = new LugusAudioTrackSettings().Volume(val);
        LugusAudio.use.Music().VolumePercentage = val;
    }

    void SliderFXChanged(float val)
    {        
        LugusConfig.use.System.SetFloat("FXVolume", val, true);        
        LugusAudio.use.SFX().BaseTrackSettings = new LugusAudioTrackSettings().Volume(val);
        LugusAudio.use.SFX().VolumePercentage = val;
    }

    void SliderAmbientChanged(float val)
    {
        LugusConfig.use.System.SetFloat("AmbientVolume", val, true);
        LugusAudio.use.Ambient().BaseTrackSettings = new LugusAudioTrackSettings().Volume(val);
        LugusAudio.use.Ambient().VolumePercentage = val;
    }

    void DoBack()
    {        
        LugusConfig.use.System.Store();
        MenuManager.use.GotoPrevious();
    }


}
