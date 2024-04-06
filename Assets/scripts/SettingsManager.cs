using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject resetProgressPrompt;
    [SerializeField] Slider pushForceSlider;
    [SerializeField] Toggle sfxToggle;

    [Header("Events")]
    public static Action<float> onPushForceChanged;
    public static Action<bool> onSfxValueChanged;

    [Header("Data")]
    private const string lastPushForceKey = "lastPushForceKey";
    private const string isSoundOnKey = "isSoundOnKey";
    private bool isSoundOn;
    private bool canSave;

    private void Awake()
    {
        LoadData();
    }
    IEnumerator Start()
    {
        //initialize the values
        Initialize();

        yield return new WaitForSeconds(0.5f);
        canSave = true;

    }

    private void Initialize()
    {
        onPushForceChanged?.Invoke(pushForceSlider.value);
        onSfxValueChanged?.Invoke(sfxToggle.isOn);

    }

    public void ToggleCallback(bool sfxActive)
    {
        //isSoundOn = sfxActive;
        onSfxValueChanged?.Invoke(sfxToggle.isOn);
        SaveData();
    }
    public void ResetButtonCallback()
    {
        resetProgressPrompt.SetActive(true);
    }

    public void ResetProgressYes()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void ResetProgressNo()
    {
        resetProgressPrompt.SetActive(false);
    }

    public void SliderValueChanged()
    {
        onPushForceChanged?.Invoke(pushForceSlider.value);

        SaveData();
    }

    private void LoadData()
    {
        pushForceSlider.value = PlayerPrefs.GetFloat(lastPushForceKey,0);
        sfxToggle.isOn = PlayerPrefs.GetInt(isSoundOnKey) == 1;
    }

    private void SaveData()
    {
        if(!canSave) { return; }

        int sfxValue = sfxToggle.isOn ? 1 : 0;
        PlayerPrefs.SetFloat(lastPushForceKey , pushForceSlider.value);
        PlayerPrefs.SetInt(isSoundOnKey, sfxValue);
    }
}
