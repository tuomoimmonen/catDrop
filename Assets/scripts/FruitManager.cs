using UnityEngine;
using System;

using Random = UnityEngine.Random;
using System.Collections.Generic;

public class FruitManager : MonoBehaviour
{
    public static FruitManager instance;

    [Header("Elements")]
    //[SerializeField] Fruit[] fruitPrefabs;
    //[SerializeField] Fruit[] firstSpawnableFruits;
    [SerializeField] SkinDataSO skinData;
    [SerializeField] Camera cam; //main camera
    [SerializeField] LineRenderer fruitSpawnLine;
    private Fruit currentFruit;
    [SerializeField] Transform fruitsParent;

    [Header("Settings")]
    [SerializeField] Transform fruitsYspawnPosition; //y position for fruit
    private bool canSpawnFruit;
    private bool isControllingFruit;

    [Header("Next fruit hint")]
    private int nextFruitIndex;

    [Header("Debug")]
    [SerializeField] bool enableGizmos;

    [Header("Events")]
    public static Action onNextFruitIndexSet;
    public static Action<Fruit> onFruitSpawned;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;

        HideLine();
        canSpawnFruit = true;
        MergeManager.onMergeHandled += MergeCallback;
        ShopManager.onSkinSelected += SkinSelectedCallback;
    }

    private void OnDisable()
    {
        MergeManager.onMergeHandled -= MergeCallback;
        ShopManager.onSkinSelected -= SkinSelectedCallback;
    }

    private void Start()
    {
        SetNextFruitIndex();
    }

    void Update()
    {
        if(!GameManager.instance.IsGameState()) { return; }

        if(canSpawnFruit)
        {
            ManagePlayerInput();
        }
    }

    private void SkinSelectedCallback(SkinDataSO selectedSkin)
    {
        skinData = selectedSkin;    
    }

    private void ManagePlayerInput()
    {
        if (Input.GetMouseButtonDown(0)) //first touch
        {
            MouseDownCallback();
        }

        else if (Input.GetMouseButton(0)) //drag touch
        {
            if (isControllingFruit)
                MouseDragCallback();
            else
                MouseDownCallback();
        }

        else if (Input.GetMouseButtonUp(0) && isControllingFruit) //remove touch
        {
            MouseUpCallback();
            
        }

    }

    private void SpawnFruit()
    {
        //y position for fruit spawning
        Vector2 spawnPosition = GetSpawnPosition();
        //spawnPosition.y = fruitsYspawnPosition.position.y;

        currentFruit = Instantiate(skinData.GetSpawnablePrefabs()[nextFruitIndex], spawnPosition, Quaternion.identity, fruitsParent);

        SetNextFruitIndex();

        onFruitSpawned?.Invoke(currentFruit);
    }

    private void SetNextFruitIndex()
    {
        nextFruitIndex = Random.Range(0, skinData.GetSpawnablePrefabs().Length);

        onNextFruitIndexSet?.Invoke();
    }

    public string GetFruitName()
    {
        return skinData.GetSpawnablePrefabs()[nextFruitIndex].name;
    }

    public Sprite GetNextFruitImage()
    {
        return skinData.GetSpawnablePrefabs()[nextFruitIndex].GetFruitSprite();
        //return firstSpawnableFruits[nextFruitIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }

    public Color GetNextFruitColor()
    {
        return skinData.GetSpawnablePrefabs()[nextFruitIndex].GetFruitColor();
    }

    public Fruit[] GetSmallFruitsForPowerUp()
    {
        List<Fruit> smallFruits = new List<Fruit>();

        for (int i = 0; i < fruitsParent.childCount; i++)
        {
            Fruit fruit = fruitsParent.GetChild(i).GetComponent<Fruit>();

            FruitType fruitType = fruit.GetFruitType();
            int fruitTypeInt = (int)fruitType;

            //what fruits to blast here
            if (fruitTypeInt <= 3)
            {
                smallFruits.Add(fruit);
            }
            /*
            if(fruitType == FruitType.Blueberry || fruitType == FruitType.Cherry || fruitType == FruitType.Plum || fruitType == FruitType.Peach)
            {
                smallFruits.Add(fruit);
            }
            */
        }

        return smallFruits.ToArray();
    }

    private void MouseDownCallback()
    {
        if (!IsClickDetected()) { return; }

        ShowLine();
        MoveFruitSpawnLine();
        SpawnFruit();

        isControllingFruit = true;
    }
    
    private bool IsClickDetected()
    {
        Vector2 mousePosition = Input.mousePosition;

        return mousePosition.y > Screen.height / 4;
    }

    private void MouseDragCallback()
    {
        MoveFruitSpawnLine();
        currentFruit.MoveTo(GetSpawnPosition());
    }

    private void MouseUpCallback()
    {
        HideLine();
        canSpawnFruit = false;
        isControllingFruit = false;
        if(currentFruit != null)
        {
            currentFruit.EnablePhysics();
        }
        StartControlTimer();
    }

    private Vector2 GetClickedWorldPosition()
    {
        //2d requires ortho camera !!
        //Debug.Log(cam.ScreenToWorldPoint(Input.mousePosition));
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 clickedWorldPosition = GetClickedWorldPosition();
        clickedWorldPosition.y = fruitsYspawnPosition.position.y;
        return clickedWorldPosition;
    }

    private void MergeCallback(FruitType fruitType, Vector2 spawnPosition)
    {
        for (int i = 0; i < skinData.GetObjectPrefabs().Length; i++)
        {
            if (skinData.GetObjectPrefabs()[i].GetFruitType() == fruitType)
            {
                SpawnMergedFruit(skinData.GetObjectPrefabs()[i], spawnPosition); break;
            }
        }
    }

    private void SpawnMergedFruit(Fruit fruit, Vector2 spawnPosition)
    {
        Fruit fruitInstance = Instantiate(fruit, spawnPosition, Quaternion.identity, fruitsParent);
        fruitInstance.EnablePhysics();
    }

    private void MoveFruitSpawnLine()
    {
        fruitSpawnLine.SetPosition(0, GetSpawnPosition());
        fruitSpawnLine.SetPosition(1, GetSpawnPosition() + Vector2.down * 15f); //hard coded length
    }

    private void HideLine()
    {
        fruitSpawnLine.enabled = false;
    }

    private void ShowLine()
    {
        fruitSpawnLine.enabled = true;
    }

    private void StartControlTimer()
    {
        Invoke("StopControlTimer", 0.5f);
    }

    private void StopControlTimer()
    {
        canSpawnFruit = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!enableGizmos) {  return; }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-50, fruitsYspawnPosition.position.y, 0), new Vector3(50, fruitsYspawnPosition.position.y, 0));
    }
#endif

}
