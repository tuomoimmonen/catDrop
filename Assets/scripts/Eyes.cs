using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Transform rightEye;
    [SerializeField] Transform leftEye;
    [SerializeField] Transform rightPupil;
    [SerializeField] Transform leftPupil;

    [Header("Settings")]
    [SerializeField] float pupilMaxMoveDistance;

    [Header("Data")]
    private Transform lastFruitTransform;

    private void Awake()
    {
        FruitManager.onFruitSpawned += FruitSpawnedCallback;
    }

    private void OnDisable()
    {
        FruitManager.onFruitSpawned -= FruitSpawnedCallback;
    }

    void Update()
    {
        if(lastFruitTransform != null)
        {
            MoveEyes();
        }
    }

    private void MoveEyes()
    {
        Vector3 targetPos = lastFruitTransform.position;

        Vector3 rightEyePupilDirection = (targetPos-rightEye.position).normalized;
        Vector3 rightPupilTargetLocalPosition = rightEyePupilDirection * pupilMaxMoveDistance;

        rightPupil.localPosition = rightPupilTargetLocalPosition;

        Vector3 leftEyePupilDirection = (targetPos - leftEye.position).normalized;
        Vector3 leftPupilTargetLocalPosition = leftEyePupilDirection * pupilMaxMoveDistance;

        leftPupil.localPosition = leftPupilTargetLocalPosition;
    }

    private void FruitSpawnedCallback(Fruit fruit)
    {
        lastFruitTransform = fruit.transform;
    }
}
