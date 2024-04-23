using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] TMP_Text levelText;
    [SerializeField] Button button;

    void Start()
    {
        GetComponent<Image>().color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.8f, 1f);
    }

    public void Configure(int levelIndex)
    {
        levelText.text = levelIndex.ToString();
    }

    public Button GetButton() => button;

    public void Enable() => button.interactable = true;

}
