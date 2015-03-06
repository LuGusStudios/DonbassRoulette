using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameOverMenu : MonoBehaviour {

    List<Text> ScoreTexts = new List<Text>();
    Button btnContinue;

    Text txtWinMessage;
    Text txtLoseMessage;

    List<int> scores = new List<int>();

	// Use this for initialization
	void Start () {
        
        // Fill scores texts
        ScoreTexts.Add(gameObject.FindComponentInChildren<Text>(true, "txtGunshot"));
        ScoreTexts.Add(gameObject.FindComponentInChildren<Text>(true, "txtMortar"));
        ScoreTexts.Add(gameObject.FindComponentInChildren<Text>(true, "txtRocket"));
        ScoreTexts.Add(gameObject.FindComponentInChildren<Text>(true, "txtStarvation"));
        ScoreTexts.Add(gameObject.FindComponentInChildren<Text>(true, "txtTotal"));

        btnContinue = gameObject.FindComponentInChildren<Button>(true, "btn_continue");
        btnContinue.gameObject.SetActive(false);
        btnContinue.onClick.AddListener(DoContinue);

        txtWinMessage = gameObject.FindComponentInChildren<Text>(true, "txtWonTheBattle");
        txtLoseMessage = gameObject.FindComponentInChildren<Text>(true, "txtLostTheBattle");

        if (CrossSceneMenuInfo.use.isPlayerWinner)
        {
            txtWinMessage.gameObject.SetActive(true);
            txtLoseMessage.gameObject.SetActive(false);
        }
        else
        {
            txtWinMessage.gameObject.SetActive(false);
            txtLoseMessage.gameObject.SetActive(true);
        }

        scores = new List<int>(new int[] { 23, 475, 86, 24, 610 });
        LugusCoroutines.use.StartRoutine(AnimateScores(scores));
	}

    void OnEnable()
    {
        AnalyticsIntegration.GameOverEvent(CrossSceneMenuInfo.use.unitsSpawned, CrossSceneMenuInfo.use.lvlDuration);
        CrossSceneMenuInfo.use.resetDict();

        LugusCamera.game.gameObject.FindComponentInChildren<MinimapCamera>(true).gameObject.SetActive(false);
        
        //if (CrossSceneMenuInfo.use.isPlayerWinner)
        //{
        //    txtWinMessage.gameObject.SetActive(true);
        //    txtLoseMessage.gameObject.SetActive(false);
        //}
        //else
        //{
        //    txtWinMessage.gameObject.SetActive(false);
        //    txtLoseMessage.gameObject.SetActive(true);
        //}
                
    }

	// Update is called once per frame
	void Update () {
	
	}

    void DoContinue()
    {
        MenuManager.use.Goto(MenuManager.MenuType.SHAREMENU);
    }

    IEnumerator AnimateScores(List<int> scores)
    {
        if (scores.Count != ScoreTexts.Count)
        {
            Debug.LogError("GameOverMenu: Scores and text counts do not match!");
            yield break;
        }

        for (int i = 0; i < scores.Count; i++)
        {
            Text text = ScoreTexts[i];
            text.text = "0";
        }


        for (int i = 0; i < scores.Count; i++)
        {               
            int score = scores[i];
            Text text = ScoreTexts[i];

            float time = 1.5f;            
            float scorePerSecond = (float)score / time;

            float counter = 0;
            float increment = 0;

            while (counter < score)
            {
                increment = scorePerSecond * Time.deltaTime;
                text.text = Mathf.FloorToInt(counter).ToString();
                counter += increment;
                yield return null;
            }
            text.text = Mathf.FloorToInt(score).ToString();
        }

        btnContinue.gameObject.SetActive(true);
        btnContinue.transform.localScale = Vector3.zero;
        btnContinue.gameObject.ScaleTo(Vector3.one).Time(0.5f).EaseType(iTween.EaseType.easeOutBounce).Execute();
    }
}
