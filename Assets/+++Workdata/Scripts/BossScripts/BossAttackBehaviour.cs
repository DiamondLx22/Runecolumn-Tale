using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBehaviour : MonoBehaviour
{
    public float meleeDamage = 10f;
    public float rangeDamage = 15f; 
    public float knockbackForce = 10f;
    public Animator animator;

    public Collider2D hitboxColliderTopDown;
    public Collider2D hitboxColliderRightLeft;

    public bool canAttack = false;  // Wird vom BossController aktiviert/deaktiviert

    private Endboss endBoss;

    private void Start()
    {
        endBoss = transform.GetComponentInParent<Endboss>();
        
        // Deaktiviere die Hitboxen am Anfang
        hitboxColliderTopDown.enabled = false;
        hitboxColliderRightLeft.enabled = false;
    }

    // Angriff starten
    /*public void StartAttack(Vector2 moveDirection)
    {
        if (!canAttack) return;

        if (endBoss.canMeleeAttack)
        {
            StartMeleeAttack(moveDirection);
        }
        else if (endBoss.canRangeAttack)
        {
            StartRangeAttack(moveDirection);
        }
    }*/
    
    private void StartMeleeAttack(Vector2 moveDirection)
    {
        // Disable both hitboxes before deciding which one to enable
        hitboxColliderTopDown.enabled = false;
        hitboxColliderRightLeft.enabled = false;
        
        // Berechnung der Angriffsrichtung
        hitboxColliderTopDown.enabled = false;
        hitboxColliderRightLeft.enabled = false;

        // Je nach Bewegungsrichtung (horizontal oder vertikal) die richtige Hitbox aktivieren
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            hitboxColliderRightLeft.enabled = true;
            Vector2 offset = hitboxColliderRightLeft.offset;
            offset.x = Mathf.Abs(offset.x) * (moveDirection.x > 0 ? 1 : -1);
            hitboxColliderRightLeft.offset = offset;
        }
        else
        {
            hitboxColliderTopDown.enabled = true;
            Vector2 offset = hitboxColliderTopDown.offset;
            offset.y = Mathf.Abs(offset.y) * (moveDirection.y > 0 ? 1 : -1);
            hitboxColliderTopDown.offset = offset;
        }

        // Animationen basierend auf dem aktuellen Angriffszustand (AttackState)
        switch (endBoss.currentAttackState)
        {
            case Endboss.AttackState.attack1:
                meleeDamage = endBoss.meleeDamage;
                animator.SetTrigger("Attack1");
                break;
            case Endboss.AttackState.attack2:
                meleeDamage = endBoss.meleeDamage;
                animator.SetTrigger("Attack2");
                break;
            case Endboss.AttackState.attack3:
                meleeDamage = endBoss.meleeDamage;
                animator.SetTrigger("Attack3");
                break;
        }
    }
    
    public void StartRangeAttack(Vector2 moveDirection)
    {
        switch (endBoss.currentAttackState)
        {
            case Endboss.AttackState.attack1:
                rangeDamage = endBoss.rangeDamage;
                animator.SetTrigger("RangeAttack1");
                break;
            case Endboss.AttackState.attack2:
                rangeDamage = endBoss.rangeDamage;
                animator.SetTrigger("RangeAttack2");
                break;
            case Endboss.AttackState.attack3:
                rangeDamage = endBoss.rangeDamage;
                animator.SetTrigger("RangeAttack3");
                break;
        }
    }
    
    /*
    private void FireProjectile(Vector2 moveDirection)
    {
        // Instantiate the projectile and shoot it towards the target
        GameObject projectile = Instantiate(endBoss.projectilePrefab, transform.position, Quaternion.identity);
        Vector2 direction = moveDirection.normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * endBoss.projectileSpeed;
    }
    */

    // Angriff beenden
    public void EndAttack()
    {
        animator.SetTrigger("StopAttack");

        // Deaktiviere die Hitboxen
        hitboxColliderRightLeft.enabled = false;
        hitboxColliderTopDown.enabled = false;
    }

    // Trefferbehandlung
    public void ColliderHit(Collider2D collider)
    {
        PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Vector3 parentPosition = transform.parent.position;
            Vector2 direction = (collider.transform.position - parentPosition).normalized;
            Vector2 knockback = direction * knockbackForce;

            playerHealth.TakeDamage((int)meleeDamage);
            playerHealth.TakeDamage((int)rangeDamage);
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
