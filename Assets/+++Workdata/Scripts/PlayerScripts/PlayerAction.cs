using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private InputAction typeAction;
    
    public enum ActionType{Default, Attack}
    public ActionType actionType;
    
    public int attackId;
    public int actionId;
    public int weaponId;
    public Animator weaponAnim;
    
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        PlayerMovement.SubscribeAction += Subscribe;
        PlayerMovement.UnsubscribeAction += Unsubscribe;
    }

    private void OnDisable()
    {
        PlayerMovement.SubscribeAction -= Subscribe;
        PlayerMovement.UnsubscribeAction -= Unsubscribe;
    }

    void Subscribe()
    {
        typeAction = playerMovement.inputActions.Player.Attack;
        typeAction.performed += Action;
    }

    void Unsubscribe()
    {
        typeAction.performed -= Action;
    }

    void Action(InputAction.CallbackContext context)
    {
        switch (actionType)
        {
            case ActionType.Default:
                print("No ActionType declared");
                break;

            case ActionType.Attack:
                if (context.performed)
                {
                    playerMovement.anim.SetTrigger("actionTrigger");
                    playerMovement.anim.SetInteger("actionId", actionId); 
                    print("action");
                }
                
                weaponAnim.SetTrigger("actionTrigger");
                weaponAnim.SetInteger("weaponId", weaponId);
                weaponAnim.SetInteger("attackId", attackId);
                float dirX = playerMovement.anim.GetFloat("dirX");
                float dirY = playerMovement.anim.GetFloat("dirY");
                weaponAnim.SetFloat("dirX", dirX);
                weaponAnim.SetFloat("dirY", dirY);
                //Player: 
                //anim trigger & actionTrigger

                //Weapon:
                //dirX & dirY und weaponId, AttackId
                break;

        }
    }
   
}
