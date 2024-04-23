using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMapManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] RectTransform mapContent;
    [SerializeField] RectTransform[] levelButtonParents;
    [SerializeField] LevelButton levelButtonPrefab;

    [Header("Data")]
    [SerializeField] LevelDataSO[] levelDatas;

    [Header("Events")]
    public static Action onLevelButtonClicked;

    private void Awake()
    {
        UiManager.onMapOpened += UpdateLevelButtonsInterractability;
    }

    private void OnDisable()
    {
        UiManager.onMapOpened -= UpdateLevelButtonsInterractability;
    }
    void Start()
    {
        Initialize();   
    }

    private void Initialize()
    {
        mapContent.anchoredPosition = Vector2.up * 1920 * (mapContent.childCount - 1);

        CreateLevelButtons();
        UpdateLevelButtonsInterractability();
    }

    private void CreateLevelButtons()
    {
        for (int i = 0; i < levelDatas.Length; i++)
        {
            CreateLevelButton(i, levelButtonParents[i]);
        }
    }

    private void CreateLevelButton(int buttonIndex, Transform levelButtonParent)
    {
        LevelButton levelButtonInstance = Instantiate(levelButtonPrefab, levelButtonParent);
        levelButtonInstance.Configure(buttonIndex + 1);

        levelButtonInstance.GetButton().onClick.AddListener(() => LevelButtonClicked(buttonIndex));
    
    }

    private void LevelButtonClicked(int buttonIndex)
    {
        while(transform.childCount > 0)
        {
            Transform t = transform.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        //spawn the level prefab
        Instantiate(levelDatas[buttonIndex].GetLevel(), transform);

        //start the game
        onLevelButtonClicked?.Invoke();
    }

    private void UpdateLevelButtonsInterractability()
    {
        int highScore = ScoreManager.instance.GetHighScore();

        for(int i = 0;i < levelDatas.Length;i++)
        {
            if (levelDatas[i].GetRequiredHighScore() <= highScore)
            {
                levelButtonParents[i].GetChild(0).GetComponent<LevelButton>().Enable();
            }
        }
    }
}
