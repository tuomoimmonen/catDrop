using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] SkinButton skinButtonPrefab;
    [SerializeField] Transform skinButtonParent;
    [SerializeField] GameObject purchaseButton;
    [SerializeField] TMP_Text selectedSkinText;
    [SerializeField] TMP_Text skinPriceText;

    [Header("Data")]
    [SerializeField] SkinDataSO[] skinDataSO;
    private bool[] unlockedStates;
    private const string skinButtonKey = "skinButtonKey_";
    private const string lastSelectedSkinKey = "lastSelectedSkin_";

    [Header("Events")]
    public static Action<SkinDataSO> onSkinSelected;

    [Header("Local variables")]
    private int lastSelectedSkin;


    private void Awake()
    {
        unlockedStates = new bool[skinDataSO.Length];

    }   
    void Start()
    {
        Initialize();
        LoadData();
    }

    private void LoadData()
    {
        for (int i = 0; i < unlockedStates.Length; i++)
        {
            int unlockedValue = PlayerPrefs.GetInt(skinButtonKey + i, 0);

            //1st is unlocked from the start
            if(i == 0) { unlockedValue = 1; }

            if(unlockedValue == 1)
            {
                unlockedStates[i] = true;
            }
        }

        LoadLastSelectedSkin();

    }

    public void PurchaseButtonCallback()
    {
        //remove the coins
        CoinManager.instance.AddCoins(-skinDataSO[lastSelectedSkin].GetPrice());

        //check for coins
        /*
        if (!CoinManager.instance.CanPurchase(skinDataSO[lastSelectedSkin].GetPrice()))
        {
            return;
        }
        */

        //unlock the skin
        unlockedStates[lastSelectedSkin] = true;

        SaveData();

        //to trigger the event and update the purchase button (hide)
        SkinButtonClickedCallback(lastSelectedSkin);
    }

    private void SaveData()
    {
        for (int i = 0; i < unlockedStates.Length; i++)
        {
            int unlockedValue = unlockedStates[i] ? 1 : 0;
            PlayerPrefs.SetInt(skinButtonKey + i, unlockedValue);
        }
    }
    private void Initialize()
    {
        //hide purchase button, 1st is unlocked
        purchaseButton.SetActive(false);

        //spawn the buttons
        for (int i = 0; i < skinDataSO.Length; i++)
        {
            SkinButton skinButtonInstance = Instantiate(skinButtonPrefab, skinButtonParent);

            skinButtonInstance.Configure(skinDataSO[i].GetObjectPrefabs()[0].GetFruitSprite(), skinDataSO[i].GetObjectPrefabs()[0].GetFruitColor());

            /*
            //make the first one selected
            if(i == 0) { skinButtonInstance.Select(); }
            */

            //get button, add listener
            int j = i; //j = clicked button index
            skinButtonInstance.GetSkinButton().onClick.AddListener(() => SkinButtonClickedCallback(j));
        }
    }

    private void SkinButtonClickedCallback(int skinButtonIndex, bool shouldSaveLastSkin = true)
    {
        lastSelectedSkin = skinButtonIndex;

        for (int i = 0; i < skinButtonParent.childCount; i++)
        {
            SkinButton currentSkinButton = skinButtonParent.GetChild(i).GetComponent<SkinButton>();

            if(i == skinButtonIndex)
            {
                currentSkinButton.Select();
            }
            else
            {
                currentSkinButton.DeSelect();
            }
        }

        if(IsSkinUnlocked(skinButtonIndex))
        {
            onSkinSelected?.Invoke(skinDataSO[skinButtonIndex]);
            if(shouldSaveLastSkin)
            {
                SaveLastSelectedSkin();
            }
        }

        ManagePurchaseButtonVisibility(skinButtonIndex);

        UpdateSelectedSkinText(skinButtonIndex);
    }

    private void UpdateSelectedSkinText(int skinButtonIndex)
    {
        selectedSkinText.text = skinDataSO[skinButtonIndex].GetName();
    }

    private void LoadLastSelectedSkin()
    {
        int lastSelectedSkin = PlayerPrefs.GetInt(lastSelectedSkinKey,0);

        SkinButtonClickedCallback(lastSelectedSkin, false);
    }
    private void SaveLastSelectedSkin()
    {
        PlayerPrefs.SetInt(lastSelectedSkinKey, lastSelectedSkin);
    }

    private bool IsSkinUnlocked(int skinButtonIndex) { return unlockedStates[skinButtonIndex]; }

    private void ManagePurchaseButtonVisibility(int skinButtonIndex)
    {
        bool canPurchase = CoinManager.instance.CanPurchase(skinDataSO[skinButtonIndex].GetPrice());
        purchaseButton.GetComponent<Button>().interactable = canPurchase;

        purchaseButton.SetActive(!IsSkinUnlocked(skinButtonIndex));

        //update the skin price text
        skinPriceText.text = skinDataSO[(skinButtonIndex)].GetPrice().ToString();

        /*
        if (unlockedStates[skinButtonIndex])
        {
            purchaseButton.SetActive(false);
        }
        else
        {
            purchaseButton.SetActive(true);
        }
        */
    }
}
