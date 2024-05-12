using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] ParticleSystem comboParticlesLow;
    [SerializeField] ParticleSystem comboParticlesMed;
    [SerializeField] ParticleSystem comboParticlesHigh;

    [Header("Settings")]
    [SerializeField] float comboTime = 3f;
    private bool canSpawnParticles;
    public float timer;
    private bool timerOn;
    public int comboCounter;

    [Header("Events")]
    public static Action on
        ;
    public static Action onMedCombo;
    public static Action onHighCombo;

    private void Awake()
    {
        Fruit.onFruitCollision += OnFruitCollisionCallback;
    }

    private void OnDisable()
    {
        Fruit.onFruitCollision -= OnFruitCollisionCallback;
    }

    void Update()
    {
        if(timerOn)
        {
            timer += Time.deltaTime;
        }

        if(canSpawnParticles)
        {
            HandleCombos();
        }
    }

    private void OnFruitCollisionCallback(Fruit fruit1, Fruit fruit2)
    {
        timerOn = true;
        comboCounter++;
        canSpawnParticles = true;
    }

    private void HandleCombos()
    {
        canSpawnParticles = false;

        if (timer < comboTime)
        {
            switch(comboCounter)
            {
                case 6 :
                    comboParticlesLow.Play();
                    //onLowCombo?.Invoke();
                    //Debug.Log("2");
                    break;
                case 8:
                    comboParticlesMed.Play();
                    comboParticlesLow.Stop();
                    //onMedCombo?.Invoke();
                    //Debug.Log("5");
                    break;
                case 12:
                    comboParticlesHigh.Play();
                    comboParticlesMed.Stop();
                    //onHighCombo?.Invoke();
                    //Debug.Log("8");
                    break;
            }
        }

        else if (timer > comboTime) 
        {
            timer = 0;
            timerOn = false;
            comboCounter = 0;
        }

    }

}
