using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Elements")]
    [SerializeField] TMP_Text gameScoreText;
    [SerializeField] TMP_Text highScoreText;
    //[SerializeField] TMP_Text topScoresText;

    [Header("Data")]
    private int score;
    public int[] highScores = new int[3];
    private int topScore;
    private bool canUpdateHighScore = true;

    [Header("Settings")]
    [SerializeField] float scoreMultiplier = 2;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;

        MergeManager.onMergeHandled += MergeHandleCallback;
        GameManager.onGameStateChanged += GameChangedCallback;
        UiManager.onMenuButtonPressedIngame += CalculateHighscore;

        highScores = new int[3];
        LoadData();
    }


    private void OnDisable()
    {
        MergeManager.onMergeHandled -= MergeHandleCallback;
        GameManager.onGameStateChanged -= GameChangedCallback;
        UiManager.onMenuButtonPressedIngame -= CalculateHighscore;
    }

    public int GetHighScore() => topScore;

    private void GameChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GameOver:
                CalculateHighscore();
                break;
            case GameState.Menu:
                canUpdateHighScore = true;
                break;
        }
    }

    private void UpdateHighscoreText()
    {
        highScoreText.text = "";

        for (int i = 0; i < 3; i++)
        {
            highScoreText.text += (i+1) + "." + highScores[i] + "\n";
        }
        //highScoreText.text = highScore.ToString();
    }

    private void Start()
    {
        UpdateGameScoreText();
        UpdateHighscoreText();
    }

    private void MergeHandleCallback(FruitType fruitType, Vector2 unUsed)
    {
        int scoreToAdd = (int)fruitType;
        score += (int)(scoreToAdd * scoreMultiplier);

        UpdateGameScoreText();
    }

    private void UpdateGameScoreText()
    {
        gameScoreText.text = score.ToString();
    }

    private void CalculateHighscore()
    {
        if (!canUpdateHighScore) { return; } //EVENT FIRE GUARD
        canUpdateHighScore = false;

        LoadData();
        
        for (int i = 0; i < highScores.Length; i++)
        {
            if (score > highScores[i])
            {
                for (int j = highScores.Length - 1; j > i; j--)
                {
                    highScores[j] = highScores[j - 1];
                    //Debug.Log(highScores[j]);
                }

                highScores[i] = score;
                break;
            }
        }

        for (int i = 0; i < highScores.Length; i++)
        {
            SaveData(i);
        }

        /*
        if (score > highScore)
        {
            highScore = score;
            SaveData();
        }
        */
    }

    
    private void LoadData()
    {
        
        for (int i = 0; i < 3; i++)
        {
            highScores[i] = PlayerPrefs.GetInt("highscore" + i, 0);
            //Debug.Log(highScores[i]);
        }

        topScore = highScores[0];
        
        //highScores[0] = PlayerPrefs.GetInt("highscore" + 0, 0);
    }
    

    private void SaveData(int index)
    {
        PlayerPrefs.SetInt("highscore" + index, highScores[index]);
    }
}
