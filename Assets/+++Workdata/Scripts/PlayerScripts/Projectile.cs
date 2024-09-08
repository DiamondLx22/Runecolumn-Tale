using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 10;

    private Rigidbody2D rb;
    private Animator anim;

    public Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        SetDirectionAnimation();


        //Destroy(gameObject, lifetime);
    }

 
    private void FixedUpdate()
    {
     
        transform.parent.position += targetPosition * speed * Time.deltaTime;
       /* float angle = Vector2.SignedAngle(Vector2.right, direction) - 90f;
        Vector3 targetRotation = new Vector3(0, 0, angle);
        Quaternion lookTo = Quaternion.Euler(targetRotation);*/
        //transform.parent.rotation = Quaternion.RotateTowards(transform.rotation, lookTo, 99999999999999999 * Time.deltaTime);

        Vector3 dir = targetPosition;
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
        }


        Destroy(gameObject);
    }

    private void SetDirectionAnimation()
    {
        if (anim != null)
        {
            Vector2 velocity = rb.velocity;
            //anim.SetFloat("dirX", velocity.x);
            //anim.SetFloat("dirY", velocity.y);
        }
    }
}