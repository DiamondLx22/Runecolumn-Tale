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


    private InputAction interactAction;
    public Rigidbody2D rb;
    
    public Animator[] meleeAnim;
    public Animator[] rangeAnim;
    public Animator[] anim;
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

        }

        else if (context.canceled) 
        { 
        
        }
        
    }

    #endregion

    

    //--- MeleeAttack CancelAnimation ---
    #region MeleeAttack CancelAnimation

    public bool canMeleeAttack = true;
    private void MeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canMeleeAttack)
        {
            for (int i = 0; i < meleeAnim.Length; i++)
            {
                meleeAnim[i].gameObject.SetActive(true);
                meleeAnim[i].SetFloat("dirX", anim[0].GetFloat("dirX"));
                meleeAnim[i].SetFloat("dirY", anim[0].GetFloat("dirY"));
            }

            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetTrigger("meleeAttack");
            }

            canMeleeAttack = false;
        }
    }
    #endregion

    
    
    //--- RangeAttack CancelAnimation ---
    #region RangeAttack CancelAnimation

    public bool canRangeAttack = true;
    private void RangeAttack(InputAction.CallbackContext context)
    {
        if (context.performed && canRangeAttack)
        {
            for (int i = 0; i < rangeAnim.Length; i++)
            {
                rangeAnim[i].gameObject.SetActive(true);
                rangeAnim[i].SetFloat("dirX", anim[0].GetFloat("dirX"));
                rangeAnim[i].SetFloat("dirY", anim[0].GetFloat("dirY"));
            }

            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetTrigger("rangeAttack");
            }

            canRangeAttack = false;
        }
    }
    #endregion
    
    
    
    //--- Update ---
    #region Update
    private void Update()
    {
        UpdateAnimations();
    }
    #endregion
    
    
    
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

        }
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetBool("isMoving", moveInput != Vector2.zero);
        }


        if (moveInput != Vector2.zero)
        {
            for (int i = 0; i < rangeAnim.Length; i++)
            {
                rangeAnim[i].SetFloat("dirX", moveInput.x);
                rangeAnim[i].SetFloat("dirY", moveInput.y);
            }
            
            for (int i = 0; i < meleeAnim.Length; i++)
            {
                meleeAnim[i].SetFloat("dirX", moveInput.x);
                meleeAnim[i].SetFloat("dirY", moveInput.y);
            }

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





