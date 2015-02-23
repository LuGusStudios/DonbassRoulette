using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]
public class UserDataText : MonoBehaviour {
	public User m_user;
	protected GUIText m_text;

	void Start()
	{
		m_text = this.GetComponent<GUIText>();
	}


	void Update () {
		m_text.text = m_user.m_money.ToString() + "(+" + m_user.m_income + ")"
			+" $\n" + m_user.GetMana().ToString() + "(+" + m_user.m_manaRegen + ")/" + m_user.m_manaMax + " MP";
	}
}
