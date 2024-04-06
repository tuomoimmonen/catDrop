using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MergeManager : MonoBehaviour
{
    Fruit lastSender;

    [Header("Events")]
    public static Action<FruitType, Vector2> onMergeHandled;
    void Start()
    {
        Fruit.onFruitCollision += FruitCollisionCallback;
    }

    private void OnDisable()
    {
        Fruit.onFruitCollision -= FruitCollisionCallback;
    }
    private void FruitCollisionCallback(Fruit sender, Fruit otherFruit)
    {
        
        if(lastSender != null) { return; }

        lastSender = sender;

        HandleMerge(sender, otherFruit);
        
        //Debug.Log("fruit collision" + sender.name);
    }

    private void HandleMerge(Fruit sender, Fruit otherFruit)
    {
        FruitType mergedFruitType = sender.GetFruitType();
        mergedFruitType += 1;

        Vector2 mergedSpawnPosition = (sender.transform.position + otherFruit.transform.position) / 2;

        //Destroy(sender.gameObject);
        //Destroy(otherFruit.gameObject);

        sender.HandleMerge();
        otherFruit.HandleMerge();

        StartCoroutine(ResetLastSender());

        onMergeHandled?.Invoke(mergedFruitType, mergedSpawnPosition);
    }

    IEnumerator ResetLastSender()
    {
        yield return new WaitForEndOfFrame();
        lastSender = null;
    }
}
