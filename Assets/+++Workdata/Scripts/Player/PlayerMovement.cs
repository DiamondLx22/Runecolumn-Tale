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

    public float timerMax;
    private float timer;
    private bool attacked = false;


    private void Awake()
    {
        inputActions = new Player_InputActions();
        moveAction = inputActions.Player.Move;
        interactAction = inputActions.Player.Interact;
        meleeAttack = inputActions.Player.MeleeAttack;
        //rangeAttack = inputActions.Player.RangeAttack;
    }
      
    private void OnEnable()
    {
        inputActions.Enable();
        moveAction.performed += Move;
        moveAction.canceled += Move;

        meleeAttack.performed += MeleeAttack;
        meleeAttack.canceled += MeleeAttack;

        rangeAttack.performed += RangeAttack;
        rangeAttack.canceled += RangeAttack;

        //interactAction.performed += Interact;
        StartCoroutine(routine: DelaySubscribe());
    }

    IEnumerator DelaySubscribe()
    {
        yield return null;
        SubscribeAction?.Invoke();
    }
    
    

    private void OnDisable()
    {
        inputActions.Disable();
        moveAction.performed -= Move;
        moveAction.canceled -= Move;

        meleeAttack.performed -= MeleeAttack;
        meleeAttack.canceled -= MeleeAttack;

        rangeAttack.performed -= RangeAttack;
        rangeAttack.canceled -= RangeAttack;


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


 
    private void MeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            for (int i = 0; i < weaponanim.Length; i++)
            {
                //weaponanim[i].SetBool("meleeAttack",true);
                SpriteRenderer renderer = weaponanim[i].GetComponent<SpriteRenderer>();
                renderer.enabled = true;
                weaponanim[i].enabled = true;
                print(moveInput.x + "  " + moveInput.y);
            }

            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetTrigger("meleeAttack");
            }
            attacked = true;
        }
    }


    private void RangeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            for (int i = 0; i < rangeAttackAnims.Length; i++)
            {
                //weaponanim[i].SetBool("meleeAttack",true);
                SpriteRenderer renderer = rangeAttackAnims[i].GetComponent<SpriteRenderer>();
                renderer.enabled = true;
                rangeAttackAnims[i].enabled = true;
                rangeAttackAnims[i].SetTrigger("rangeAttack");
                //print(moveInput.x + "  " + moveInput.y);
            }

            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].SetTrigger("rangeAttack");
            }
            attacked = true;
        }
    }
    private void Update()
    {
        UpdateAnimations();

        if (attacked)
        {
            timer += Time.deltaTime;
            if (timer > timerMax)
            {
                for (int i = 0; i < weaponanim.Length; i++)
                {
                    //weaponanim[i].SetBool("meleeAttack",true);
                    //weaponanim[i].enabled = false;
                    SpriteRenderer renderer = weaponanim[i].GetComponent<SpriteRenderer>();
                    renderer.enabled = false;
                }
                
                for (int i = 0; i < rangeAttackAnims.Length; i++)
                {
                    //weaponanim[i].SetBool("meleeAttack",true);
                    //weaponanim[i].enabled = false;
                    SpriteRenderer renderer = rangeAttackAnims[i].GetComponent<SpriteRenderer>();
                    renderer.enabled = false;
                }
                
                attacked = false;
                timer = 0;
            }

        }
    }

    
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


