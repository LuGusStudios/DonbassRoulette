using UnityEngine;
using UnityEditor;
using System.Collections;
 
class ProjectAssetPostprocessor : AssetPostprocessor
{
	// Use this to exclude assets in certain directories from post-processing on import.
	// They can then remain unaffected by this script and retain the settings set by the author (e.g. for asset packs).
	
	protected string[] excludedFolders = new string[]{"Assets/ExternalAssets", "Assets/StandardAssets"};

	protected bool IsExcludedFolder(string path)
	{
		foreach (string s in excludedFolders)
		{
			if (path.StartsWith(s))
				return true;
		}

		return false;
	}
	 
	protected void OnPreprocessModel ()
	{
		PreprocessModel();
	}

	protected void PreprocessModel ()
	{
		if (IsExcludedFolder(assetPath))
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

		Debug.Log("AssetPostprocessor: preprocessed " + assetPath +".");
	}
	
	protected string FilePathWithoutExtension(string path)
	{
		int fileExtPos = path.LastIndexOf(".");

		if (fileExtPos >= 0 )
			return path.Substring(0, fileExtPos);
		else
			return path;
	}
}