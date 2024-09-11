using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float swordDamage = 10f;
    public float knockbackForce = 10f;
    public Animator[] anim;
    public Collider2D hitboxColliderTopDown;
    public Collider2D hitboxColliderRightLeft;

    public AttackState currentAttackState;

    private Rigidbody2D rb; // Neu hinzugefügt
    private SpriteRenderer spriteRenderer; // Neu hinzugefügt
    public Color32 normalColor; // Neu hinzugefügt
    public Color32 hitColor; // Neu hinzugefügt
    public float hitColorTime = 0.1f; // Neu hinzugefügt

    [System.Serializable]
    public enum AttackState
    {
        attack1,
        attack2,
        attack3
    }

    //private EndBoss endboss;

    public float dirX;
    public float dirY;

    public void Start()
    {
        //endboss = FindObjectOfType<EndBoss>();

        hitboxColliderTopDown.enabled = false;
        hitboxColliderRightLeft.enabled = false;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartAttack()
    {
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

        hitboxColliderRightLeft.enabled = false;
        hitboxColliderTopDown.enabled = false;
    }

    public void OnBossDeath()
    {
        Destroy(gameObject);
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
            ApplyKnockback(knockback); // Anwendung von ApplyKnockback
        }
        else
        {
            Debug.LogWarning("Collider hat keine HealthComponent");
        }

        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Gegner Treffer erfolgreich");
        }
    }

    // Knockback-Funktion aus Slimelin.cs übernommen
    public void ApplyKnockback(Vector2 knockback)
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(knockback, ForceMode2D.Impulse);
            rb.isKinematic = true;
            Invoke("ResetVelocity", 1);
        }
    }

    private void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }

    // Neu: Farbanpassung für Treffer
    private void ResetColor()
    {
        spriteRenderer.color = normalColor;
    }

    // Kollisionserkennung aus Slimelin.cs übernommen
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockbackForce;

            enemyHealth.OnHit(swordDamage, knockback); // Übertragen des Schadens und Knockback
        }
    }
}
