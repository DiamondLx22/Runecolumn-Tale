using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    private Player_InputActions inputActions;
    private InputAction inventoryAction;
    private InventoryManager inventoryManager;
    private PlayerMovement playerMovement;

    [SerializeField] private GameObject inventoryContainer;

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

        inputActions = new Player_InputActions();
        inventoryAction = inputActions.UI.Inventory;
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnEnable()
    {
        EnableInput();
        inventoryAction.performed += Inventory;
    }

    private void OnDisable()
    {
        DisableInput();
        inventoryAction.performed -= Inventory;
    }

    public void EnableInput()
    {
        inputActions.Enable();
    }

    public void DisableInput()
    {
        inputActions.Disable();
    }

    void Inventory(InputAction.CallbackContext context)
    {
        inventoryContainer.SetActive(!inventoryContainer.activeSelf);
        inventoryManager.RefreshInventory();
    
        if (inventoryContainer.activeSelf)
        {
            playerMovement.DisableInput();
            //inventoryManager.RefreshInventory();
        }

        else 
        {
            playerMovement.EnableInput();
        }
    }
}
