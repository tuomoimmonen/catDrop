using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject fruitPrefab;
    [SerializeField] Camera cam; //main camera
    [SerializeField] LineRenderer fruitSpawnLine;
    private GameObject currentFruit;

    [Header("Settings")]
    [SerializeField] Transform fruitsYspawnPosition; //y position for fruit

    [Header("Debug")]
    [SerializeField] bool enableGizmos;

    private void Awake()
    {
        HideLine();
    }

    void Update()
    {
        ManagePlayerInput();
    }

    private void ManagePlayerInput()
    {
        if (Input.GetMouseButtonDown(0)) //first touch
        {
            MouseDownCallback();
        }
        else if (Input.GetMouseButton(0)) //drag touch
        {
            MouseDragCallback();
        }
        else if (Input.GetMouseButtonUp(0)) //remove touch
        {
            MouseUpCallback();
            currentFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

    }

    private void SpawnFruit()
    {
        //y position for fruit spawning
        Vector2 spawnPosition = GetSpawnPosition();
        //spawnPosition.y = fruitsYspawnPosition.position.y;

        currentFruit = Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);
    }

    private void MouseDownCallback()
    {
        ShowLine();
        MoveFruitSpawnLine();
        SpawnFruit();
    }

    private void MouseDragCallback()
    {
        MoveFruitSpawnLine();
        currentFruit.transform.position = GetSpawnPosition();
    }

    private void MouseUpCallback()
    {
        HideLine();
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!enableGizmos) {  return; }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-50, fruitsYspawnPosition.position.y, 0), new Vector3(50, fruitsYspawnPosition.position.y, 0));
    }
#endif

}
