using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private GameState gameState;
    private StateManager stateManager;

    [SerializeField]
    private InventorySlot[] InventorySlots;

    [Header("Item Description")]
    [SerializeField] private GameObject itemDescriptionContainer;
    [SerializeField] private TextMeshProUGUI itemHeaderText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Image itemImage;

    private void Awake()
    {
        gameState = FindObjectOfType<GameState>();
        stateManager = FindObjectOfType<StateManager>();
    }

    public void RefreshInventory()
    {
        List<State> currentStateList = gameState.GetStateList();

        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (currentStateList.Count == 0)
            {
                itemDescriptionContainer.SetActive(false);
                InventorySlots[i].TurnOffBorder();

            }

            if (i < currentStateList.Count)
            {
                StateInfo newStateInfo = stateManager.GetStateById(currentStateList[i].id);
                newStateInfo.amount = currentStateList[i].amount;
                InventorySlots[i].SetStateInfo(newStateInfo);
            }

            else
            {
                InventorySlots[i].ChangeVisuals(false);
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
