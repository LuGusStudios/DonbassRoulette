using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour {

	public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif 
    }
}
