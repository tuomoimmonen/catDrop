using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonTweenController : MonoBehaviour
{
    [Header("Settins")]
    [SerializeField] int shakeVibrato = 1;
    [SerializeField] float shakeDuration = 2f;
    [SerializeField] float shakeStrength = 0.1f;
    void Start()
    {
        ShakeButton();
    }

    void Update()
    {
        //if (GameManager.instance.IsGameState()) { transform.DOKill(); }

    }

    private void ShakeButton()
    {
        //transform.DOJump(transform.position, 5f, 2, 2f).SetEase(Ease.InOutBounce).SetLoops(-1);
        transform.DOShakeScale(shakeDuration, shakeStrength,shakeVibrato).SetEase(Ease.InBounce).SetLoops(-1);
    }
}
