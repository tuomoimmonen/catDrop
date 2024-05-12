using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PowerUpManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Button powerUpButton;
    private Animator powerButtonAnim;

    [Header("Settings")]
    [SerializeField] int blastPrice;

    [Header("Data")]
    public bool noSmallFruits;

    public Fruit[] smallFruits;

    [Header("Events")]
    public static Action onPowerUpButtonPressed;

    private void Awake()
    {
        //powerUpButton.interactable = false;
        powerButtonAnim = powerUpButton.gameObject.GetComponent<Animator>();
        //powerUpButton.gameObject.SetActive(false);
        CoinManager.onCoinValueUpdated += CoinsUpdatedCallback;
        PowerUpManager.onPowerUpButtonPressed += ManagePowerUpButtonInterractability;
    }

    private void Start()
    {
        //ManagePowerUpButtonInterractability();
        powerButtonAnim.gameObject.transform.DOScale(0f, 0.2f);
    }

    private void OnDisable()
    {
        CoinManager.onCoinValueUpdated -= CoinsUpdatedCallback;
        PowerUpManager.onPowerUpButtonPressed -= ManagePowerUpButtonInterractability;
    }

    private void CoinsUpdatedCallback()
    {
        //Debug.Log("fire");
        ManagePowerUpButtonInterractability();
    }

    public void PowerUpButtonCallback()
    {
        Fruit[] smallFruits = FruitManager.instance.GetSmallFruitsForPowerUp();
        //ManagePowerUpButtonInterractability();
        
        //guard for no smallfruits
        if(smallFruits.Length <= 0)
        {
            noSmallFruits = true;
            ManagePowerUpButtonInterractability();
            //powerUpButton.gameObject.SetActive(false);
            return; 
        }
        

        for (int i = 0; i < smallFruits.Length; i++)
        {
            smallFruits[i].HandleMergeParticles(); //all small fruits so spawnable
        }
        CoinManager.instance.AddCoins(-blastPrice);
    }

    private void ManagePowerUpButtonInterractability()
    {
        //bool canBlast = CoinManager.instance.CanPurchase(blastPrice);
        //powerUpButton.gameObject.SetActive(canBlast);

        StartCoroutine(DelayBeforeCheckingIfNoSmallFruits());

        /*
        if(canBlast && !noSmallFruits)
        {
            powerButtonAnim.gameObject.transform.DOScale(1.2f, 0.5f);
            //powerButtonAnim.SetBool("canSpawn", true);
        }
        */
        

        //StartCoroutine(WaitBeforeActivatingButton());
    }

    private IEnumerator WaitBeforeActivatingButton()
    {
        yield return new WaitForEndOfFrame();
        bool canBlast = CoinManager.instance.CanPurchase(blastPrice);

        if (canBlast && !noSmallFruits)
        {
            powerButtonAnim.gameObject.transform.DOScale(1.2f, 0.5f);
            //powerButtonAnim.SetBool("canSpawn", true);
        }
    }

    private IEnumerator DelayBeforeCheckingIfNoSmallFruits()
    {
        yield return new WaitForSeconds(0.7f); //this is guard for spikes, sometimes missing with low values

        smallFruits = FruitManager.instance.GetSmallFruitsForPowerUp();

        //guard for no smallfruits
        if (smallFruits.Length <= 0)
        {
            powerButtonAnim.gameObject.transform.DOScale(0f, 0.5f);
            //powerButtonAnim.SetBool("canSpawn", false);
            noSmallFruits = true;
        }
        else if (smallFruits.Length >= 1)
        {
            noSmallFruits = false;
        }

        yield return new WaitForSeconds(0.2f);
        bool canBlast = CoinManager.instance.CanPurchase(blastPrice);

        if (canBlast && !noSmallFruits)
        {
            //Debug.Log("should show button");
            powerButtonAnim.gameObject.transform.DOScale(1.2f, 0.5f);
            //powerButtonAnim.SetBool("canSpawn", true);
        }


    }
}
