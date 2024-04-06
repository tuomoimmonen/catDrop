using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class FruitManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Fruit[] fruitPrefabs;
    [SerializeField] Fruit[] firstSpawnableFruits;
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

    private void Awake()
    {
        HideLine();
        canSpawnFruit = true;
        MergeManager.onMergeHandled += MergeCallback;
    }

    private void OnDisable()
    {
        MergeManager.onMergeHandled -= MergeCallback;
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

        currentFruit = Instantiate(firstSpawnableFruits[nextFruitIndex], spawnPosition, Quaternion.identity, fruitsParent);

        SetNextFruitIndex();
    }

    private void SetNextFruitIndex()
    {
        nextFruitIndex = Random.Range(0, firstSpawnableFruits.Length);

        onNextFruitIndexSet?.Invoke();
    }

    public string GetFruitName()
    {
        return firstSpawnableFruits[nextFruitIndex].name;
    }

    public Sprite GetNextFruitImage()
    {
        return firstSpawnableFruits[nextFruitIndex].GetFruitSprite();
        //return firstSpawnableFruits[nextFruitIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }

    private void MouseDownCallback()
    {
        ShowLine();
        MoveFruitSpawnLine();
        SpawnFruit();

        isControllingFruit = true;
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
        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            if (fruitPrefabs[i].GetFruitType() == fruitType)
            {
                SpawnMergedFruit(fruitPrefabs[i], spawnPosition); break;
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
