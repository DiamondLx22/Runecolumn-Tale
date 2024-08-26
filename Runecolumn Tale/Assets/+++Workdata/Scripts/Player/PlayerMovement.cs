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
    private InputAction interactAction;
    public Rigidbody2D rb;
    public Animator[] anim;
    private Vector2 moveInput;
    public float movespeed;
    //private Interactable selectedInteractable;

    public static event Action SubscribeAction;
    public static event Action UnsubscribeAction;
    
    


    private void Awake()
    {
        inputActions = new Player_InputActions();
        moveAction = inputActions.Player.Move;
        interactAction = inputActions.Player.Interact;
    }
      
    private void OnEnable()
    {
        inputActions.Enable();
        moveAction.performed += Move;
        moveAction.canceled += Move;
        
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

    private void Update()
    {
        UpdateAnimations();
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


