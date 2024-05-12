using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject tutorialHand;
    private Animator animator;

    [Header("Data")]
    private bool tutoriaComplete = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        FruitManager.onPlayerInputDetected += OnPlayerInputDetectedCallback;
        LevelMapManager.onLevelButtonClicked += OnPlayerStartedGameCallback;
        LoadData();
    }

    private void OnDisable()
    {
        FruitManager.onPlayerInputDetected -= OnPlayerInputDetectedCallback;
        LevelMapManager.onLevelButtonClicked -= OnPlayerStartedGameCallback;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("tutorialComplete", 0) == 1)
        {
            tutorialHand.SetActive(false);
            animator.enabled = false;
        }
    }

    private void OnPlayerInputDetectedCallback()
    {
        if(PlayerPrefs.GetInt("tutorialComplete",0) == 1) { return; }
        tutoriaComplete = true;
        SaveData();
    }

    private void OnPlayerStartedGameCallback()
    {
        if (PlayerPrefs.GetInt("tutorialComplete", 0) == 1) { return; }
        tutorialHand.SetActive(true);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("tutorialComplete", 1);
        tutorialHand.SetActive(false);
        animator.enabled = false;

    }

    private void LoadData()
    {
        PlayerPrefs.GetInt("tutorialComplete", 0);
    }
}
