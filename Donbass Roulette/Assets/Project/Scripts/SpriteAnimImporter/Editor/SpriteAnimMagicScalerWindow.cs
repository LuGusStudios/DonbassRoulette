using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SpriteAnimMagicScalerWindow : EditorWindow 
{
	public float magicScale = 4.8f;

	[MenuItem ("Window/Sprite Animation Magic Scale Tool")]
	public static void Init()
	{
		EditorWindow.GetWindow<SpriteAnimMagicScalerWindow>(true, "Sprite Animation Magic Scale Tool");
	}

	public void SetupLocal()
	{
		// assign variables that have to do with this class only
	}
	
	public void SetupGlobal()
	{
		// lookup references to objects / scripts outside of this script
	}
	
	protected void Awake()
	{
		SetupLocal();
	}

	protected void Start() 
	{
		SetupGlobal();
	}
	
	protected void Update() 
	{
	
	}

	protected void OnGUI()
	{
		magicScale = EditorGUILayout.FloatField("Magic scale factor", magicScale);

		if (GUILayout.Button("Apply to selection"))
		{
			ApplyToSelection(Selection.activeGameObject);
		}
	}

	protected void ApplyToSelection(GameObject target)
	{
		if (target == null)
			return;

		foreach(SpriteRenderer sr in target.GetComponentsInChildren<SpriteRenderer>(true))
		{
			sr.transform.localScale = Vector3.one * magicScale;
		}
	}

}
