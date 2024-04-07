using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadlineDisplay : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject gameoverLine;
    [SerializeField] Transform fruitParent;

    private void Awake()
    {
        GameManager.onGameStateChanged += OnGameStateChangedCallback;
    }

    private void OnDisable()
    {
        GameManager.onGameStateChanged -= OnGameStateChangedCallback;
    }

    private void OnGameStateChangedCallback(GameState state)
    {
        if (state == GameState.Game)
        {
            StartCheckingForNearbyFruits();
        }
        else
        {
            StopCheckingForNearbyFruits();
        }
    }

    IEnumerator CheckForNearbyFruitsCoroutine()
    {
        HideDeadline();

        for (int i = 0; i < fruitParent.childCount; i++)
        {
            if (!fruitParent.GetChild(i).GetComponent<Fruit>().FruitHasCollided())
                continue;

            float distance = Mathf.Abs(fruitParent.GetChild(i).transform.position.y - gameoverLine.transform.position.y);

            if (distance <= 1)
            {
                ShowDeadline();
                break;
            }
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(CheckForNearbyFruitsCoroutine());
    }

    private void StartCheckingForNearbyFruits()
    {
        StartCoroutine(CheckForNearbyFruitsCoroutine());

    }

    private void StopCheckingForNearbyFruits()
    {
        HideDeadline();
        StopCoroutine(CheckForNearbyFruitsCoroutine());
    }

    private void HideDeadline() => gameoverLine.SetActive(false);

    private void ShowDeadline() => gameoverLine.SetActive(true);

}
