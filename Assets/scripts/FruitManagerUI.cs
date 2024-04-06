using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FruitManager))]
public class FruitManagerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] TMP_Text nextFruitHintText;
    [SerializeField] Image nextFruitImage;
    private FruitManager fruitManager;

    private void Awake()
    {
        fruitManager = GetComponent<FruitManager>();
        FruitManager.onNextFruitIndexSet += UpdateNextFruitHint;
    }

    private void OnDisable()
    {
        FruitManager.onNextFruitIndexSet -= UpdateNextFruitHint;
    }

    private void UpdateNextFruitHint()
    {
        nextFruitHintText.text = fruitManager.GetFruitName();
        nextFruitImage.sprite = fruitManager.GetNextFruitImage();
    }
}
