using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public float swordDamage = 10f;
    public float knockbackForce = 10f;
    public Animator[] anim;
    public Collider2D hitboxColliderTopDown;
    public Collider2D hitboxColliderRightLeft;

    private PlayerMovement playerMovement;

    
    
    public void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        
        hitboxColliderTopDown.enabled = false;
        hitboxColliderRightLeft.enabled = false;
    }

    public void StartAttack()
    {
        float dirX = playerMovement.moveInput.x;
        float dirY = playerMovement.moveInput.y;

        hitboxColliderTopDown.enabled = false;
        hitboxColliderRightLeft.enabled = false;

        if (dirX != 0)
        {
            hitboxColliderRightLeft.enabled = true;

            Vector2 offset = hitboxColliderRightLeft.offset;
            offset.x = Mathf.Abs(offset.x) * (dirX > 0 ? 1 : -1);
            hitboxColliderRightLeft.offset = offset;
        }
        else if (dirY != 0)
        {
            hitboxColliderTopDown.enabled = true;
            
            Vector2 offset = hitboxColliderTopDown.offset;
            offset.y = Mathf.Abs(offset.y) * (dirY > 0 ? 1 : -1);
            hitboxColliderTopDown.offset = offset;
        }
    }

    public void EndAttack()
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].gameObject.SetActive(false);
        }

        hitboxColliderRightLeft.enabled = false;
        hitboxColliderTopDown.enabled = false;

        playerMovement.canMeleeAttack = true;
        playerMovement.canRangeAttack = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Slimelin slimelin = collider.GetComponent<Slimelin>();

        if (slimelin != null)
        {
            Vector3 parentPosition = transform.parent.position;
            Vector2 direction = collider.transform.position - parentPosition.normalized;
            Vector2 knockback = direction * knockbackForce;

            slimelin.ApplyDamage(swordDamage);
            slimelin.ApplyKnockback(knockback);
        }

        else
        {
            Debug.LogWarning("Collider hat keine HealthComponent");
        }

        // Überprüfen, ob das kollidierte Objekt das Tag "Enemy" hat
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Treffer erfolgreich");
        }
    }
}
