using UnityEngine;
using System.Collections;

public class Bullet : Projectile {
	public float m_speed;
	protected Vector3 m_prvPos;
	protected bool m_start = false;


	void Start()
	{
		m_prvPos = this.transform.position;
	}


	override protected void ApplyEffect(Collider2D col)
	{
		Body body = col.GetComponent<Body>();

		if(body)
		{
			body.ReduceHp(m_value);
			Destroy(this.gameObject);

		}
	}
	override public void Initialize(float value, Vector3 goal)
	{
		m_value = value;
		this.gameObject.MoveTo(goal).Speed(m_speed).Execute();
	}


	void Update()
	{
		Vector3 diff = this.transform.position - m_prvPos;
		if( diff != Vector3.zero )
			this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(-diff.y, -diff.x) + 90);

		m_prvPos = this.transform.position;

	}

}
