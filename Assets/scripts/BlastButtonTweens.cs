using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class BlastButtonTweens : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] RectTransform blastButton;
    private Animator blastButtonAnimator;

    private void Awake()
    {
        MergeManager.onMergeHandled += OnFruitMergeCallback;
        blastButtonAnimator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        MergeManager.onMergeHandled -= OnFruitMergeCallback;
    }

    private void OnFruitMergeCallback(FruitType type, Vector2 vector)
    {
        AnimateBlastButton();
    }
    private void AnimateBlastButton()
    {
        int randomNumber = UnityEngine.Random.Range(0, 100);
        if(randomNumber < 80) { return; }

        else if (randomNumber >= 80)
        {
            transform.DOShakePosition(1f, 10f, 1, 50).SetEase(Ease.InOutBounce).SetLoops(2);
        }
        
    }
}
