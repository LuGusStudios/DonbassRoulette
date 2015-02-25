using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RPG : Projectile 
{
    public float m_timeRemove;
    public float m_timeReach;
    public AreaOfEffect m_aoe;

    protected Vector3 m_prvPos;

    override public void Initialize(Side side, float value, Vector3 goal)
    {
        m_side = side;
        m_value = value;
        StartCoroutine(LifetimeTo(goal));
    }

    IEnumerator LifetimeTo(Vector3 goal)
    {
        Vector3[] path = { this.transform.position, Vector3.Lerp(this.transform.position, goal, .5f) + new Vector3(0, 1, 0), goal };

         this.gameObject.MoveTo(path).Time(m_timeReach).Execute();

         yield break;
    }

    protected override void ApplyBodyEffect(Body body)
    {
        Explode();
    }
    protected override void ApplyNonBodyEffect(Collider2D col)
    {
        Explode();
    }

    protected void Explode()
    {
        m_removing = true;

        AreaOfEffect aoe = Instantiate(m_aoe, this.transform.position, this.transform.rotation) as AreaOfEffect;
        aoe.m_value = this.m_value;
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (!m_removing)
        {
            Vector3 diff = this.transform.position - m_prvPos;
            this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(-diff.y, -diff.x) + 90);
            m_prvPos = this.transform.position;
        }
    }
}
