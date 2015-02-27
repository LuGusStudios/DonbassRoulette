using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HideOnDeath : MonoBehaviour 
{
    protected Body controllingBody = null;

    public void SetupGlobal() 
	{
        controllingBody = gameObject.FindComponentInParent<Body>(true);

        if (controllingBody == null)
        {
            this.enabled = false;
        }
        else
        {
            controllingBody.m_delDeath += OnDeath;
        }
	}
	
	protected void Start() 
	{
		SetupGlobal();
	}

    protected void OnDeath()
    {
        this.gameObject.SetActive(false);
    }
	
}
