using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public class Item
    {
        public StateInfo itemState;  // The static data for the item
        public int currentAmount;    // Current amount of the item (can be different from itemState.amount)
    
        // Constructor for creating an item with its state and initial amount
        public Item(StateInfo state, int amount)
        {
            itemState = state;
            currentAmount = amount;
        }
    
        // Example method to add more of the same item
        public void AddAmount(int amount)
        {
            currentAmount += amount;
        }

        // Example method to reduce the amount (when used, sold, etc.)
        public bool RemoveAmount(int amount)
        {
            if (currentAmount >= amount)
            {
                currentAmount -= amount;
                return true;
            }
            return false;
        }

        // Check if this item can stack with another item
        public bool CanStackWith(Item other)
        {
            return itemState.id == other.itemState.id;
        }
    }

