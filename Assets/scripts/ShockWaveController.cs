using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShockWaveController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float shockWaveDuration = 1f;
    [SerializeField] float startPosition = -0.1f;
    [SerializeField] float endPosition = 1f;

    [Header("Elements")]
    [SerializeField] Camera cam;
    [SerializeField] Material shockMaterial;
    [SerializeField] ScriptableRendererFeature shockRenderer;
    private static int waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");

    private void Awake()
    {
        //shockMaterial = GetComponent<SpriteRenderer>().material;
        MergeManager.onMergeHandled += OnMergeCallback;
    }

    private void OnDisable()
    {
        MergeManager.onMergeHandled -= OnMergeCallback;
    }
    private void Start()
    {
        shockRenderer.SetActive(true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) { CallShockWave(); }
    }
    private void OnMergeCallback(FruitType type, Vector2 spawnPosition)
    {
        CallShockWave();
    }
    public void CallShockWave()
    {
        StartCoroutine(ShockWaveAction(startPosition, endPosition));
    }

    private IEnumerator ShockWaveAction(float startPosition, float endPosition)
    {
        //shockRenderer.SetActive(true);

        shockMaterial.SetFloat(waveDistanceFromCenter, startPosition);
    
        float elapsedTime = 0f;
        float lerpedAmount = 0f;

        while (elapsedTime < shockWaveDuration)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startPosition, endPosition, (elapsedTime / shockWaveDuration));
            shockMaterial.SetFloat(waveDistanceFromCenter, lerpedAmount);

            yield return null;
        }

        //shockRenderer.SetActive(false);
    }
}
