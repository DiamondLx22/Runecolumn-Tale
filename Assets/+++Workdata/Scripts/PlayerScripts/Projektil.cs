using System;
using UnityEngine;

public class Projektil : MonoBehaviour
{
    public float speed = 10f;           // Die Geschwindigkeit des Projektils
    public float lifetime = 5f;         // Die Lebensdauer des Projektils umgewandelt in Sekunden
    public int damage = 10;             // Schaden, welches das Projektil verursacht

    public float dirX;  // Direction of projectile (set by player)
    public float dirY;  // Direction of projectile (set by player)

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        SetDirectionAnimation();

        // Destroy the projectile after a certain lifetime
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        // Move the projectile in the direction provided (dirX, dirY)
        Vector2 direction = new Vector2(dirX, dirY).normalized; // Ensure direction is normalized
        rb.velocity = direction * speed;
        //transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)  // Changed from Collider to Collider2D for 2D collision detection
    {
        // Check if the projectile hits an enemy
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            // Apply damage to the enemy
            enemy.TakeDamage(damage);
        }

        // Destroy the projectile once it hits something
        Destroy(gameObject);
    }

    private void SetDirectionAnimation()
    {
        if (anim != null)
        {
            anim.SetFloat("dirX", dirX);
            anim.SetFloat("dirY", dirY);
        }
    }
}