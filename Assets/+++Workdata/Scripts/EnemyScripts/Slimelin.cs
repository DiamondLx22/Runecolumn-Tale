using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slimelin : MonoBehaviour, IDamageable
{
  public float damage = 1f;
  public float knockbackForce = 10f;
  public float moveSpeed = 500f;

  public SlimelinDetector slimelinDetect;
  private Animator animator;
  Rigidbody2D rb;

  private float _health = 100f;

  public float Health
  {
    get { return _health; }
    set
    {
      if (value < _health)
      {
        animator.SetTrigger("hit");
      }

      _health = value;
    }
  }

  public void Start()
  {
    animator = GetComponent<Animator>();
    rb = GetComponent<Rigidbody2D>();
  }

  public void OnHit(Vector2 knockback)
  {
    rb.AddForce(knockback);
  }

   public void OnHit(float damage)
  {
    Health -= damage;
  }

  public void OnHit(float damage, Vector2 knockback)
  {
    Health -= damage;
    rb.AddForce(knockback);
  }

  void FixedUpdate()
  {
    if (slimelinDetect.detectObjects.Count > 0)
    {
      Vector2 direction = (slimelinDetect.detectObjects[0].transform.position - transform.position).normalized;
      
      rb.AddForce(direction * moveSpeed * Time.deltaTime);
    }
  }

  //void OnCollisionEnter2D(Collider2D col)
    //{
     // IDamageable damageable = col.GetComponent<IDamageable>();

      //if (damageable != null)
     // {
       ////  Vector2 knockback = direction * knockbackForce;

       // damageable.OnHit(damage, knockback);
      //}
   // }
}
