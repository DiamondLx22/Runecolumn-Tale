using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBehaviour : MonoBehaviour
{
      public float swordDamage = 10f;
    public float knockbackForce = 10f;
    public Animator animator;

    public Collider2D hitboxColliderTopDown;
    public Collider2D hitboxColliderRightLeft;

    public bool canAttack = false;  // Wird vom BossController aktiviert/deaktiviert

    private BossController bossController;

    private void Start()
    {
        bossController = GetComponent<BossController>();
        
        // Deaktiviere die Hitboxen am Anfang
        hitboxColliderTopDown.enabled = false;
        hitboxColliderRightLeft.enabled = false;
    }

    // Angriff starten
    public void StartAttack(Vector2 moveDirection)
    {
        if (!canAttack) return;

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
        switch (bossController.currentAttackState)
        {
            case BossController.AttackState.attack1:
                animator.SetTrigger("Attack1");
                break;
            case BossController.AttackState.attack2:
                animator.SetTrigger("Attack2");
                break;
            case BossController.AttackState.attack3:
                animator.SetTrigger("Attack3");
                break;
        }
    }

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
