using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShareMenu : MonoBehaviour {

    Button btnShareFB;
    Button btnShareTwitter;

    Button btnReplay;
    Button btnMenu;

    Text msgContent;

    string message = "I won the battle but lost the game. Just like may civilian casualties every day. Play now: Battle for Donetsk";
    string messageTwitter = "I won the battle but lost the game. Just like may civilian casualties every day. Play now: ";
    //string messageWin = "I spared the lives of countless civilians and opened the road to peace. Play now: Battle for Donetsk";

	// Use this for initialization
	void Start () {
        btnShareFB = gameObject.FindComponentInChildren<Button>(true, "btn_facebook");
        btnShareTwitter = gameObject.FindComponentInChildren<Button>(true, "btn_twitter");
        btnReplay = gameObject.FindComponentInChildren<Button>(true, "btn_replay");
        btnMenu = gameObject.FindComponentInChildren<Button>(true, "btn_menu");

        msgContent = gameObject.FindComponentInChildren<Text>(true, "txt_message");

        btnShareFB.onClick.AddListener(DoShareOnFB);
        btnShareTwitter.onClick.AddListener(DoShareOnTwitter);
        btnReplay.onClick.AddListener(DoReplay);
        btnMenu.onClick.AddListener(DoGotoMenu);

        if (!GameData.use.ceasefireBroken)
        {
            message = LugusResources.use.Localized.GetText("share.messagewin");
            messageTwitter = LugusResources.use.Localized.GetText("share.messagewintwitter");
        }
        else
        {
            message = LugusResources.use.Localized.GetText("share.message");
            messageTwitter = LugusResources.use.Localized.GetText("share.messagetwitter");
        }

        msgContent.text = message;
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
        CrossSceneMenuInfo.use.nextMenuOnReload = MenuManager.MenuType.LEVELSELECTMENU;
        Application.LoadLevel(Application.loadedLevel);
    }

    void DoGotoMenu()
    {
        CrossSceneMenuInfo.use.nextMenuOnReload = MenuManager.MenuType.MAINMENU;
        Application.LoadLevel(Application.loadedLevel);
    }

    void DoShareOnFB()
    {
        Debug.Log("Sharing on FB");
        AnalyticsIntegration.ClickedSocialFacebookEvent();           
        SocialShareBasic.use.facebook.Share(message);
    }

    void DoShareOnTwitter()
    {
        Debug.Log("Sharing on Twitter");
        AnalyticsIntegration.ClickedSocialTwitterEvent();
        SocialShareBasic.use.twitter.Share(messageTwitter);
    }
}
