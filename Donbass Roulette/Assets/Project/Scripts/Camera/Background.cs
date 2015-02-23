using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Sprite))]
public class Background : MonoBehaviour {

	public void Move(float value)
	{
		this.transform.position = this.transform.position.xAdd(value);
	}
}
