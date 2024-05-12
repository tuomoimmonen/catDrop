using UnityEngine;
using System;
using UnityEngine.Pool;

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
    [SerializeField] GameObject spawnerCloud;
    private Fruit currentFruit;
    //private Fruit mergedFruit;
    [SerializeField] Transform fruitsParent;


    //Vector2 fruitSpawnPosition;
    //public ObjectPool<Fruit> spawnableFruitsPool;
    //public ObjectPool<Fruit> mergeFruitsPool;

    [Header("Settings")]
    [SerializeField] Transform fruitsYspawnPosition; //y position for fruit
    private bool canSpawnFruit;
    private bool isControllingFruit;
    [SerializeField] float minMouseXpos, maxMouseXpos;

    [Header("Next fruit hint")]
    private int nextFruitIndex;
    //private int mergedFruitIndex;

    [Header("Debug")]
    [SerializeField] bool enableGizmos;

    [Header("Events")]
    public static Action onNextFruitIndexSet;
    public static Action<Fruit> onFruitSpawned;
    public static Action onPlayerInputDetected;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;

        //HideLine();
        //MoveFruitSpawnLine();
        canSpawnFruit = true;
        MergeManager.onMergeHandled += MergeCallback;
        ShopManager.onSkinSelected += SkinSelectedCallback;

    }

    private void OnDestroy()
    {
        MergeManager.onMergeHandled -= MergeCallback;
        ShopManager.onSkinSelected -= SkinSelectedCallback;
    }

    private void Start()
    {
        //skinData.InitializePools(10, 20, 10, 20);

        /*
        spawnableFruitsPool = new ObjectPool<Fruit>(() =>
        {
            return Instantiate(skinData.GetSpawnablePrefabs()[nextFruitIndex], GetSpawnPosition(), Quaternion.identity, fruitsParent);
        }, fruit =>
        {
            fruit.gameObject.SetActive(true);
        }, fruit =>
        {
            fruit.gameObject.SetActive(false);
        }, fruit =>
        {
            Destroy(fruit.gameObject);
        }, false, 10, 20);

        mergeFruitsPool = new ObjectPool<Fruit>(() =>
        {
            return Instantiate(skinData.GetObjectPrefabs()[mergedFruitIndex], fruitsParent);
        }, fruit =>
        {
            fruit.gameObject.SetActive(true);
        }, fruit =>
        {
            fruit.gameObject.SetActive(false);
        }, fruit =>
        {
            Destroy(fruit.gameObject);
        }, false, 10, 20);
        */

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

        //fruitSpawnPosition = GetSpawnPosition();
        //currentFruit = spawnableFruitsPool.Get();
        //currentFruit = GetNextSpawnableFruitIndex(nextFruitIndex);

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
        //return fruitPool.Get().name;
        //return currentFruit.gameObject.name;
    }

    public Sprite GetNextFruitImage()
    {
        return skinData.GetSpawnablePrefabs()[nextFruitIndex].GetFruitSprite();
        //return fruitPool.Get().GetFruitSprite();
        //return currentFruit.GetFruitSprite();
        //return firstSpawnableFruits[nextFruitIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }

    public Color GetNextFruitColor()
    {
        return skinData.GetSpawnablePrefabs()[nextFruitIndex].GetFruitColor();
        //return fruitPool.Get().GetFruitColor();
        //return currentFruit.GetFruitColor();

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
                //Debug.Log("small fruit added");
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
        onPlayerInputDetected?.Invoke();
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
        //HideLine();
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
        if(clickedWorldPosition.x <= minMouseXpos) { clickedWorldPosition.x = minMouseXpos; }
        else if(clickedWorldPosition.x >= maxMouseXpos) { clickedWorldPosition.x = maxMouseXpos; }
        clickedWorldPosition.y = fruitsYspawnPosition.position.y;
        return clickedWorldPosition;
    }

    private void MergeCallback(FruitType fruitType, Vector2 spawnPosition)
    {

        //mergedFruitIndex = 0;

        for (int i = 0; i < skinData.GetObjectPrefabs().Length; i++)
        {
            if (skinData.GetObjectPrefabs()[i].GetFruitType() == fruitType)
            {
                //mergedFruitIndex = (int)fruitType;
                SpawnMergedFruit(skinData.GetObjectPrefabs()[i], spawnPosition); break;
            }
            
            /*
            if (mergeFruitsPool.Get().GetFruitType() == fruitType)
            {
                mergedFruitIndex = i;
                SpawnMergedFruit(skinData.GetObjectPrefabs()[i], spawnPosition); break;
            }
            */


        }
    }

    private void SpawnMergedFruit(Fruit fruit, Vector2 spawnPosition)
    {
        Fruit fruitInstance = Instantiate(fruit, spawnPosition, Quaternion.identity, fruitsParent);
        fruitInstance.EnablePhysics();
        //mergedFruit = GetMergeableFruit(mergedFruitIndex);
        //mergedFruitIndex = index;
        //mergedFruit = mergeFruitsPool.Get();
        //mergedFruit.transform.position = spawnPosition;
        //mergedFruit.transform.parent = fruitsParent;
        //mergedFruit.EnablePhysics();
        //mergedFruitIndex = 0;

        //fruitInstance.HandleMergeText();
    }

    /*
    public Fruit GetMergeableFruit(int index)
    {
        if (mergeFruitsPool != null && index >= 0 && index < skinData.GetObjectPrefabs().Length)
        {
            Fruit fruit = mergeFruitsPool.Get();
            if (fruit != null)
            {
                Destroy(fruit.gameObject); // Destroy the previously pooled object
                fruit = Instantiate(skinData.GetObjectPrefabs()[index]);
            }
            return fruit;
        }
        else
        {
            Debug.LogError("MergeableFruitsPool is not initialized or index is out of range.");
            return null;
        }
    }
    */

    /*
    public Fruit GetNextSpawnableFruitIndex(int index)
    {
        if (spawnableFruitsPool != null && index >= 0 && index < skinData.GetSpawnablePrefabs().Length)
        {
            Fruit fruit = spawnableFruitsPool.Get();
            if (fruit != null)
            {
                Destroy(fruit.gameObject); // Destroy the previously pooled object
                fruit = Instantiate(skinData.GetSpawnablePrefabs()[index]);
            }
            return fruit;
        }
        else
        {
            Debug.LogError("MergeableFruitsPool is not initialized or index is out of range.");
            return null;
        }
    }
    */

    private void MoveFruitSpawnLine()
    {
        spawnerCloud.transform.position = GetSpawnPosition();
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
