using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] AudioSource mergeSource;
    [SerializeField] AudioSource mergeSource2;
    [SerializeField] AudioSource spawnSource;

    [Header("Sounds")]
    [SerializeField] AudioClip[] mergeClips;
    [SerializeField] AudioClip[] spawnClips;

    private void Awake()
    {
        MergeManager.onMergeHandled += MergeHandledCallback;
        SettingsManager.onSfxValueChanged += SfxValueChangedCallback;
        FruitManager.onFruitSpawned += FruitSpawnedCallback;
    }

    private void OnDisable()
    {
        MergeManager.onMergeHandled -= MergeHandledCallback;
        SettingsManager.onSfxValueChanged -= SfxValueChangedCallback;
        FruitManager.onFruitSpawned -= FruitSpawnedCallback;
    }

    private void SfxValueChangedCallback(bool sfxActive)
    {
        mergeSource.mute = !sfxActive;
        mergeSource2.mute = !sfxActive;
        spawnSource.mute = !sfxActive;
        //mergeSource.volume = sfxActive ? 1.0f : 0.0f; //true = 1, false 0
    }
    private void MergeHandledCallback(FruitType unUsed1, Vector2 unUsed2) 
    {
        PlayMergeSound();
    }

    public void PlayMergeSound()
    {
        if (!mergeSource.isPlaying)
        {
            float randomPitch = Random.Range(0.8f, 1.3f);
            mergeSource.clip = mergeClips[Random.Range(0, mergeClips.Length)];
            mergeSource.Play();
        }
        else
        {
            float randomPitch = Random.Range(0.9f, 1.2f);
            mergeSource2.clip = mergeClips[Random.Range(0, mergeClips.Length)];
            mergeSource2.Play();
        }

    }

    private void FruitSpawnedCallback(Fruit currentFruit)
    {
        PlaySpawnSound();
    }

    private void PlaySpawnSound()
    {
        float randomPitch = Random.Range(0.95f, 1.1f);
        spawnSource.clip = spawnClips[Random.Range(0, spawnClips.Length)];
        spawnSource.Play();
    }

    public void OnButtonClickedCallback()
    {
        float randomPitch = Random.Range(0.8f, 1.2f);
        mergeSource.clip = spawnClips[0];
        mergeSource.Play();
    }
}
