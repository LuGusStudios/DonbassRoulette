using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ScreenCapManager : MonoBehaviour 
{
    public string baseName = "Frame";
    public string extension = ".png";
    public int framesToCapture = 30;
    public string directory = "ScreenCaps";
    public bool captureSeriesOnStart = false;

    protected int frameCounter = 1;
    protected bool capturingSeries = false;

	public void SetupLocal()
	{
		// assign variables that have to do with this class only
	}
	
	public void SetupGlobal()
	{
        if (captureSeriesOnStart)
            capturingSeries = true;
	}
	
	protected void Awake()
	{
		SetupLocal();
	}

	protected void Start() 
	{
		SetupGlobal();
	}

    protected void CheckFolder()
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
	
	protected void Update() 
	{
        if (LugusInput.use.KeyDown(KeyCode.A) && !capturingSeries)
            CaptureSingle();

        if (LugusInput.use.KeyDown(KeyCode.S) && !capturingSeries)
        {
            capturingSeries = true;
        }

        if (capturingSeries)
        {
            if (frameCounter <= framesToCapture)
            {
                print(frameCounter);
                CaptureSingle();
                frameCounter++;
            }

            if (frameCounter > framesToCapture)
            {
                Debug.Log("ScreenCapManager: Finished capturing series.");
                capturingSeries = false;
            }
        }
	}

    protected void CaptureSingle()
    {
        CheckFolder();

        string name = directory + Path.DirectorySeparatorChar + baseName + frameCounter.ToString() + extension;
        Application.CaptureScreenshot(name);
        Debug.Log("ScreenCapManager: Made screenshot: " + name);
    }


}
