using UnityEngine;
using System.Collections;

public class Arrow : Projectile {
	public float m_timeReach;
	public float m_timeRemove;
	
	protected Vector3 m_prvPos;
	protected bool m_start = false;

	public override void Initialize( float value, Vector3 goal )
	{
		m_value = value;
		Vector3[] path = { this.transform.position, Vector3.Lerp(this.transform.position,goal,.5f) + new Vector3(0,3,0), goal };
		this.gameObject.MoveTo(path).Time(m_timeReach).Execute();
	}


	protected override void ApplyEffect(Collider2D col)
	{
		Body body = col.GetComponent<Body>();

		if(body)
		{
			body.ReduceHp(m_value);
			Destroy(this.gameObject);
		}
	}

	virtual protected IEnumerator Remove()
	{
		m_removing = true;
		yield return new WaitForSeconds(m_timeRemove); // TODO : fade-out instead of disappearing after X seconds
		Destroy(this.gameObject);
	}
	
	void Update () {
		if(m_start)
		{
			Vector3 diff = this.transform.position - m_prvPos;

			if(diff == Vector3.zero)
			{
				StartCoroutine(Remove());
				m_start = false;
			}
			else
			{
				this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(-diff.y, -diff.x) + 90);
			}
		}

		if(m_prvPos != Vector3.zero)
		{
			m_start = true;
		}

		m_prvPos = this.transform.position;
	}
}
