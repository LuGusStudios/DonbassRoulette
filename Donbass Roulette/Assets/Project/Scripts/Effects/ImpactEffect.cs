using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactEffect : MonoBehaviour 
{
    public float disappearDelay = 5.0f;
    public float disappearTime = 2.0f;

    protected SpriteRenderer spriteRenderer = null;
    protected ParticleSystem smoke = null; 

	public void SetupLocal()
	{
        spriteRenderer = gameObject.FindComponent<SpriteRenderer>();
        smoke = gameObject.FindComponentInChildren<ParticleSystem>(true, "Smoke");
	}
	
	public void SetupGlobal()
	{
        StartCoroutine(DisappearRoutine());
	}
	
	protected void Awake()
	{
		SetupLocal();
	}

	protected void Start() 
	{
		SetupGlobal();
	}

    protected IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(disappearDelay);

        float timer = 0.0f;
        smoke.Stop();

        while(timer < disappearTime)
        {
            spriteRenderer.color = spriteRenderer.color.a(Mathf.Lerp(1.0f, 0.0f, timer / disappearTime));
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
