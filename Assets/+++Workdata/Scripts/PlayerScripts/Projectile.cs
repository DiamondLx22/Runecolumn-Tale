using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    private Rigidbody2D rb;
    private Animator anim;

    public Vector3 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        SetDirectionAnimation();

        
    }

 
    private void FixedUpdate()
    {
        transform.parent.position += direction * speed * Time.deltaTime;
    }

    public void RotateObject()
    {
        Vector2 dir = direction;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.parent.rotation = Quaternion.Euler(0, 0, angle);
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
            
            Destroy(gameObject);
        }

    }

    public void SetDirectionAnimation()
    {
        if (anim != null)
        {
           // anim.SetFloat("dirX", direction.x);
           // anim.SetFloat("dirY", direction.y);
        }
    }
}