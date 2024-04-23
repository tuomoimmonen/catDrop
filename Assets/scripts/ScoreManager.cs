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

    [Header("Data")]
    private int score;
    private int highScore;

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

        LoadData();
    }


    private void OnDisable()
    {
        MergeManager.onMergeHandled -= MergeHandleCallback;
        GameManager.onGameStateChanged -= GameChangedCallback;
    }

    public int GetHighScore() => highScore;

    private void GameChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GameOver:
                CalculateHighscore();
                break;
        }
    }

    private void UpdateHighscoreText()
    {
        highScoreText.text = highScore.ToString();
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
        if(score > highScore)
        {
            highScore = score;
            SaveData();
        }
    }

    private void LoadData()
    {
        highScore = PlayerPrefs.GetInt("highscore", 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("highscore", highScore);
    }
}
