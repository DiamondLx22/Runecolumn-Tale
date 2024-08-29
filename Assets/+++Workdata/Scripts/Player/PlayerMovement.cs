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
    
    public Animator[] weaponanim;
    public Animator[] rangeAttackAnims;
    public Animator[] anim;
    private Vector2 moveInput;
    public float movespeed;
    //private Interactable selectedInteractable;

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
   

        //interactAction.performed += Interact;
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


        // interactAction.performed -= Interact;
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
    private void MeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            for (int i = 0; i < weaponanim.Length; i++)
            {
               weaponanim[i].gameObject.SetActive(true);
               weaponanim[i].SetFloat("dirX", anim[0].GetFloat("dirX"));
               weaponanim[i].SetFloat("dirY", anim[0].GetFloat("dirY"));
            }

            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetTrigger("meleeAttack");
            }
        }
    }
    #endregion

    
    
    //--- RangeAttack CancelAnimation ---
    #region RangeAttack CancelAnimation
    
    private void RangeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            for (int i = 0; i < weaponanim.Length; i++)
            {
                weaponanim[i].gameObject.SetActive(true);
                weaponanim[i].SetFloat("dirX", anim[0].GetFloat("dirX"));
                weaponanim[i].SetFloat("dirY", anim[0].GetFloat("dirY"));
            }

            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetTrigger("rangeAttack");
            }
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
            for (int i = 0; i < weaponanim.Length; i++)
            {
                weaponanim[i].SetFloat("dirX", moveInput.x);
                weaponanim[i].SetFloat("dirY", moveInput.y);
            }
            
            for (int i = 0; i < rangeAttackAnims.Length; i++)
            {
                rangeAttackAnims[i].SetFloat("dirX", moveInput.x);
                rangeAttackAnims[i].SetFloat("dirY", moveInput.y);
            }

        }
        for (int i = 0; i < anim.Length; i++)
        {
            weaponanim[i].SetBool("meleeAttack", moveInput != Vector2.zero);
        }

    }
    #endregion



    private void FixedUpdate()
    {
        rb.velocity = moveInput * movespeed;
    }

   private void Interact(InputAction.CallbackContext context)
    {
       // if (selectedInteractable != null)
        {
           // selectedInteractable.Interact();
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
      //  Interactable interactable = col.GetComponent<Interactable>();
        //if (interactable == null) return;

       // if (selectedInteractable != null)
        {
        //    selectedInteractable.Deselect();
        }
       // selectedInteractable = interactable;
       // selectedInteractable.Select();
    }

    private void TryDeselectInteractable(Collider2D col)
    {
       // Interactable interactable = col.GetComponent<Interactable>();
        //if (interactable == null) return;

      //  if (interactable == selectedInteractable)
        {
       //     selectedInteractable.Deselect();
       //     selectedInteractable = null;
        }
    }


}


