using UnityEngine;
using UnityEditor;
using System.Collections;
 
class ProjectAssetPostprocessor : AssetPostprocessor
{
	// Use this to only apply post-processing to certain directories.
	// Others can then remain unaffected by this script and retain the settings set by the author (e.g. for asset packs, standard assets).
	
	public static string[] includedFolders = new string[]{"Assets/Project", "Assets/Resources"}; 
	protected string DEFAULT_KEY = "DEFAULTS_SET";
	protected uint DEFAULT_VERSION = 1;


	// DEFAULTS==================================================================================================================================
	// SET SENSIBLE DEFAULTS HERE ON A PER-PROJECT BASIS

	// AUDIO DEFAULTS
	public static bool audio3DDefault = false;
	public static string[] compressedAudioExtensions = {".mp3", ".ogg"}; 
	public static float streamFromDiscLength = 60.0f;
	public static float compressedInMemoryLength = 30.0f;

	// Set this AssetPostprocessor as first in line. Other processors that run later need to override this method with a higher number.
	public override int GetPostprocessOrder()
	{
		return 0;
	}

	// http://docs.unity3d.com/ScriptReference/AssetPostprocessor.GetVersion.html
	public override uint GetVersion()
	{
		return DEFAULT_VERSION;
	}

	// UTILITIES------------------------------------------------------------------------------------------------------------------------------------
	public static bool IsIncludedFolder(string path)
	{
		foreach (string s in includedFolders)
		{
			if (path.StartsWith(s))
				return true;
		}

		return false;
	}

	protected string FilePathWithoutExtension(string path)
	{
		int fileExtPos = path.LastIndexOf(".");
		
		if (fileExtPos >= 0 )
			return path.Substring(0, fileExtPos);
		else
			return path;
	}

	
	protected bool DefaultsSet
	{
		get
		{
			string key = string.Format("{0}_{1}", DEFAULT_KEY, DEFAULT_VERSION);
			return assetImporter.userData.Contains(key);
		}
		set
		{
			if (value == true)
			{
				string key = string.Format("{0}_{1}", DEFAULT_KEY, DEFAULT_VERSION);
				assetImporter.userData = key;
			}
			else
			{
				assetImporter.userData = string.Empty;
			}
		}
	}
	 
	// PRE/POST-PROCESSING ----------------------------------------------------------------------------------------------------------------------------------
	protected void OnPreprocessModel()
	{
		PreprocessModel();
	}

	protected void OnPreprocessAudio()
	{
		PreprocessAudio();
	}

	protected void OnPostprocessAudio(AudioClip clip)
	{
		PostProcessAudio(clip);
	}
	

	protected void PreprocessModel ()
	{
		if (!IsIncludedFolder(assetPath))
			return;

		if (DefaultsSet)
			return;

		ModelImporter importer = (ModelImporter) assetImporter;

		importer.globalScale = 1.0f;
		importer.importMaterials = false;

		string pathWithoutExtension = FilePathWithoutExtension(assetPath);

		if (pathWithoutExtension.Contains("@"))
		{
			importer.importAnimation = true;
		}
		else
		{
			importer.importAnimation = false;
			importer.animationType = ModelImporterAnimationType.None;
		}

		DefaultsSet = true;

		Debug.Log("AssetPostprocessor: preprocessed " + assetPath +".");
	}

	protected void PreprocessAudio ()
	{
		if (!IsIncludedFolder(assetPath))
			return;

		if (DefaultsSet)
			return;

		AudioImporter importer = (AudioImporter) assetImporter;

		importer.threeD = audio3DDefault;

		// Other settings will happen in OnPostProcessAudio, since we want access to the audio clip's length.
		// Setting DefaultsSet to true won't happen here yet either. That way, PostProcessAudio can also still run.

		Debug.Log("AssetPostprocessor: preprocessed " + assetPath +".");
	}

	protected void PostProcessAudio(AudioClip clip)
	{
		if (!IsIncludedFolder(assetPath))
			return;
		
		if (DefaultsSet)
			return;

		AudioImporter importer = (AudioImporter) assetImporter;
		bool compressedFile = false;
		bool reimportNeeded = false;

		foreach (string extension in compressedAudioExtensions)
		{
			if ( assetPath.EndsWith(extension) )
			{
				compressedFile = true;
				break;
			}
		}

		if (compressedFile)
		{
			if (clip.length >= streamFromDiscLength)
			{
				if (importer.loadType != AudioImporterLoadType.StreamFromDisc)
				{
					importer.loadType = AudioImporterLoadType.StreamFromDisc;
					reimportNeeded = true;
				}
			}
			else if (clip.length >= compressedInMemoryLength)
			{
				if (importer.loadType != AudioImporterLoadType.CompressedInMemory)
				{
					importer.loadType = AudioImporterLoadType.CompressedInMemory;
					reimportNeeded = true;
				}
			}
			else
			{
				if (importer.loadType != AudioImporterLoadType.DecompressOnLoad)
				{
					importer.loadType = AudioImporterLoadType.DecompressOnLoad;
					reimportNeeded = true;
				}
			}
		}
		else
		{
			// If file is kinda long, import this uncompressed file as compressed anyway.
			if (clip.length >= compressedInMemoryLength)
			{
				if (importer.format != AudioImporterFormat.Compressed)
				{
					importer.format = AudioImporterFormat.Compressed;
					importer.loadType = AudioImporterLoadType.CompressedInMemory;
					reimportNeeded = true;
				}
			}

			// If file is quite long, set to stream from disc.
			if (clip.length >= streamFromDiscLength)
			{
				if (importer.loadType != AudioImporterLoadType.StreamFromDisc)
				{
					importer.loadType = AudioImporterLoadType.StreamFromDisc;
					reimportNeeded = true;
				}
			}      
		}

		if (reimportNeeded)
		{
			AssetDatabase.ImportAsset(assetPath);
		}

		DefaultsSet = true;

		Debug.Log("AssetPostprocessor: postprocessed " + assetPath +".");
	}



}