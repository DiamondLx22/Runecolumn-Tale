using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    public StateCategorys targetCategory;
    public string requiredItemId;
    public Animator[] animators;

    public GameObject animationParent;
    
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

            //var currentItem = InventoryManager.GetCurrentItemInfo();
//
            //if (currentItem.itemState.category == targetCategory)
            //{
            //    if (assignedItem == currentItem) return;
            //    SetItem(currentItem);
            //    inventoryManager.ClearSlot();
            //    
            //    if (currentItem.itemState.id == requiredItemId)
            //    {
            //        PlayAnimations();
            //    }
            //    
            //    else
            //    {
            //        StopAnimations();
            //    }
            //}
        }
        else
        {
            //Zwischen speichern 
            if (stateInfo.id != "")
            {
                inventoryManager.SetCurrentItem(assignedItem, this);
            }

            StopAnimations();
        }
    }

    private void PlayAnimations()
    {
        foreach (var animator in animators)
        {
            animator.enabled = true;
            animator.Play("AnimationName", -1, 0f);
        }
    }

    private void StopAnimations()
    {
        if (animationParent != null)
        {
            Animator[] parentAnimators = animationParent.GetComponentsInChildren<Animator>();
            foreach (var animator in parentAnimators)
            {
                animator.enabled = false;
            }
        }
        else
        {
            foreach (var animator in animators)
            {
                animator.enabled = false;
            }
        }
    }
}
