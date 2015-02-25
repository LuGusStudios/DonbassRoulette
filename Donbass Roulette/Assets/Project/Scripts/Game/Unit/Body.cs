using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Body : MonoBehaviour {

    public enum Composition
    {
        None = 0,
        Mineral = 1,
        Organic = 2
    };
    public Composition m_composition;
    public float m_hpMax;
	protected float m_hp;

	public float m_deathDestroyTime;
	public Side m_side;
	
	public delegate void Delegate();
	public Delegate m_DelDeath;
    protected Collider2D m_collider = null;

	virtual protected void Start()
	{
		m_hp = m_hpMax;
		if(m_side == Side.Right)
			this.transform.localScale = this.transform.localScale.xMul(-1);

        m_collider = gameObject.FindComponent<Collider2D>();
	}


	protected IEnumerator Death()
	{
        m_collider.enabled = false;

		if(m_DelDeath != null)
			m_DelDeath();

		yield return new WaitForSeconds(m_deathDestroyTime); // death animation time

		Destroy(this.transform.parent.gameObject);
	}


	public void ReduceHp( float value )
	{
        if (m_hp <= 0)
            return;

		m_hp -= value;
		if(m_hp <= 0)
		{
			m_hp = 0;

			StartCoroutine(Death());
		}
	}

	public void AddHp( float value )
	{
		m_hp += value;
		if(m_hp > m_hpMax)
			m_hp = m_hpMax;
	}


	public float GetHpRatio()
	{
		return (m_hp / m_hpMax);
	}

    // This function will find all relevant sprite renderers, separate them and send them flying.
    public void Explode(float force, Vector2 position)
    {
        Transform graphicsParent = null;

        // We only want sprite renderers under a certain parent, e.g. not the body's healthbar.
        // TODO: Something more elegant?
        graphicsParent = this.transform.FindChild("Graphics");

        // Not al bodies have this set (e.g. only infantry units), but that makes sense, because they're the only ones that can be blown apart anyway.
        if (graphicsParent == null)
            return;

        SpriteRenderer[] parts = graphicsParent.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer sr in parts)
        {
            // Ideally, parent all parts to this object's parent, so they don't move along anymore,
            // but still get deleted with it.
            Transform newParent = this.transform.parent;

            // Have a backup just in case of weird situations where the parent somehow is null.
            if (newParent == null)
                newParent = this.transform;

            sr.transform.parent = this.transform.parent;

            BoxCollider2D boxCollider2D =
            sr.gameObject.AddComponent<BoxCollider2D>();

            boxCollider2D.size = Vector2.one;
            
            Rigidbody2D rigidbody2d =
            sr.gameObject.AddComponent<Rigidbody2D>();

            // The tiny random vector2 below adds a little random torque on the exploding parts.
            Vector2 direction = ( ((new Vector2(Random.value , Random.value) * 0.01f ) + sr.transform.position.ToVector2()) - position).normalized;

            rigidbody2d.angularDrag = 1f;
            rigidbody2d.mass = 1.0f;
            rigidbody2d.AddForceAtPosition(direction * force, position, ForceMode2D.Impulse);
        }
    }

}
