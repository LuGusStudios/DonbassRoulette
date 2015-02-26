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
        // This is pretty hard-coded for now. Move to public variables if necessary.

        // Camera shake diminshes as player is further from explosion.
        Vector3 cameraShake = new Vector3(0.5f, 0.5f, 0);
        float distanceToCamera = Mathf.Abs(this.transform.position.x - LugusCamera.game.transform.position.x);
        float fallOffPerMeter = 0.1f;
       // LugusCamera.game.Shake(cameraShake * Mathf.Lerp(0.5f, 0.0f, distanceToCamera * fallOffPerMeter), 0.5f);

        // Shake camera parent so that camera itself can keep moving.
        if (LugusCamera.game.transform.parent != null)
        {
            iTween.ShakePosition(LugusCamera.game.transform.parent.gameObject,
            cameraShake * Mathf.Lerp(0.5f, 0.0f, distanceToCamera * fallOffPerMeter),
            0.5f);
        }
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
