using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Settings")]
    private GameState gameState;

    [Header("Events")]
    public static Action<GameState> onGameStateChanged;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;

        SetMenuState();

        Application.targetFrameRate = 60;
    }

    private void SetMenuState()
    {
        SetGameState(GameState.Menu);
    }

    private void SetGame()
    {
        SetGameState(GameState.Game);
    }

    private void SetGameOver()
    {
        SetGameState(GameState.GameOver);
    }

    private void SetGameState(GameState gameState)
    {
        this.gameState = gameState;

        onGameStateChanged.Invoke(gameState);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState()
    {
        SetGame();
    }

    public bool IsGameState()
    {
        return gameState == GameState.Game;
    }

    public void SetGameOverState()
    {
        SetGameOver();
    }

}
