using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


[CreateAssetMenu(fileName = "SkinData", menuName = "SO/SkinData", order = 0)]
public class SkinDataSO : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] new string name;
    [SerializeField] int skinPrice;

    [Header("Data")]
    [SerializeField] Fruit[] objectPrefabs;
    [SerializeField] Fruit[] spawnablePrefabs;


    public int GetPrice() {  return skinPrice; }
    public String GetName()
    {
        return name;
    }
    public Fruit[] GetObjectPrefabs()
    {
        return objectPrefabs;
    }

    public Fruit[] GetSpawnablePrefabs()
    {
        return spawnablePrefabs;
    }
}
