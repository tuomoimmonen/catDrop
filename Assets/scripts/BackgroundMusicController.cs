using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicController : MonoBehaviour
{
    public static BackgroundMusicController instance;

    [Header("Elements")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip[] backgroundMusics;
    [SerializeField] Vector2 volumeMinMaxValue;
    private float musicVolume;
    private int musicIndex;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        SettingsManager.onSfxValueChanged += SfxValueChangedCallback;
        SettingsManager.onMusicVolumeChanged += OnMusicVolumeChangedCallback;
    }

    private void OnMusicVolumeChangedCallback(float obj)
    {
        musicVolume = Mathf.Lerp(volumeMinMaxValue.x, volumeMinMaxValue.y, obj);
        musicSource.volume = musicVolume;
    }

    private void OnDestroy()
    {
        SettingsManager.onSfxValueChanged -= SfxValueChangedCallback;
        SettingsManager.onMusicVolumeChanged -= OnMusicVolumeChangedCallback;
    }

    private void Start()
    {
        musicSource.clip = backgroundMusics[0];
        musicSource.Play();
    }

    void Update()
    {
        if(!musicSource.isPlaying) { PlayNextSong(); }
    }

    private void PlayNextSong()
    {
        if(!musicSource.isPlaying)
        {
            musicIndex = (musicIndex + 1) % backgroundMusics.Length;
            musicSource.clip = backgroundMusics[musicIndex];
            musicSource.Play();
        }
    }

    private void SfxValueChangedCallback(bool sfxActive)
    {
        musicSource.mute = !sfxActive;
        //musicSource.volume = musicVolume;
    }
}
