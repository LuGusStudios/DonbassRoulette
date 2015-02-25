using UnityEngine;
using System.Collections;

public abstract class AreaOfEffect : MonoBehaviour {
	public Side m_side;
    public float m_value;

	public enum SpellType
	{
		Offensive = 0,
		Healing = 1,
		Defensive = 2,
		Others = 3
	};

	abstract public SpellType GetSpellType();


	public int m_times = 1;
	public float m_duration;
	public float m_range;

	abstract protected void ApplyEffect(Collider2D col);

	protected float m_timer = 0;
	protected int m_timeApplied = 0;


	public void Initialize(Side side)
	{
		m_side = side;
	}


	void Update()
	{

		float timeDiv = 0;// if m_times == 0, do it every frame
		if( m_times != 0 )
			timeDiv = m_duration / m_times;

		if(m_timer > m_duration)
		{
			Destroy(this.gameObject);
		}
		else if(m_timer >= timeDiv * m_timeApplied)
		{
			Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, m_range);
			foreach(Collider2D col in cols)
			{
				ApplyEffect(col);
			}

            OnApply();

			m_timeApplied++;
		}

		m_timer += Time.deltaTime;
	}

    //Override for some extra effect every time the effect is applied, regardless of whether it hit something. (e.g. camera shake).
    public virtual void OnApply()
    { 
    }

	void OnDrawGizmos()
	{
		// draw circle2D
		Gizmos.color = Color.yellow;
		const int sections = 12;
		float radius = m_range;

		Vector3 center = this.transform.position;

		Vector3 prvPos = new Vector3(center.x + radius, center.y, center.z);
		for(float f = 0; f < 2 * Mathf.PI; f += (2 * Mathf.PI) / sections)
		{
			float x = center.x + radius * Mathf.Cos(f);
			float y = center.y - radius * Mathf.Sin(f);

			Vector3 newPos = new Vector3(x, y, center.z);
			Gizmos.DrawLine(prvPos, newPos);
			prvPos = newPos;
		}
	}
}
