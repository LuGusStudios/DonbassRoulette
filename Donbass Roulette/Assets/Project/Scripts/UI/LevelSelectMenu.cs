using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Advertisements;

public class LevelSelectMenu : MonoBehaviour {

    Button btnChooseLeft;
    Button btnChooseRight;
    Button btnBack;

    bool _isShowingAd = false;
    bool _hasMadeSelection = false;

	// Use this for initialization
	void Start () {
        btnChooseLeft = gameObject.FindComponentInChildren<Button>(true, "ButtonLeft");
        btnChooseRight= gameObject.FindComponentInChildren<Button>(true, "ButtonRight");
        btnBack = gameObject.FindComponentInChildren<Button>(true, "btn_back");

        btnChooseLeft.onClick.AddListener(() => { LugusCoroutines.use.StartRoutine(DoSelectSide(true)); });
        btnChooseRight.onClick.AddListener(() => { LugusCoroutines.use.StartRoutine(DoSelectSide(false)); });
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
	
	}

    void DoBack()
    {
        if (_isShowingAd || _hasMadeSelection) return;
        MenuManager.use.Goto(MenuManager.MenuType.MAINMENU);
    }

    IEnumerator DoSelectSide(bool leftSide)
    {                        
        if (_isShowingAd || _hasMadeSelection) yield break;
        _hasMadeSelection = true;

        Button btnSelect;
        Button btnOther;
        if (leftSide) {
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

        yield return new WaitForSeconds(1.0f);

        //if (_isShowingAd) yield break;

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

    void AdCallback(ShowResult result)
    {
        Debug.Log( "Add result: " + result.ToString() );
        _isShowingAd = false;        

        if (result == ShowResult.Finished)
        {
        
        }
        else if (result == ShowResult.Skipped)
        { 
        
        }
        else if (result == ShowResult.Failed)
        {

        }

        StartGame();
    }

    void StartGame()
    {
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
