using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SkinData", menuName = "SO/SkinData", order = 0)]
public class SkinDataSO : ScriptableObject
{
    [SerializeField] Fruit[] objectPrefabs;
    [SerializeField] Fruit[] spawnablePrefabs;

    public Fruit[] GetObjectPrefabs()
    {
        return objectPrefabs;
    }

    public Fruit[] GetSpawnablePrefabs()
    {
        return spawnablePrefabs;
    }
}
