using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class CreateEmptyChild : MonoBehaviour 
{
	// Add menu to the main menu
	[MenuItem ("GameObject/Create Empty As Child")]
	private static void CreateGameObjectAsChild () 
	{
		GameObject go = new GameObject ("GameObject");
		go.transform.parent = Selection.activeTransform;
		go.transform.localPosition = Vector3.zero;
		Selection.activeTransform = go.transform;
	}

	// The item will be disabled if no transform is selected.
	[MenuItem ("GameObject/Create Empty As Child", true)]
	private static bool ValidateCreateGameObjectAsChild () 
	{
		return Selection.activeTransform != null;
	}

	// Add context menu to Transform's context menu
	[MenuItem ("CONTEXT/Transform/Create Empty As Child")]
	private static void CreateGameObjectAsChild (MenuCommand command) 
	{
		Transform tr = (Transform) command.context;
		GameObject go = new GameObject ("GameObject");
		go.transform.parent = tr;
		go.transform.localPosition = Vector3.zero;
		Selection.activeTransform = go.transform;
	}
}
