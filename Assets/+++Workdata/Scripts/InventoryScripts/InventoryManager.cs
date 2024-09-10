                                                    using System;
                                                    using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> inventorySlots;
    public ItemManager itemManager;
    
    private GameState gameState;
    private ItemManager stateManager;

    public Item currentItem;
    private InventorySlot currentSlot;

    //[SerializeField]
    //private InventorySlot[] InventorySlots;

    [Header("Item Description")]
    [SerializeField] private GameObject itemDescriptionContainer;
    [SerializeField] private TextMeshProUGUI itemHeaderText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemImage;

   private void Awake() 
   { 
       gameState = FindObjectOfType<GameState>();
       stateManager = FindObjectOfType<ItemManager>();
   }

   private void Start()
   {
      /* for (int i = 0; i < inventorySlots.Count; i++)
       {
           if (i < itemManager.stateInfos.Count)
           {
               Item newItem = itemManager.GetItemById(itemManager.stateInfos[i].id);
               inventorySlots[i].SetItem(newItem);
           }
       }*/
   }
   
   public Item GetItemInSlot(int slotIndex)
   {
       if (slotIndex < inventorySlots.Count)
       {
           return inventorySlots[slotIndex].assignedItem;
       }
       return null;
   }

   public void SetCurrentItem(Item info, InventorySlot slot)
   {
       currentItem = info;
       currentSlot = slot;
   }

   public bool HasCurrentItem()
   {
       return currentItem.currentAmount == 0 ? false : true;
   }
   
   public Item GetCurrenItemInfo()
   {
       return currentItem;
   }

   public void ClearSlot()
   {
       currentSlot.ClearSlot();
       currentSlot = null;
       currentItem = new Item(new StateInfo(), 0);
   }

public void RefreshInventory() 
 {
     List<State> currentStateList = gameState.GetStateList();

     for (int i = 0; i < inventorySlots.Count; i++)
     {
         if (currentStateList.Count == 0)
         {
             itemDescriptionContainer.SetActive(false);
             //inventorySlots[i].TurnOffBorder();
             
         }
         
         if (i < currentStateList.Count)
         {
             StateInfo stateInfo = stateManager.GetStateById(currentStateList[i].id);
             Item newItem = new Item(stateInfo, 0);
             newItem.currentAmount = currentStateList[i].amount;
             inventorySlots[i].SetItem(newItem);
         }

         else 
         {
             inventorySlots[i].ChangeVisuals(false);
         }

     }
 }

    public void ShowItemDescription(StateInfo stateInfo)
    {
        itemDescriptionContainer.SetActive(true);
        itemHeaderText.SetText(stateInfo.itemName);
        itemDescriptionText.SetText(stateInfo.description);
        itemImage.sprite = stateInfo.icon;
    }
}
