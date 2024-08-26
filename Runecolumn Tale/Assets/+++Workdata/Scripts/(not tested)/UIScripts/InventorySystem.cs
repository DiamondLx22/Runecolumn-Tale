using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
     // Die maximale Anzahl an Gegenständen im Inventar
    public int inventorySize = 20;

    // Das UI-Panel, das die Inventargegenstände anzeigt
    public GameObject inventoryPanel;

    // Die Vorlage für ein Inventargegenstands-UI-Element
    public GameObject inventoryItemPrefab;

    // Eine Liste, die die aktuellen Gegenstände im Inventar speichert
    private List<Item> items;

    // Die Struktur eines Gegenstands
    [System.Serializable]
    public class Item
    {
        public string itemName;  // Der Name des Gegenstands
        public Sprite itemIcon;  // Das Icon des Gegenstands
    }

    private void Start()
    {
        // Initialisiert die Liste der Gegenstände
        items = new List<Item>();

        // Initialisiert das Inventar mit leeren Gegenständen
        for (int i = 0; i < inventorySize; i++)
        {
            items.Add(null);
        }

        // Aktualisiert das UI, um den aktuellen Inventarstatus anzuzeigen
        UpdateInventoryUI();
    }

    // Fügt dem Inventar einen neuen Gegenstand hinzu
    public void AddItem(Item newItem)
    {
        // Findet den ersten leeren Platz im Inventar
        for (int i = 0; i < inventorySize; i++)
        {
            if (items[i] == null)
            {
                // Fügt den neuen Gegenstand hinzu und aktualisiert das UI
                items[i] = newItem;
                UpdateInventoryUI();
                return;
            }
        }
        Debug.Log("Inventar ist voll!");
    }

    // Entfernt einen Gegenstand aus dem Inventar
    public void RemoveItem(int index)
    {
        if (index >= 0 && index < inventorySize && items[index] != null)
        {
            // Entfernt den Gegenstand und aktualisiert das UI
            items[index] = null;
            UpdateInventoryUI();
        }
    }

    // Aktualisiert das Inventar-UI
    private void UpdateInventoryUI()
    {
        // Löscht alle bestehenden UI-Elemente
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Fügt UI-Elemente für alle Gegenstände im Inventar hinzu
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject itemUI = Instantiate(inventoryItemPrefab, inventoryPanel.transform);
            if (items[i] != null)
            {
                // Setzt das Icon und den Namen des Gegenstands
                itemUI.transform.Find("ItemIcon").GetComponent<Image>().sprite = items[i].itemIcon;
                itemUI.transform.Find("ItemName").GetComponent<Text>().text = items[i].itemName;
            }
            else
            {
                // Setzt ein leeres Icon und einen leeren Namen, wenn der Platz leer ist
                itemUI.transform.Find("ItemIcon").GetComponent<Image>().sprite = null;
                itemUI.transform.Find("ItemName").GetComponent<Text>().text = "";
            }
        }
    }
}
