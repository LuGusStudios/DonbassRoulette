using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crosshair : MonoBehaviour 
{
    public float blinkDuration = 2.0f;
    public int blinkTimes = 2;
    public float minAlpha = 0.5f;

    protected SpriteRenderer spriteRenderer = null;

	public void SetupLocal()
	{
        spriteRenderer = gameObject.FindComponent<SpriteRenderer>();
	}
	
	public void SetupGlobal()
	{
        StartCoroutine(BlinkRoutine());

        Vector3 originalScale = this.transform.localScale;
        this.transform.localScale = originalScale * 1.3f;


        this.gameObject.ScaleTo(originalScale).
           EaseType(iTween.EaseType.easeOutBounce).
           Time(blinkDuration * blinkTimes * 0.4f).
           Execute();


        this.gameObject.ScaleTo(Vector3.zero).
            Delay(blinkDuration * blinkTimes * 0.8f).
            Time(blinkDuration * blinkTimes * 0.2f).
            Execute();
	}
	
	protected void Awake()
	{
		SetupLocal();
	}

	protected void Start() 
	{
		SetupGlobal();
	}

    protected IEnumerator BlinkRoutine()
    {
        int counter = 0;
        float timer = 0;

        while (counter < blinkTimes)
        {
            while (spriteRenderer.color.a > minAlpha)
            {
                spriteRenderer.color = spriteRenderer.color.a(Mathf.Lerp(1.0f, minAlpha, timer / (blinkDuration * 0.5f)));

                timer += Time.deltaTime;
                yield return null;
            }

            timer = 0;

            while (spriteRenderer.color.a < 1)
            {
                spriteRenderer.color = spriteRenderer.color.a(Mathf.Lerp(minAlpha, 1, timer / (blinkDuration * 0.5f)));

                timer += Time.deltaTime;
                yield return null;
            }

            timer = 0;

            counter++;
        }


        yield break;
    }
}
