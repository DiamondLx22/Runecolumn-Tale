using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBehaviour : MonoBehaviour
{
    public float swordDamage = 10f;
    public float knockbackForce = 10f;
    public Animator[] anim;
    public Collider2D hitboxColliderTopDown;
    public Collider2D hitboxColliderRightLeft;

    private Slimelin slimelin;

    public float dirX;
    public float dirY;
    
    public void Start()
    {
        slimelin = FindObjectOfType<Slimelin>();

        if (hitboxColliderRightLeft)
        {
            hitboxColliderRightLeft.enabled = false;
        }

        if (hitboxColliderTopDown)
        {
            hitboxColliderTopDown.enabled = false;
        }
    }

    public void StartAttack()
    {
        //dirX = playerMovement.moveInput.x;
        //dirY = playerMovement.moveInput.y;
     
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
            anim[i].SetTrigger("StopAttack");
        }

        if (hitboxColliderRightLeft != null)
        {
            hitboxColliderRightLeft.enabled = false; 
        }

        if (hitboxColliderTopDown != null)
        {
            hitboxColliderTopDown.enabled = false; 
        }
    }

    public void ColliderHit(Collider2D collider)
    {
        PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Vector3 parentPosition = transform.parent.position;
            Vector2 direction = collider.transform.position - parentPosition.normalized;
            Vector2 knockback = direction * knockbackForce;

            playerHealth.TakeDamage((int)swordDamage);
            playerHealth.ApplyKnockback(knockback);
        }

        else
        {
            Debug.LogWarning("Collider hat keine HealthComponent");
        }

        // Überprüfen, ob das kollidierte Objekt das Tag "Player" hat
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Gegner Treffer erfolgreich");
        }
    }
    
}

