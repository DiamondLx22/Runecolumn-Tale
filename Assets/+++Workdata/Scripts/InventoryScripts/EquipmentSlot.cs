using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    public StateCategorys targetCategory;
    public Animator[] animators;

    public GameObject animationParent;

    // Dictionary to map item IDs to their respective animation names
    private Dictionary<string, List<string>> itemAnimations = new Dictionary<string, List<string>>()
    {
        {
            "item_amber_staff", new List<string>
            {
                { "Player_Staff1_Attack-Up" },
                { "Player_Staff1_Attack-Down" },
                { "Player_Staff1_Attack-Left" },
                { "Player_Staff1_Attack-Right" },
                { "Weapon_Staff1_Attack-Up" },
                { "Weapon_Staff1_Attack-Down" },
                { "Weapon_Staff1_Attack-Left" },
                { "Weapon_Staff1_Attack-Right" },
                { "Weapon_Staff1_AttackEffect-Up" },
                { "Weapon_Staff1_AttackEffect-Down" },
                { "Weapon_Staff1_AttackEffect-Left" },
                { "Weapon_Staff1_AttackEffect-Right" },
                { "Projectile_Shoot-Up" },
                { "Projectile_Shoot-Down" },
                { "Projectile_Shoot-Left" },
                { "Projectile_Shoot-Right" }
            }


            // Weitere IDs und Animationen hinzufügen
        }
    };

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (inventoryManager.HasCurrentItem())
        {
            // Lade das Item in den Slot
            if (inventoryManager.GetCurrenItemInfo().itemState.category == targetCategory)
            {
                if (assignedItem == inventoryManager.GetCurrenItemInfo()) return;
                SetItem(inventoryManager.GetCurrenItemInfo());
                inventoryManager.ClearSlot();
            }

            var currentItem = assignedItem;

            if (currentItem != null && currentItem.itemState.category == targetCategory)
            {
                if (itemAnimations.ContainsKey(currentItem.itemState.id))
                {
                    PlayAnimations(currentItem.itemState.id);
                }
                else
                {
                    StopAnimations();
                }
            }
        }
        else
        {
            // Item zwischenspeichern und Animation stoppen
            if (assignedItem != null && assignedItem.itemState.id != "")
            {
                inventoryManager.SetCurrentItem(assignedItem, this);
            }

            StopAnimations();
        }
    }

    private void PlayAnimations(string itemId)
    {
        List<string> animationNames;
        if (itemAnimations.TryGetValue(itemId, out animationNames))
        {
            foreach (var animator in animators)
            {
                animator.enabled = true;
                foreach (var animationName in animationNames)
                {
                    animator.Play(animationName, -1, 0f); // Spiele jede spezifische Animation
                }
            }
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