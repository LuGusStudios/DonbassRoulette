using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Advertisements;

public class LevelSelectMenu : MonoBehaviour {

    Button btnChooseLeft;
    Button btnChooseRight;
    Button btnStart;
    Button btnBack;
    Image imgStart;

    bool _isShowingAd = false;
    bool _hasMadeSelection = false;

    Side chosenSide = Side.None;

	// Use this for initialization
	void Start () {
        btnChooseLeft = gameObject.FindComponentInChildren<Button>(true, "ButtonLeft");
        btnChooseRight= gameObject.FindComponentInChildren<Button>(true, "ButtonRight");
        btnStart = gameObject.FindComponentInChildren<Button>(true, "btn_start");
        btnBack = gameObject.FindComponentInChildren<Button>(true, "btn_back");

        imgStart = btnStart.transform.GetChild(0).GetComponent<Image>();

        btnChooseLeft.onClick.AddListener(() => { LugusCoroutines.use.StartRoutine(DoSelectSide(Side.Left)); });
        btnChooseRight.onClick.AddListener(() => { LugusCoroutines.use.StartRoutine(DoSelectSide(Side.Right)); });
        btnStart.onClick.AddListener(DoStart);
        btnBack.onClick.AddListener(DoBack);
	}

    void OnEnable()
    {
        if (btnChooseLeft == null || btnChooseRight == null) return;

        btnChooseLeft.image.color = new Color(1f, 1f, 1f);
        btnChooseRight.image.color = new Color(1f, 1f, 1f);

        btnChooseLeft.transform.localScale = new Vector3(1f, 1f, 1f);
        btnChooseRight.transform.localScale = new Vector3(1f, 1f, 1f);

        _isShowingAd = false;
        _hasMadeSelection = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (chosenSide != Side.None)
        {
            float freq = 5;
            float amp = 0.1f;

            imgStart.transform.localScale = Vector3.one + Vector3.one * Mathf.Sin(Time.realtimeSinceStartup * freq) * amp;
        }
	}

    void DoStart()
    {
        Debug.Log("Chosen side: " + chosenSide.ToString());

        if (chosenSide == Side.None) return;

        if (Advertisement.isReady())
        {
            _isShowingAd = true;
            Advertisement.Show(null, new ShowOptions
            {
                resultCallback = result =>
                {
                    AdCallback(result);
                }
            });
        }
        else
        {
            Debug.Log("No add could be displayed");
            StartGame();
        }
    }

    void DoBack()
    {
        if (_isShowingAd || _hasMadeSelection) return;
        MenuManager.use.Goto(MenuManager.MenuType.MAINMENU);
    }

    IEnumerator DoSelectSide(Side side)
    {                        
        if (_isShowingAd) yield break;        

        chosenSide = side;

        Button btnSelect;
        Button btnOther;

        if (chosenSide == Side.Left) {
            btnSelect = btnChooseLeft;
            btnOther = btnChooseRight;
        }
        else {
            btnSelect = btnChooseRight;
            btnOther = btnChooseLeft;
        }

        AssignSidesAndFaction(leftSide);

        btnSelect.gameObject.ScaleTo(Vector3.one * 1.05f).Time(0.5f).EaseType(iTween.EaseType.easeOutBounce).Execute();
        btnSelect.image.color = new Color(1f, 1f, 1f);
        btnOther.gameObject.ScaleTo(Vector3.one * 0.9f).Time(0.5f).EaseType(iTween.EaseType.easeOutBounce).Execute();
        btnOther.image.color = new Color(0.2f, 0.2f, 0.2f);

    }

    void AdCallback(ShowResult result)
    {
        Debug.Log( "Add result: " + result.ToString() );
        _isShowingAd = false;        

        // These can be used determine action depending on the add result
        /*if (result == ShowResult.Finished){}
        else if (result == ShowResult.Skipped){}
        else if (result == ShowResult.Failed){}*/

        StartGame();
    }

    void StartGame()
    {
        if (chosenSide == Side.Left)
        {

        }
        else 
        {
        
        }

        LugusCamera.game.gameObject.FindComponentInChildren<MinimapCamera>(true).gameObject.SetActive(true);
        DataLoader dl = FindObjectOfType<DataLoader>();
        dl.Load("level_01");
        MenuManager.use.Goto(MenuManager.MenuType.GAMEMENU);
        CameraController.use.InitializeView();
    }

    protected void AssignSidesAndFaction(bool playerIsRebel)
    {
        if (playerIsRebel)
        {
            GameData.use.player.m_side = Side.Left;
            GameData.use.player.faction = Faction.Rebel;

            GameData.use.ai.m_side = Side.Right;
            GameData.use.ai.faction = Faction.Ukraine;
        }
        else
        {
            GameData.use.player.m_side = Side.Right;
            GameData.use.player.faction = Faction.Ukraine;

            GameData.use.ai.m_side = Side.Left;
            GameData.use.ai.faction = Faction.Rebel;
        }

        Debug.Log("LevelSelectMenu: Assigned player faction: <color=magenta>" + GameData.use.player.faction + "</color>");  // Colors work in debug output. Wow.
    }
}
