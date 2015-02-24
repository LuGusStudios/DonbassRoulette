using UnityEngine;
using UnityEditor;
using System.Collections;

public class SpriteAnimImporterPreferencesWindow : EditorWindow 
{
	
	private const string c_WindowTitle = "Sprite Animation Importer Preferences";
	private GUIContent m_NameFilterLabel;
	private GUIContent m_BoneShapePrefixLabel;
	private GUIContent m_ExceptionBonesLabel;
	
	private void OnEnable () 
	{
		m_NameFilterLabel = new GUIContent ("Name Filter", "Models with names that contain '" + SpriteAnimImporterPrefs.NameFilter + "', will go through the sprite animation post processor. The name filter is not case sensitive.");
	}
	
	private void OnGUI () 
	{
		this.title = c_WindowTitle;

		GUILayout.Label ("Model Name Filter", EditorStyles.boldLabel);
		SpriteAnimImporterPrefs.NameFilter = EditorGUILayout.TextField (m_NameFilterLabel, SpriteAnimImporterPrefs.NameFilter);
		
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button ("Use Defaults")) 
		{
			SpriteAnimImporterPrefs.UseDefaultEditorPrefs ();
		}

		GUILayout.FlexibleSpace();

		EditorGUILayout.EndHorizontal();
	}
	
	[MenuItem ("Window/Sprite Anim Importer Preferences")]
	private static void CreateConfigurationWindow() {
		EditorWindow.GetWindow (typeof(SpriteAnimImporterPreferencesWindow));
	}
}
