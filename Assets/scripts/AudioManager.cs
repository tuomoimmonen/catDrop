using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] AudioSource mergeSource;

    private void Awake()
    {
        MergeManager.onMergeHandled += MergeHandledCallback;
        SettingsManager.onSfxValueChanged += SfxValueChangedCallback;
    }

    private void OnDisable()
    {
        MergeManager.onMergeHandled -= MergeHandledCallback;
        SettingsManager.onSfxValueChanged -= SfxValueChangedCallback;
    }

    private void SfxValueChangedCallback(bool sfxActive)
    {
        mergeSource.mute = !sfxActive;
        //mergeSource.volume = sfxActive ? 1.0f : 0.0f; //true = 1, false 0
    }
    private void MergeHandledCallback(FruitType unUsed1, Vector2 unUsed2) 
    {
        PlayMergeSound();
    }

    public void PlayMergeSound()
    {
        float randomPitch = Random.Range(0.8f, 1.3f);
        mergeSource.Play();
    }
}
