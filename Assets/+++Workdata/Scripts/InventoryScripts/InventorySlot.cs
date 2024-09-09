using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public StateInfo assignedSlot;
    public Item assignedItem;
    
    [HideInInspector]
    public StateInfo stateInfo;
    [HideInInspector]
    public InventoryManager inventoryManager;

    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI itemAmount;
    [SerializeField]
    private Toggle slotToggle;

    [SerializeField] 
    private GameObject slotBorder;

    public void Awake()
    {
        
    }

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }
    
    public void SetItem(Item newItem)
    {
        assignedItem = newItem;
        SetStateInfo(newItem.itemState);
        SetVisuals();
    }
    
    public void SetStateInfo(StateInfo stateInfo)
    {
        assignedSlot = this.stateInfo;
        this.stateInfo = stateInfo;
        SetVisuals();
    }
    
    public void ClearSlot()
    {
        assignedItem = null;
        stateInfo = new StateInfo();
        SetVisuals();
    }

    
    public void SetVisuals()
    {
        if (assignedItem != null)
        {
            itemImage.sprite = assignedItem.itemState.icon;
            itemAmount.SetText(assignedItem.currentAmount.ToString());
            ChangeVisuals(true);
        }
        else
        {
            ChangeVisuals(false);
        }
    }
    
    
    
    public void ChangeVisuals(bool value)
    {
        slotToggle.interactable = value;
        itemImage.gameObject.SetActive(value);
        itemAmount.gameObject.SetActive(value);
        slotBorder.gameObject.SetActive(value);
    }

    public void RemoveItem()
    {
        assignedItem = null;
        ChangeVisuals(false);
    }

    public bool HasItem()
    {
        return assignedItem != null;
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

  
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (inventoryManager.HasCurrentItem())
        {
            //Rein laden
            SetItem(inventoryManager.GetCurrenItemInfo());
            inventoryManager.ClearSlot();
        }
        else
        {
            //Zwischen speichern 
            if (stateInfo.id != "")
            {
                inventoryManager.SetCurrentItem(assignedItem, this);
            }
        }
    }
}
