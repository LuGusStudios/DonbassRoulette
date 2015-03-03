using UnityEngine;
using System.Collections;

public class ExplosiveArrow : Arrow {
	public AreaOfEffect m_aoe;

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
        AreaOfEffect aoe = Instantiate(m_aoe, this.transform.position, this.transform.rotation) as AreaOfEffect;
        aoe.m_value = this.m_value;
        Destroy(this.gameObject);

        SoundManager.use.PlaySound(LugusAudio.use.SFX(), SoundManager.use.GetRandomExplosionSound());
    }

	override protected IEnumerator Remove()
	{
		m_removing = true;
		yield return new WaitForSeconds(m_timeRemove); // TODO : fade-out instead of disappearing after X seconds
		Instantiate(m_aoe, this.transform.position, this.transform.rotation);
		Destroy(this.gameObject);
	}
}
