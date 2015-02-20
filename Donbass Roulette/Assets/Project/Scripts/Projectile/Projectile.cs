using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public abstract class Projectile : MonoBehaviour
{
	protected float m_value;
	protected bool m_activated = false;
	protected bool m_removing = false;

	abstract protected void ApplyEffect(Collider2D col);
	abstract public void Initialize(float value,Vector3 goal);


	void OnTriggerExit2D(Collider2D col)
	{// activate after exiting the launcher
		m_activated = true;
	}



	void OnTriggerEnter2D(Collider2D col)
	{
		if(m_activated && !m_removing)
		{
			ApplyEffect(col);
		}
	}

	

}
