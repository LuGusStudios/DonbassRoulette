using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

    Slider slMusic = null;
    Slider slFX = null;
    Button btnBack = null;    

	// Use this for initialization
	void Start () {
        Debug.Log("Start");

        slMusic = gameObject.FindComponentInChildren<Slider>(true, "slider_music");
        slFX = gameObject.FindComponentInChildren<Slider>(true, "slider_FX");
        btnBack = gameObject.FindComponentInChildren<Button>(true, "btn_back");

        slMusic.onValueChanged.AddListener(SliderMusicChanged);
        slFX.onValueChanged.AddListener(SliderFXChanged);
        btnBack.onClick.AddListener(DoBack);

        LoadStoredSettings();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        CameraController.use.blockingInput = true;
        AnalyticsIntegration.OpenOptionsEvent();
    }

    void LoadStoredSettings()
    {
        slMusic.value = LugusConfig.use.System.GetFloat("MusicVolume", 1);
        slFX.value =    LugusConfig.use.System.GetFloat("FXVolume", 1);
    }

    void SliderMusicChanged(float val)
    {
        LugusConfig.use.System.SetFloat("MusicVolume", val, true);
        LugusAudio.use.Music().VolumePercentage = val;
    }

    void SliderFXChanged(float val)
    {
        LugusConfig.use.System.SetFloat("FXVolume", val, true);
        LugusAudio.use.SFX().VolumePercentage = val;
    }

    void DoBack()
    {        
        LugusConfig.use.System.Store();
        MenuManager.use.GotoPrevious();
    }


}
