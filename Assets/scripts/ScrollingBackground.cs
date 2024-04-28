using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollingBackground : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] backgroundSprites;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        UiManager.onMapOpened += OnMapOpenedCallback;
    }

    private void OnDisable()
    {
        UiManager.onMapOpened -= OnMapOpenedCallback;
    }

    private void OnMapOpenedCallback()
    {
        int randomBackground = UnityEngine.Random.Range(0, backgroundSprites.Length);
        spriteRenderer.sprite = backgroundSprites[randomBackground];
    }
}
