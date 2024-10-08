using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Player_InputActions inputActions;
    private InputAction moveAction;
    private InputAction meleeAttack;
    private InputAction rangeAttack;
    private Vector2 lookDirection = Vector2.down;

    public GameObject staff1Projectile;
    public GameObject projectile;


    private InputAction interactAction;
    public Rigidbody2D rb;

    public Animator[] meleeAnim;
    public Animator[] rangeAnim;
    public ProjectileSpawner[] projectileSpawners;
    public Animator[] anim;
    public EquipmentSlot[] equipmentSlot;
    public GameObject[] swords;
    public Vector2 moveInput;

    public float movespeed;
    private Interactable selectedInteractable;
  

    public static event Action SubscribeAction;
    public static event Action UnsubscribeAction;

    //--- Awake ---

    #region Awake

    private void Awake()
    {
        inputActions = new Player_InputActions();
        moveAction = inputActions.Player.Move;
        interactAction = inputActions.Player.Interact;
        meleeAttack = inputActions.Player.MeleeAttack;
        rangeAttack = inputActions.Player.RangeAttack;
    }

    #endregion

    //--- OnEnable ---

    #region OnEnable

    private void OnEnable()
    {
        inputActions.Enable();
        moveAction.performed += Move;
        moveAction.canceled += Move;

        meleeAttack.performed += MeleeAttack;
        rangeAttack.performed += RangeAttack;


        interactAction.performed += Interact;
        StartCoroutine(routine: DelaySubscribe());
    }

    IEnumerator DelaySubscribe()
    {
        yield return null;
        SubscribeAction?.Invoke();
    }

    #endregion


    //--- OnDisable ---

    #region OnDisable

    private void OnDisable()
    {
        inputActions.Disable();
        moveAction.performed -= Move;
        moveAction.canceled -= Move;

        meleeAttack.performed -= MeleeAttack;
        rangeAttack.performed -= RangeAttack;


        interactAction.performed -= Interact;
    }

    public void EnableInput()
    {
        inputActions.Enable();
    }

    public void DisableInput()
    {
        inputActions.Disable();
    }

    #endregion


    //--- Movement ---

    #region Movement

    void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.performed)
        {
            if (moveInput != Vector2.zero)
            {
                lookDirection = moveInput;
            }
        }

        else if (context.canceled)
        {
        }

        //if (moveInput != Vector2.zero) lookDirection = context.ReadValue<Vector2>();
    }

    #endregion


    //--- MeleeAttack CancelAnimation ---

    #region MeleeAttack CancelAnimation

    public bool canMeleeAttack = true;

    private void MeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canMeleeAttack && IsMeleeItemEquipped())
        {
            for (int i = 0; i < meleeAnim.Length; i++)
            {
                meleeAnim[i].gameObject.SetActive(true);
                WeaponBehaviour weaponBehaviour = meleeAnim[i].GetComponent<WeaponBehaviour>();
                if (weaponBehaviour != null)
                {
                    weaponBehaviour.StartAttack();
                    weaponBehaviour.dirX = lookDirection.x;
                    weaponBehaviour.dirY = lookDirection.y;
                }
                
                anim[i].SetFloat("dirX", lookDirection.x);
                anim[i].SetFloat("dirY", lookDirection.y);
                
                anim[i].SetTrigger("meleeAttack");
            }

            canMeleeAttack = false;
        }
        else
        {
            DisableMeleeAnimations();
        }
    }

    #endregion


    //--- RangeAttack CancelAnimation ---

    #region RangeAttack CancelAnimation

    public bool canRangeAttack = true;

    private void RangeAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canRangeAttack && IsRangeItemEquipped())
        {
            for (int i = 0; i < rangeAnim.Length; i++)
            {
                rangeAnim[i].gameObject.SetActive(true);
                anim[i].SetFloat("dirX", lookDirection.x);
                anim[i].SetFloat("dirY", lookDirection.y);
                
                anim[i].SetTrigger("rangeAttack");
            }

            canRangeAttack = false;
        }
        else
        {
            DisableRangeAnimations();
        }
    }
    

    #endregion


    //--- Update ---

    #region Update

    private void Update()
    {
        if (moveInput != Vector2.zero)
        {
            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetFloat("dirX", moveInput.x);
                anim[i].SetFloat("dirY", moveInput.y);
            }

            lookDirection = moveInput;
        }
        UpdateAnimations();
    }

    #endregion

    
    
    public List<string> swordIDs = new List<string>
    {
        
    };
    
    private bool IsMeleeItemEquipped()
    {
        foreach (var slot in equipmentSlot)
        {
            if (slot.assignedItem != null && swordIDs.Contains(slot.assignedItem.itemState.id))  // Beispielhafte ID
            {
                return true;
            }
        }
        return false;
    }



    public List<string> staffIDs = new List<string>
    {
        "item_amber_staff"
    };
        
    private bool IsRangeItemEquipped()
    {
        foreach (var slot in equipmentSlot)
        {
            if (slot.assignedItem != null && staffIDs.Contains(slot.assignedItem.itemState.id))  // Beispielhafte ID
            {
                return true;
            }
        }
        return false;
    }

    // Deaktiviere Nahkampfanimationen
    private void DisableMeleeAnimations()
    {
        for (int i = 0; i < meleeAnim.Length; i++)
        {
            meleeAnim[i].gameObject.SetActive(false);
        }
    }

    // Deaktiviere Fernkampfanimationen
    private void DisableRangeAnimations()
    {
        for (int i = 0; i < rangeAnim.Length; i++)
        {
            rangeAnim[i].gameObject.SetActive(false);
        }
    }
    


    //--- UpdateAnimations ---

    #region UpdateAnimations

    public void UpdateAnimations()
    {
        if (moveInput != Vector2.zero)
        {
            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetFloat("dirX", moveInput.x);
                anim[i].SetFloat("dirY", moveInput.y);
            }

            lookDirection = moveInput;
        }


        for (int i = 0; i < rangeAnim.Length; i++)
        {
                rangeAnim[i].SetFloat("dirX", lookDirection.x);
                rangeAnim[i].SetFloat("dirY", lookDirection.y);
        }

        for (int i = 0; i < meleeAnim.Length; i++)
        {
                meleeAnim[i].SetFloat("dirX", lookDirection.x);
                meleeAnim[i].SetFloat("dirY", lookDirection.y);
                
                WeaponBehaviour weaponBehaviour = meleeAnim[i].GetComponent<WeaponBehaviour>();
                if (weaponBehaviour != null)
                {
                    weaponBehaviour.dirX = moveInput.x;
                    weaponBehaviour.dirY = moveInput.y;
                }
        }
    

        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetBool("isMoving", moveInput != Vector2.zero);
        }

        for (int i = 0; i < projectileSpawners.Length; i++)
        {
            projectileSpawners[i].targetDirection = new Vector2(lookDirection.x, lookDirection.y);
        }
    }

    #endregion


    private void FixedUpdate()
    {
        rb.velocity = moveInput * movespeed;
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (selectedInteractable != null)
        {
            selectedInteractable.Interact();
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        TrySelectInteractable(col);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        TryDeselectInteractable(col);
    }

    private void TrySelectInteractable(Collider2D col)
    {
        Interactable interactable = col.GetComponent<Interactable>();
        if (interactable == null) return;

        if (selectedInteractable != null)
        {
            selectedInteractable.Deselect();
        }

        selectedInteractable = interactable;
        selectedInteractable.Select();
    }

    private void TryDeselectInteractable(Collider2D col)
    {
        Interactable interactable = col.GetComponent<Interactable>();
        if (interactable == null) return;

        if (interactable == selectedInteractable)
        {
            selectedInteractable.Deselect();
            selectedInteractable = null;
        }
    }
}