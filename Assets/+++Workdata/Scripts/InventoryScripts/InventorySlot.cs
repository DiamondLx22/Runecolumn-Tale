using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class InventorySlot : MonoBehaviour
{
    private StateInfo stateInfo;
    private InventoryManager inventoryManager;

    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI itemAmount;
    [SerializeField]
    private Toggle slotToggle;

    [SerializeField] 
    private GameObject slotBorder;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }
    public void SetStateInfo(StateInfo stateInfo) 
    {
        this.stateInfo = stateInfo;
        SetVisuals();
    }

    public void SetVisuals()
    {
        ChangeVisuals(true);
        itemImage.sprite = stateInfo.icon;
        itemAmount.SetText(stateInfo.amount.ToString());
    }

    public void ChangeVisuals(bool value)
    {
        slotToggle.interactable = value;
        itemImage.gameObject.SetActive(value);
        itemAmount.gameObject.SetActive(value);
        slotBorder.gameObject.SetActive(value);
    }
    
    public void TurnOffBorder()
    {
        StartCoroutine(InitiateTurnOffBorder());
    }

    IEnumerator InitiateTurnOffBorder()
    {
        yield return null;
        slotBorder.SetActive(false);
    }

    public void ShowDescription()
    {
        if (slotToggle.isOn)
        {
            inventoryManager.ShowItemDescription(stateInfo);
        }
    }

}
