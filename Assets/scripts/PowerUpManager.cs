using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Button powerUpButton;

    [Header("Settings")]
    [SerializeField] int blastPrice;

    [Header("Data")]
    private bool noSmallFruits;

    private void Awake()
    {
        powerUpButton.interactable = false;
        CoinManager.onCoinValueUpdated += CoinsUpdatedCallback;
    }

    private void OnDisable()
    {
        CoinManager.onCoinValueUpdated -= CoinsUpdatedCallback;
    }

    private void CoinsUpdatedCallback()
    {
        ManagePowerUpButtonInterractability();
    }

    public void PowerUpButtonCallback()
    {
        Fruit[] smallFruits = FruitManager.instance.GetSmallFruitsForPowerUp();

        //guard for no smallfruits
        if(smallFruits.Length <= 0)
        {
            return; 
        }
        for (int i = 0; i < smallFruits.Length; i++)
        {
            smallFruits[i].HandleMergeParticles();
        }

        CoinManager.instance.AddCoins(-blastPrice);
    }

    private void ManagePowerUpButtonInterractability()
    {
        bool canBlast = CoinManager.instance.CanPurchase(blastPrice);
        powerUpButton.interactable = canBlast;
    }
}
