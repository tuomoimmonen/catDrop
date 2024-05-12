using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleTweenController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] float endPositionValue;
    [SerializeField] float durationToMove;

    void Start()
    {
        transform.DOLocalMoveX(endPositionValue, durationToMove).SetEase(Ease.OutBounce);
    }
}
