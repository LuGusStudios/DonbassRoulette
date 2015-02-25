using UnityEngine;
using System.Collections;

public class UnitRifle : Unit {

    override protected void Attack(Body body)
    {
        if (m_DelAttack != null)
            m_DelAttack();

        if (m_projectile != null)
        {// range attack
            Projectile projectile = Instantiate(m_projectile, this.transform.position, Quaternion.identity) as Projectile;
            projectile.Initialize(this.m_side, this.m_damage, body.transform.position.yAdd(Random.Range(-0.8f, 0.8f)));
        }
        else
        {// melee attack (immediate)
            body.ReduceHp(m_damage);
        }

        m_attackTimer = m_attackCooldown;
    }
}
