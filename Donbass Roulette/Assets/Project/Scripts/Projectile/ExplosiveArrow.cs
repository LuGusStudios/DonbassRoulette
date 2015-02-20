using UnityEngine;
using System.Collections;

public class ExplosiveArrow : Arrow {
	public AreaOfEffect m_aoe;

	protected override void ApplyEffect(Collider2D col)
	{
		Instantiate(m_aoe, this.transform.position, this.transform.rotation);
		Destroy(this.gameObject);
	}

	override protected IEnumerator Remove()
	{
		m_removing = true;
		yield return new WaitForSeconds(m_timeRemove); // TODO : fade-out instead of disappearing after X seconds
		Instantiate(m_aoe, this.transform.position, this.transform.rotation);
		Destroy(this.gameObject);
	}
}
