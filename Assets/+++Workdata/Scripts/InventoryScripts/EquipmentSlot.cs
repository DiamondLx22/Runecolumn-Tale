using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    public StateCategorys targetCategory; 
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (base.inventoryManager.HasCurrentItem())
        {
            //Rein laden
            if (inventoryManager.GetCurrenItemInfo().itemState.category == targetCategory)
            {
                SetItem(inventoryManager.GetCurrenItemInfo());
                
                inventoryManager.ClearSlot();
            }
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
