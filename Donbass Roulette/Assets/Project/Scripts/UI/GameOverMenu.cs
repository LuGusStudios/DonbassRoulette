using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameOverMenu : MonoBehaviour {

    List<Text> ScoreTexts = new List<Text>();
    Button btnContinue;

    Text txtWinMessage;
    Text txtLoseMessage;
    Text txtCeasefireMessage;

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
        txtCeasefireMessage = gameObject.FindComponentInChildren<Text>(true, "txtCeasefire");

        //scores = new List<int>(new int[] { 23, 475, 86, 24, 610 });
        CalcScores();

        if (!GameData.use.ceasefireBroken)
        {
            txtWinMessage.gameObject.SetActive(false);
            txtLoseMessage.gameObject.SetActive(false);
            txtCeasefireMessage.gameObject.SetActive(true);
            scores = new List<int>(new int[] { 0, 0, 0, 0, 0 });
        }
        else if (CrossSceneMenuInfo.use.isPlayerWinner)
        {
            txtWinMessage.gameObject.SetActive(true);
            txtLoseMessage.gameObject.SetActive(false);
            txtCeasefireMessage.gameObject.SetActive(false);
        }
        else
        {
            txtWinMessage.gameObject.SetActive(false);
            txtLoseMessage.gameObject.SetActive(true);
            txtCeasefireMessage.gameObject.SetActive(false);
        }
        
        LugusCoroutines.use.StartRoutine(AnimateScores(scores));
	}

    void OnEnable()
    {
        AnalyticsIntegration.GameOverEvent(CrossSceneMenuInfo.use.unitsSpawned, CrossSceneMenuInfo.use.lvlDuration);
        CrossSceneMenuInfo.use.resetDict();

        LugusCamera.game.gameObject.FindComponentInChildren<MinimapCamera>(true).gameObject.SetActive(false);
        SoundManager.use.FadeGameOverMusic();
        
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

    void CalcScores()
    {
        // avg 100 kills in 2 min

        float duration = CrossSceneMenuInfo.use.lvlDuration;
        float timePercentage = duration / 120.0f;
        float baseAmount = 100;

        //scores = new List<int>(new int[] { 23, 475, 86, 24, 610 });

        //                                                Percentage:  MIN   MAX  
        int gunshot =   Mathf.FloorToInt(timePercentage * Random.Range(0.1f, 0.2f) * baseAmount);
        int mortar =    Mathf.FloorToInt(timePercentage * Random.Range(0.3f, 0.4f) * baseAmount);
        int rpg =       Mathf.FloorToInt(timePercentage * Random.Range(0.2f, 0.3f) * baseAmount);
        int starve =    Mathf.FloorToInt(timePercentage * Random.Range(0.0f, 0.05f) * baseAmount);

        int total = gunshot + mortar + rpg + starve;

        scores = new List<int>(new int[] { gunshot, mortar, rpg, starve, total });
    }

    void DoContinue()
    {
        MenuManager.use.Goto(MenuManager.MenuType.SHAREMENU);
    }

    IEnumerator AnimateScores(List<int> scores)
    {
        if (!GameData.use.ceasefireBroken)
        {
            for (int i = 0; i < scores.Count; i++)
            {
                int score = scores[i];
                Text text = ScoreTexts[i];
                text.text = score.ToString();
            }

            yield return new WaitForSeconds(3.0f);
        }
        else
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

        }

        // Activate continue button
        btnContinue.gameObject.SetActive(true);
        btnContinue.transform.localScale = Vector3.zero;
        btnContinue.gameObject.ScaleTo(Vector3.one).Time(0.5f).EaseType(iTween.EaseType.easeOutBounce).Execute();
    }
}
