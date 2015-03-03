using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour 
{
    protected Entity entity = null;
    protected GameObject muzzleFlash = null;
    protected ILugusCoroutineHandle showMuzzleFlashRoutine = null;

	void Start () 
    {
        entity = gameObject.FindComponentInParent<Entity>();

        if (entity != null)
        {
            entity.m_delAttack += Attack;
            entity.m_delDeath += Death;
        }

	}

    protected void Attack()
    {
        if (showMuzzleFlashRoutine != null && showMuzzleFlashRoutine.Running)
        {
            showMuzzleFlashRoutine.StopRoutine();
        }

        // Run this coroutine on the game object itself to ensure it is destroyed along with it.
        showMuzzleFlashRoutine = LugusCoroutines.use.StartRoutine(ShowMuzzleFlash(), this.gameObject);

    }

    protected void Death()
    {
        if (showMuzzleFlashRoutine != null && showMuzzleFlashRoutine.Running)
        {
            showMuzzleFlashRoutine.StopRoutine();
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    protected IEnumerator ShowMuzzleFlash()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(2f);
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
}
