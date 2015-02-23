using UnityEngine;
using System.Collections;
using System.IO;

public abstract class FileManager : MonoBehaviour
{
	private bool Exist(string path)
	{
		if(File.Exists(path))
		{
			return true;
		}
		return false;
	}

	public void Save(string fileLocation, string filePath)
	{
		if(!Directory.Exists(fileLocation))
			Directory.CreateDirectory(fileLocation);

		if(Exist(filePath))
		{
			Debug.Log("File already exist");
		}
		else
		{
			Debug.Log("File saved on " + filePath);
			P_Save(filePath);
		}
		
	}

    public void Load(string tempPath)
	{
        string filePath = Application.dataPath + tempPath;
		if(!Exist(filePath))
		{
			Debug.Log("File doesn't exist");
		}
		else
		{
			Debug.Log("File loaded : " + filePath);
			P_Load(filePath);
		}
	}

	protected abstract void P_Save(string path);
	protected abstract void P_Load(string path);
}
