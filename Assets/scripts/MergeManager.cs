using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting.FullSerializer;
using TMPro;

public class MergeManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] TMP_Text comboText;

    [Header("Settings")]
    Fruit lastSender;
    public int comboCounter = 0;
    public float timer;

    [Header("Events")]
    public static Action<FruitType, Vector2> onMergeHandled;
    void Start()
    {
        Fruit.onFruitCollision += FruitCollisionCallback;
    }

    private void Update()
    {
        if (!GameManager.instance.IsGameState()) { return; }
        //timer += Time.deltaTime;
        //ComboStarted();

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

        //comboCounter++;
        
        //Debug.Log("fruit collision" + sender.name);
    }

    private void HandleMerge(Fruit sender, Fruit otherFruit)
    {
        FruitType mergedFruitType = sender.GetFruitType();
        mergedFruitType += 1;
        //if(mergedFruitType > FruitType.Watermelon) { return; }

        //in the middle of transforms
        Vector2 mergedSpawnPosition = (sender.transform.position + otherFruit.transform.position) / 2;

        //Destroy(sender.gameObject);
        //Destroy(otherFruit.gameObject);
        sender.HandleMergeText();
        sender.HandleMergeParticles();
        otherFruit.HandleMergeParticles();
        //CoinManager.instance.AddCoins((int)mergedFruitType);
        //one frame pause
        StartCoroutine(ResetLastSender());

        onMergeHandled?.Invoke(mergedFruitType, mergedSpawnPosition);
    }

    IEnumerator ResetLastSender()
    {
        yield return new WaitForEndOfFrame();
        lastSender = null;
    }
}
