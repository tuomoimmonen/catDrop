using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] TMP_Text coinText;
    public void UpdateText(string text)
    {
        coinText.text = text;
    }
}
