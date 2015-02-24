//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2011-2012 Edelweiss Interactive (http://edelweissinteractive.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SpriteAnimImporter : AssetPostprocessor 
{
	// Override this method to ensure this one runs after other processors.
	public override int GetPostprocessOrder()
	{
		return 1000;
	}

	public override uint GetVersion ()
	{
		return 1;
	}

	protected void OnPreprocessModel ()
	{
		// Restrict this to the same folders as the standard asset post-processor.
		if (!ProjectAssetPostprocessor.IsIncludedFolder(assetPath))
			return;

		if (!assetPath.ToLower().Contains(SpriteAnimImporterPrefs.NameFilter.ToLower()))     
			return;

		ModelImporter importer = (ModelImporter) assetImporter;
		importer.importMaterials = true;



		Debug.Log("SpriteAnimImporter: Asset path " + assetPath + " contained sprite animation marker " + SpriteAnimImporterPrefs.NameFilter.ToLower() +". Beginning import pre-processing.");
	}

	private void OnPostprocessModel (GameObject root) 
	{
		// Restrict this to the same folders as the standard asset post-processor.
		if (!ProjectAssetPostprocessor.IsIncludedFolder(assetPath))
			return;

		if (!root.name.ToLower().Contains(SpriteAnimImporterPrefs.NameFilter.ToLower()))     
			return;

		Debug.Log("SpriteAnimImporter: Game object " + root.name + " contained sprite animation marker " + SpriteAnimImporterPrefs.NameFilter.ToLower() +". Beginning import post-processing.");
			
		
		MeshRenderer[] meshRenderers = root.GetComponentsInChildren<MeshRenderer>(true);

		AssetDatabase.StartAssetEditing();
		// Loop backwards through the list to allow easy deleting of objects.
		for (int i = meshRenderers.Length - 1; i >= 0; i--) 
		{
			MeshRenderer meshRenderer = meshRenderers[i];
			GameObject targetGameObject = meshRenderer.gameObject;

			Material attachedMaterial = meshRenderer.sharedMaterial;

			if (attachedMaterial == null)
			{
				Debug.LogWarning("SpriteAnimImporter: Mesh renderer " + meshRenderer.name + " had no material!");
				continue;
			}

			Texture attachedTexture = attachedMaterial.mainTexture;

			if (attachedTexture == null)
			{
				Debug.LogWarning("SpriteAnimImporter: Material " + attachedMaterial.name + " had no assigned main texture!");
				continue;
			}

			string texturePath = AssetDatabase.GetAssetPath(attachedTexture);
			TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;

			if (textureImporter == null)
			{
				Debug.LogError("SpriteAnimImporter: Failed to retrieve texture importer for texture " + attachedTexture.name + ".");
				continue;
			}

			Sprite retrievedSprite = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Sprite)) as Sprite;

			// NOTE: This won't work correctly!
			// Even though the dialogue window is shown, the import doesn't happen immediately, which means the script will fail to find the sprites further below.
			// Conversion of the textures to sprites will still happen, though, so when you run this script a second time, everything will suddenly be fine...
			if (retrievedSprite == null && textureImporter.textureType != TextureImporterType.Sprite)
			{
 			
				if (EditorUtility.DisplayDialog(
					"Sprite animation contained a non-sprite texture: " + texturePath, 
					"Do you want to reimport this texture as a sprite?", 
					"Yes", 
					"No"))
				{
					textureImporter.textureType = TextureImporterType.Sprite;

					// Some of these might not be necessary, but in various attempts to solve the problem described above, they were all included.
					// They don't seem to do any harm either.
					AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceSynchronousImport);
					AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
					AssetDatabase.SaveAssets();

					retrievedSprite = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Sprite)) as Sprite;
				}

				Debug.LogError("SpriteAnimImporter: Failed to retrieve sprite for texture " + attachedTexture.name + ". It may not yet have been set as a sprite. Fix this and reimport the model.");
			}

			if (retrievedSprite == null)
			{
				Debug.LogError("SpriteAnimImporter: Failed to retrieve sprite for texture " + attachedTexture.name + ".");
				continue;
			}

//			// Automate texture packing.
//			if (string.IsNullOrEmpty(textureImporter.spritePackingTag))
//			{
//				textureImporter.spritePackingTag = root.name;
//			}

			// A sprite associated with the material has been retrieved. We can now proceed to replace the mesh renderer + material with a sprite renderer + sprite setup.

			// Clear the mesh renderer and mesh filter.
			MeshFilter meshFilter = targetGameObject.GetComponent<MeshFilter>();

			if (meshFilter != null)
			{
				if (meshFilter.sharedMesh != null)
					Object.DestroyImmediate(meshFilter.sharedMesh, true);

				Object.DestroyImmediate(meshFilter);
			}

			Object.DestroyImmediate(meshRenderer);

			// Create sprite renderer and assign sprite.
			SpriteRenderer spriteRenderer = targetGameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = retrievedSprite;

		//	targetGameObject.transform.localScale = Vector3.one * 4.8f;



			// Most characters are modeled with their z-axis align with the world z-axis (facing away from the camera.)
			// Sprites, on the other hand, by default face the camera.
			// To counteract this, we need to rotate the game object.
			targetGameObject.transform.Rotate(new Vector3(0, 180, 0));
		}
		AssetDatabase.StopAssetEditing();
		Debug.Log("SpriteAnimImporter: Game object " + root.name + " finished importing as sprite animation.");
	}

}