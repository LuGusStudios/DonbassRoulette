using UnityEngine;
using System.Collections;

public class Bullet : Projectile {
    public Vector3 m_dir;
    public float m_distLeft;
	public float m_speed;
	protected Vector3 m_prvPos;
	protected bool m_start = false;


	void Start()
	{
		m_prvPos = this.transform.position;
	}

	override protected void ApplyBodyEffect(Body body)
	{
		body.ReduceHp(m_value);
		Destroy(this.gameObject);
	}
    override protected void ApplyNonBodyEffect(Collider2D col)
    {
        Destroy(this.gameObject);
    }

	override public void Initialize(Side side, float value, Vector3 goal)
	{
        m_side = side;
		m_value = value;

        m_dir = (goal - this.transform.position).normalized;
        m_distLeft = (goal - this.transform.position).magnitude * 2;

        this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(-m_dir.y, -m_dir.x) + 90);
    }

    void FixedUpdate()
    {
        Vector3 move = m_dir * m_speed;
        this.transform.position += move;
        m_distLeft -= m_speed;
        if( m_distLeft < 0 )
        {
            Destroy(this.gameObject);
        }
    }

	void Update()
	{
	}

}
