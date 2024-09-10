using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    public StateCategorys targetCategory; 
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (inventoryManager.HasCurrentItem())
        {
            //Rein laden
            if (inventoryManager.GetCurrenItemInfo().itemState.category == targetCategory)
            {
                if(assignedItem == inventoryManager.GetCurrenItemInfo()) return;
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
