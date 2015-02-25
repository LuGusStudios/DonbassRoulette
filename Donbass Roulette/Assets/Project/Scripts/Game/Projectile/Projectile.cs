using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public abstract class Projectile : MonoBehaviour
{
    public Side m_side;
	protected float m_value;
	protected bool m_removing = false;

    abstract public void Initialize(Side side, float value, Vector3 goal);

    abstract protected void ApplyBodyEffect(Body body);
    abstract protected void ApplyNonBodyEffect(Collider2D col);


	void OnTriggerEnter2D(Collider2D col)
	{
        if (!m_removing)
        {
            Body body = col.GetComponent<Body>();
            if (body)
            {
                if (body.m_side != m_side)
                    ApplyBodyEffect(body);
            }
            else
            {
                ApplyNonBodyEffect(col);
            }
        }
	}
}
