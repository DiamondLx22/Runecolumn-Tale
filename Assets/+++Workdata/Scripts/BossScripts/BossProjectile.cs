using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    public float fireInterval = 1.0f;            // Intervall zwischen den Schüssen
    public int numberOfShots = 3;                // Anzahl der Schüsse im Intervall
    
    public GameObject bossprojectilePrefab;          // Referenz auf das Projektil-Prefab
    public Transform firePoint; 
    
    private Transform target;
    private Vector2 attackDirection;
    private bool useIntervalFire = false;

    public Collider2D hitboxColliderTopDown;
    public Collider2D hitboxColliderRightLeft;
    public float knockbackForce = 10f;

    public void Initialize(Transform targetTransform, float bossprojectileDamage, Vector2 attackDir)
    {
        target = targetTransform;
        damage = bossprojectileDamage;
        attackDirection = attackDir;
        useIntervalFire = useIntervalFire;

        ActivateHitbox();
    }
    
    private void Start()
    {
             AlignBossProjectileToDirection();
     
             if (useIntervalFire)
             {
                 StartCoroutine(FireBossProjectilesInInterval());
             }
             else
             {
                 SingleFire();
             }
         }
         
         private void ActivateHitbox()
         {
             hitboxColliderTopDown.enabled = false;
             hitboxColliderRightLeft.enabled = false;
     
             if (attackDirection.x != 0)
             {
                 hitboxColliderRightLeft.enabled = true;
                 Vector2 offset = hitboxColliderRightLeft.offset;
                 offset.x = Mathf.Abs(offset.x) * (attackDirection.x > 0 ? 1 : -1);
                 hitboxColliderRightLeft.offset = offset;
             }
             else if (attackDirection.y != 0)
             {
                 hitboxColliderTopDown.enabled = true;
                 Vector2 offset = hitboxColliderTopDown.offset;
                 offset.y = Mathf.Abs(offset.y) * (attackDirection.y > 0 ? 1 : -1);
                 hitboxColliderTopDown.offset = offset;
             }
         }
         
         private void FireSingleBossProjectile()
         {
             if (bossprojectilePrefab != null && firePoint != null)
             {
                 GameObject newProjectile = Instantiate(bossprojectilePrefab, firePoint.position, Quaternion.identity);
                 
                 BossProjectile bossprojectileScript = newProjectile.GetComponent<BossProjectile>();
     
                 if (bossprojectileScript != null)
                 {
                     bossprojectileScript.Initialize(target, damage, attackDirection);
                 }
             }
         }
         
         private IEnumerator FireBossProjectilesInInterval()
         {
             for (int i = 0; i < numberOfShots; i++)
             {
                 if (target != null)
                 {
                     FireSingleBossProjectile();
                 }
                 
                 yield return new WaitForSeconds(fireInterval);
             }
             
             Destroy(gameObject);
         }
         
         private void AlignBossProjectileToDirection()
         {
             if (attackDirection != Vector2.zero)
             {
                 float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
                 transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
             }
         }
     
         private void SingleFire()
         {
             if (target== null)
             {
                Destroy(gameObject);
                return;
             }
             
             Vector3 moveDirection = (target.transform.position - transform.position).normalized;
             Rigidbody2D rb = GetComponent<Rigidbody2D>();
             rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * speed;
     
             //Vector3 direction = (target.position - transform.position).normalized;
             //transform.position += direction * speed * Time.deltaTime;
         }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform == target)
        {
            PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)damage);
            }
            
            Destroy(gameObject);
        }
    }

    private void ApplyKnockback(Collider2D collider)
    {
        Rigidbody2D playerRigidbody = collider.GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            Vector2 knockbackDirection = attackDirection.normalized;
            Vector2 knockback = knockbackDirection * knockbackForce;
            
            playerRigidbody.AddForce(knockback, ForceMode2D.Impulse);
        }
    }
    
    public void ColliderHit(Collider2D collider)
    {
        PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Vector3 parentPosition = transform.position;
            Vector2 direction = new Vector2(attackDirection.x, attackDirection.y);
            Vector2 knockback = direction * knockbackForce;

            playerHealth.TakeDamage((int)damage);
            // Knockback kann hier auf den Spieler angewendet werden
        }
        
        Destroy(gameObject);
    }


    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destroy if the target no longer exists
            return;
        }

        // Move towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        ////transform.position += direction * speed * Time.deltaTime;
        //transform.position += direction * speed;

        // Optional: Add rotation towards the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
