using UnityEngine;
using System.Collections;


public class Factory : MonoBehaviour {
    protected Map m_map;
	public GameObject m_prefabUnit;
	public int m_price;

	public float m_cooldown;
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

            // This is a bit of a silly fix. Given that some units have bone animations, they also have parts that are offset on the z-axis.
            // Because of this, they might sometimes clip through one another on the z-axis. To prevent this, we make them very narrow in that direction.
            // Normally, this would be applied on the prefab, but that requires us to apply to every single unit (which is prone to oversights)
            // and has the additional disadvantage that it becomes hard to edit the prefab.
            // Instead, we "flatten" the unit in code here. On units that don't need it, the effect is invisible anyway.
            // Note: we can't scale them to 0, because that would give z-fighting.
            obj.transform.localScale = obj.transform.localScale.z(0.05f);

			Unit unit = obj.GetComponentInChildren<Unit>();
			unit.Initialize(side);
			
			m_timer = m_cooldown;
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
