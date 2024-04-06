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

    private void Awake()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
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
    }

    private void SetGameOver()
    {
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    private void SetGame()
    {
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void PlayButtonCallback()
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
}
