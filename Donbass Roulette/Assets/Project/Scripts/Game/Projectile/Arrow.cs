using UnityEngine;
using System.Collections;

public class Arrow : Projectile {
	public float m_timeReach;
	public float m_timeRemove;
	
	protected Vector3 m_prvPos;
	protected bool m_start = false;


    override public void Initialize(Side side, float value, Vector3 goal)
	{
        m_side = side;
		m_value = value;
        StartCoroutine(LifetimeTo(goal));
	}

    IEnumerator LifetimeTo(Vector3 goal)
    {
        Vector3[] path = { this.transform.position, Vector3.Lerp(this.transform.position, goal, .5f) + new Vector3(0, 3, 0), goal };
        yield return this.gameObject.MoveTo(path).Time(m_timeReach).YieldExecute();
        StartCoroutine(Remove());
    }

	protected override void ApplyBodyEffect(Body body)
	{
		body.ReduceHp(m_value);
		Destroy(this.gameObject);
	}
    protected override void ApplyNonBodyEffect(Collider2D col)
    {
        StartCoroutine(Remove());   
    }

	virtual protected IEnumerator Remove()
	{
		m_removing = true;
		yield return new WaitForSeconds(m_timeRemove); // TODO : fade-out instead of disappearing after X seconds
		Destroy(this.gameObject);
	}
	
	void Update () {
        if (!m_removing)
        {
            Vector3 diff = this.transform.position - m_prvPos;
            this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(-diff.y, -diff.x) + 90);
            m_prvPos = this.transform.position;
        }
	}
}
