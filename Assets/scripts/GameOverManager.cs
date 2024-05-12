using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject gameOverLine;
    [SerializeField] Transform fruitParents;

    [Header("Timer")]
    [SerializeField] float durationBeforeGameOver;
    private float timer;
    private bool timerOn;

    void Update()
    {
        ManageGameOver();
    }

    private void ManageGameOver()
    {
        if (timerOn)
        {
            ManageTimerOn();
        }
        else
        {
            if (IsFruitAboveLine())
            {
                StartTimer();
            }
        }
    }
    private void ManageTimerOn()
    {
        timer += Time.deltaTime;
        if (!IsFruitAboveLine()) { StopTimer(); }

        if (timer >= durationBeforeGameOver)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        GameManager.instance.SetGameOverState();
    }

    private bool IsFruitAboveLine()
    {
        for (int i = 0; i < fruitParents.childCount; i++)
        {
            Fruit fruit = fruitParents.transform.GetChild(i).GetComponent<Fruit>();
            if (fruit.FruitHasCollided())
            {
                if (IsFruitAboveGameOverLine(fruitParents.GetChild(i)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsFruitAboveGameOverLine(Transform fruit)
    {
        if(fruit.position.y > gameOverLine.transform.position.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StartTimer()
    {
        timer = 0;
        timerOn = true;
    }

    private void StopTimer()
    {
        timerOn = false;
    }
}
