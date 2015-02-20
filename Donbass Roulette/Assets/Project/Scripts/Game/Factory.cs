using UnityEngine;
using System.Collections;


public class Factory : MonoBehaviour {
    protected Map m_map;
	public GameObject m_prefabUnit;
	public int m_price;

	public float m_couldown;
	private float m_timer = 0;

    void Start()
    {
        m_map = GameObject.FindObjectOfType<Map>();
        if (!m_map)
        {
            Debug.LogError("The Map doesn't exist although Factories which spawn stuff on it does");
        }

    }



	// return the price of the spawn
	public bool Spawn(float posX, Side side)
	{
		if(m_timer <= 0)
		{
            GameObject obj = Instantiate(m_prefabUnit, m_map.GetRandomStartingGroundPos().xAdd(posX), Quaternion.identity) as GameObject;
			Unit unit = obj.GetComponentInChildren<Unit>();
			unit.Initialize(side);
			
			m_timer = m_couldown;
			return true;
		}
		return false;
	}

	public float GetTimer()
	{
		return m_timer;
	}

	void Update () {
		if(m_timer > 0)
		{
			m_timer -= Time.deltaTime;
		}
		else
		{
			m_timer = 0;
		}
	}
}
