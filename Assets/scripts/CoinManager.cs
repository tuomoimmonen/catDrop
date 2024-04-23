using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [Header("Variables")]
    private int coins;
    private const string coinsKey = "coins";

    [Header("Events")]
    public static Action onCoinValueUpdated;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;

        LoadData();

        MergeManager.onMergeHandled += MergeHandledCallback;
    }

    private void OnDisable()
    {
        MergeManager.onMergeHandled -= MergeHandledCallback;
    }

    private void Start()
    {
        UpdateCoinsText();
    }

    private void MergeHandledCallback(FruitType fruitType, Vector2 spawnPosition)
    {
        int coinsToAdd = (int)fruitType;

        AddCoins(coinsToAdd);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        coins = Mathf.Clamp(coins, 0, coins); //prevent negative amounts

        UpdateCoinsText();

        SaveData();

        onCoinValueUpdated?.Invoke();
    }

    public bool CanPurchase(int price)
    {
        return coins >= price;
    }

    private void UpdateCoinsText()
    {
        //texts are disabled default, spagetti find
        CoinText[] coinTexts = Resources.FindObjectsOfTypeAll(typeof(CoinText)) as CoinText[];

        for(int i = 0; i < coinTexts.Length; i++)
        {
            coinTexts[i].UpdateText(coins.ToString());
        }
    }

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt(coinsKey, 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(coinsKey, coins);
    }
}
