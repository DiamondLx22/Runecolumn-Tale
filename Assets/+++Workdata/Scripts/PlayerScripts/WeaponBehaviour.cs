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
    private ProjectileSpawner projectileSpawner;

    public float dirX;
    public float dirY;
    
    public void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        
        hitboxColliderTopDown.enabled = false;
        hitboxColliderRightLeft.enabled = false;
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
            anim[i].gameObject.SetActive(false);
        }

        playerMovement.canMeleeAttack = true;
        playerMovement.canRangeAttack = true;
        
        hitboxColliderRightLeft.enabled = false;
        hitboxColliderTopDown.enabled = false;
    }

    public void ShootStaff1Projectile()
    {
        if (projectileSpawner != null)
        {
            projectileSpawner.SpawnProjectile(); 
        }
        else
        {
            Debug.LogError("ProjectileSpawner wurde nicht gefunden!");
        }
        
        
    }

    public void ColliderHit(Collider2D collider)
    {
        EnemyHealth enemy = collider.GetComponent<EnemyHealth>();
        print(enemy.gameObject.name);
        if (enemy != null)
        {
            Vector3 parentPosition = transform.parent.position;
            Vector2 direction = new Vector2(dirX, dirY);
            Vector2 knockback = direction * knockbackForce;

            enemy.OnHit(swordDamage, knockback);
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
