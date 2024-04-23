using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject mapPanel;

    [Header("Events")]
    public static Action onMapOpened;

    private void Awake()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
        LevelMapManager.onLevelButtonClicked += LevelButtonCallback;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        LevelMapManager.onLevelButtonClicked -= LevelButtonCallback;
    }

    private void GameStateChangedCallback(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                SetMenu();
                break;

            case GameState.Game:
                SetGame();
                break;

            case GameState.GameOver:
                SetGameOver();
                break;
        }
    }
    private void SetMenu()
    {
        menuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(false);
        settingsPanel.SetActive(false);
        shopPanel.SetActive(false);
        mapPanel.SetActive(false);
    }

    private void SetGameOver()
    {
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gamePanel.SetActive(false);
        mapPanel.SetActive(false);
    }

    private void SetGame()
    {
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);
        mapPanel.SetActive(false);
    }

    public void LevelButtonCallback()
    {
        GameManager.instance.SetGameState();
    }

    public void NextButtonCallback()
    {
        SceneManager.LoadScene(0);
    }

    public void SettingsButtonCallback()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void ShopPanelCallback() => shopPanel.SetActive(true);

    public void CloseShopPanel() => shopPanel.SetActive(false);

    public void OpenMap() 
    { 
        mapPanel.SetActive(true);
        onMapOpened?.Invoke();
    }

    public void CloseMap() => mapPanel.SetActive(false);
}
