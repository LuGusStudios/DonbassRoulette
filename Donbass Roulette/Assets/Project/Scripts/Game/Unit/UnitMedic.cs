using UnityEngine;
using System.Collections;

public class UnitMedic : Unit {
    public float m_healRange;
    public float m_healValue;

    public delegate void OnHeal(Body target);
    public OnHeal onHeal = null;

    override protected void Update()
    {
        if (m_hp <= 0)
            return;

        Body onRangeAlly = GetNearestSideBody(this.m_side, m_healRange, Composition.Organic);
        if (onRangeAlly != null && onRangeAlly.GetHpRatio() < 1 && onRangeAlly.GetHpRatio() > 0)
        {
            Heal(onRangeAlly);
        }
        else
        {
            base.Update();
        }
    }


    protected void Heal(Body body)
    {
        body.AddHp(m_healValue);

        if (onHeal != null)
            onHeal(body);
    }

    override protected void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        // draw circle2D
        Gizmos.color = Color.yellow;
        const int sections = 12;
        float radius = m_healRange;

        Vector3 center = this.transform.position;

        Vector3 prvPos = new Vector3(center.x + radius, center.y, center.z);
        for (float f = 0; f < 2 * Mathf.PI; f += (2 * Mathf.PI) / sections)
        {
            float x = center.x + radius * Mathf.Cos(f);
            float y = center.y - radius * Mathf.Sin(f);

            Vector3 newPos = new Vector3(x, y, center.z);
            Gizmos.DrawLine(prvPos, newPos);
            prvPos = newPos;
        }
    }
}
