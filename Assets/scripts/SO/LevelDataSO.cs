using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelData", menuName = "SO Level Data", order = 1)]
public class LevelDataSO : ScriptableObject
{

    [Header("Data")]
    [SerializeField] GameObject levelPrefab;
    [SerializeField] int requiredHighScore;

    public GameObject GetLevel() => levelPrefab;

    public int GetRequiredHighScore() => requiredHighScore;

}
