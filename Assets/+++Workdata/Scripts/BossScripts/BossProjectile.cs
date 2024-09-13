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

    public void Initialize(Transform targetTransform, float bossprojectileDamage, Vector2 attackDir)
    {
        target = targetTransform;
        damage = bossprojectileDamage;
        attackDirection = attackDir;
    }
    
    private void Start()
    {
        // Richte das Projektil in die Angriffsrichtung aus
        AlignBossProjectileToDirection();

        // Nach dem Start das Intervall-Feuer auslösen
        StartCoroutine(FireBossProjectilesInInterval());
    }
    
    private void FireSingleBossProjectile()
    {
        if (bossprojectilePrefab != null && firePoint != null)
        {
            // Instanziere ein neues Projektil am FirePoint
            GameObject newProjectile = Instantiate(bossprojectilePrefab, firePoint.position, Quaternion.identity);

            // Hole das Projektil-Skript des neuen Projektils
            BossProjectile bossprojectileScript = newProjectile.GetComponent<BossProjectile>();

            if (bossprojectileScript != null)
            {
                // Initialisiere das neue Projektil mit dem gleichen Ziel, Schaden und Angriffsrichtung
                bossprojectileScript.Initialize(target, damage, attackDirection);
            }
        }
    }
    
    private void AlignBossProjectileToDirection()
    {
        // Optional: Das Projektil in die richtige Richtung drehen
        if (attackDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
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
        transform.position += direction * speed * Time.deltaTime;

        // Optional: Add rotation towards the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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

            Destroy(gameObject); // Destroy projectile on hit
        }
    }
    
    private IEnumerator FireBossProjectilesInInterval()
    {
        for (int i = 0; i < numberOfShots; i++)
        {
            if (target != null)
            {
                FireSingleBossProjectile();  // Einzelnes Projektil feuern
            }

            // Warte das angegebene Intervall, bevor der nächste Schuss abgefeuert wird
            yield return new WaitForSeconds(fireInterval);
        }

        // Nach dem letzten Schuss das ursprüngliche Projektil zerstören
        Destroy(gameObject);
    }
}
