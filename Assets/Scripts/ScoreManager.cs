using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }



    public TextMeshProUGUI nowScoreUI;
    public TextMeshProUGUI bestScoreUI;
    private int nowScore = 0;

    public int bestScore = 0;

    public int NowScore
    {
        get => nowScore;
        set
        {
            //value == scoreManager.NowScore + 1;
            nowScore = value;
            nowScoreUI.text = "Now Score : " + nowScore;

            if (nowScore > bestScore)
            {
                bestScore = nowScore;
                bestScoreUI.text = "Best Score : " + bestScore;
            }

            PlayerPrefs.SetInt("Best score", bestScore);
        }
    }

    public int getScore()
    {
        return nowScore;
    }
    public void setScore(int value)
    {
        nowScore++;
        nowScoreUI.text = "Now Score : " + nowScore;

        if (nowScore > bestScore)
        {
            bestScore = nowScore;
            bestScoreUI.text = "Best Score : " + bestScore;
        }

        PlayerPrefs.SetInt("Best score", bestScore);
    }

    // Start is called before the first frame update
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("Best score");
        bestScoreUI.text = "Best Score : " + bestScore;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
