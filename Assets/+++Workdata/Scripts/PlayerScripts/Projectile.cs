using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;          
    public float lifetime = 5f;         
    public int damage = 10;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        SetDirectionAnimation();

   
        Destroy(gameObject, lifetime);
        
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * speed;  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); 
        }

 
        Destroy(gameObject);
    }

    private void SetDirectionAnimation()
    {
        if (anim != null)
        {
            Vector2 velocity = rb.velocity;
            anim.SetFloat("dirX", velocity.x);
            anim.SetFloat("dirY", velocity.y);
        }
    }
}