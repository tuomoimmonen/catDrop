using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGroundChanger : MonoBehaviour
{
    [Header("Elements")]
    SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] Sprite[] groundSprites;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        LevelMapManager.onLevelButtonClicked += LevelButtonClickedCallback;
    }

    private void OnDisable()
    {
        LevelMapManager.onLevelButtonClicked -= LevelButtonClickedCallback;
    }

    private void LevelButtonClickedCallback()
    {
        SelectRandomGroundSprite();
    }

    private void SelectRandomGroundSprite()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        int randomSprite = Random.Range(0, groundSprites.Length);
        spriteRenderer.sprite = groundSprites[randomSprite];
    }
}
