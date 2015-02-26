using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShareMenu : MonoBehaviour {

    Button btnShareFB;
    Button btnShareTwitter;

    Button btnReplay;
    Button btnMenu;

	// Use this for initialization
	void Start () {
        btnShareFB = gameObject.FindComponentInChildren<Button>(true, "btn_facebook");
        btnShareTwitter = gameObject.FindComponentInChildren<Button>(true, "btn_twitter");
        btnReplay = gameObject.FindComponentInChildren<Button>(true, "btn_replay");
        btnMenu = gameObject.FindComponentInChildren<Button>(true, "btn_menu");

        btnShareFB.onClick.AddListener(DoShareOnFB);
        btnShareTwitter.onClick.AddListener(DoShareOnTwitter);
        btnReplay.onClick.AddListener(DoReplay);
        btnMenu.onClick.AddListener(DoGotoMenu);
	}

    void OnEnable()
    {
        SocialShareBasic.use.InitializeSocial();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void DoReplay()
    { 
    
    }

    void DoGotoMenu()
    { 
        
    }

    void DoShareOnFB()
    {
        Debug.Log("Sharing on FB");
        SocialShareBasic.use.facebook.Share("BFD");
    }

    void DoShareOnTwitter()
    {
        Debug.Log("Sharing on Twitter");
        SocialShareBasic.use.twitter.Share("BAAAAAATLE");
    }
}
