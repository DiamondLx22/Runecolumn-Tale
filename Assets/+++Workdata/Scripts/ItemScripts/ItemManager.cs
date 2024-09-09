using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ItemManager : MonoBehaviour
{

    private GameController gameController;
    //public StateInfo[] stateInfos;
    public List<StateInfo> stateInfos;

    public InventorySlot[] inventorySlots;

    [SerializeField] private TextMeshProUGUI ItemPopUpHeader, ItemPopUpDescription, ItemPopUpAmount;
    [SerializeField] private GameObject ItemPopUpCollected;
    [SerializeField] private Image image;
    

    public bool isStateContainerShown = false;
    //private void Awake()
    //{
    //    gameController = FindObjectOfType<GameController>();
    //}

    private void Start()
    {
        InitializeInventory();
    }

    private void InitializeInventory()
    {
        for (int i = 0; 1 < inventorySlots.Length; i++)
        {
            if (i < stateInfos.Count)
            {
                Item newItem = new Item(stateInfos[i], 1);
                inventorySlots[i].SetItem(newItem);
            }
        }
    }

    public Item GetItemById(string id)
    {
        foreach (StateInfo stateInfo in stateInfos)
        {
            if (stateInfo.id == id)
            {
                return new Item(stateInfo, 1);
            } 
        }

        return null;
    }

    private void OnEnable()
    {
        GameState.StateAdded += NewStateCollected;
    }


    private void OnDisable()
    {
        GameState.StateAdded -= NewStateCollected;
    }

    void NewStateCollected(string id, int amount)
    {
        foreach (StateInfo stateInfo in stateInfos)
        {
            if (stateInfo.id == id)
            {
                ItemPopUpHeader.SetText(stateInfo.itemName);
                ItemPopUpDescription.SetText(stateInfo.description);
                
                
                image.sprite = stateInfo.icon;
            }

        }

        print($"new Item collected with the id: {id} with the amount of");
        StartCoroutine(DelayOpenPanel());
    }

    IEnumerator DelayOpenPanel()
    {
        yield return null;
        ItemPopUpCollected.SetActive(true);
        Selectable newSelection;


        yield return null; //Wait for next Update() / next frame


        isStateContainerShown = true;
        //gameController.StartStatePopUp();
    }

    public void CloseStatePopUp()
    {
        ItemPopUpCollected.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        isStateContainerShown = false;
        //gameController.EndStatePopUpMode();
    }

    public StateInfo GetStateById(string id)
    {
        foreach (StateInfo stateInfo in stateInfos)
        {
            if (stateInfo.id == id)
            {
                return stateInfo;
            }
        }

        return null;
    }
}
