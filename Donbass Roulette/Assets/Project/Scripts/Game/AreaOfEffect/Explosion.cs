using UnityEngine;
using System.Collections;


public class Explosion : AreaOfEffect {
    public float explosionForce = 6.0f;

	override public SpellType GetSpellType()
	{
		return SpellType.Offensive;
	}

    public override void OnApply()
    {
        // This is pretty hard-coded for now. Move to public variable if necessary.
        CameraController.use.ShakeCamera(new Vector3(0.5f, 0.5f));
    }

	override protected void ApplyEffect(Collider2D col)
	{
		Body body = col.GetComponent<Body>();
		if(body /*&& body.m_side != m_side*/)
		{
			body.ReduceHp(m_value);

            // If body died, see if it can explode somewhat more... violently...
            if (body.GetHpRatio() <= 0)
                body.Explode(explosionForce, this.transform.position);
		}
	}

}
