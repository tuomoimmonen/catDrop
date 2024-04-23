using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Image iconImage;
    [SerializeField] GameObject selectionOutline;
    [SerializeField] Button skinButton;

    public void Configure(Sprite sprite, Color color)
    {
        iconImage.sprite = sprite;
        iconImage.color = color;
    }

    public void Select() => selectionOutline.SetActive(true);
    public void DeSelect() => selectionOutline.SetActive(false);

    public Button GetSkinButton() { return skinButton; }
}
