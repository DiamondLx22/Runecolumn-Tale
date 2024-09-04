using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slimelin : MonoBehaviour
{
  public float maxHealth = 100f;
  private float currentHealth;
  public float damage = 10f;
  public float knockbackForce = 5f;
  private Rigidbody2D rb;

  public float hitColorTime = 0.1f;
  public Color32 normalColor;
  public Color32 hitColor;

  private SpriteRenderer spriteRenderer;
  public SlimelinDetector slimelinDetect;
  private Animator animator;
  public float moveSpeed;

  public void ApplyDamage(float damage)
  {
    currentHealth -= damage;
    if (currentHealth <= 0f)
    {
      Die();
    }
  }
  

  public void Start()
  {
    currentHealth = maxHealth;
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    
    animator = GetComponent<Animator>();
  }
  

   public void OnHit(float damage)
   {
     currentHealth -= this.damage;
     spriteRenderer.color = hitColor;
     Invoke("ResetColor", hitColorTime);

     if (currentHealth <= 0)
     {
       Die();
     }
   }

   public void ApplyKnockback(Vector2 knockback)
   {
     Rigidbody2D rb = GetComponent<Rigidbody2D>();
     if (rb != null)
     {
       rb.AddForce(knockback, ForceMode2D.Impulse);
     }
   }

   private void ResetColor()
   {
     spriteRenderer.color = normalColor;
   }

   void Die()
   {
     Destroy(gameObject);
   }

   private void OnCollisionEnter2D(Collision2D collision)
   {
     EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
     if (enemyHealth != null)
     {
       Vector2 direction = (collision.transform.position - transform.position).normalized;
       Vector2 knockback = direction * knockbackForce;
       
       enemyHealth.OnHit(damage, knockback);
     }
   }


   void FixedUpdate()
  {
    if (slimelinDetect.detectObjects.Count > 0)
    {
      //Vector2 direction = (slimelinDetect.detectObjects[0].transform.position - transform.position).normalized;
      Vector2 direction = Vector2.MoveTowards(transform.position, slimelinDetect.detectObjects[0].transform.position, moveSpeed * Time.deltaTime);
      
      transform.position = direction;
      
      
    }
  }

  //e void OnTriggerEnter2D(Collider2D col)
  //
  //ageable damageable = col.GetComponent<IDamageable>();

  //damageable != null)
  //
  //ctor2 direction = (col.transform.position - transform.position).normalized;
  //ctor2 knockback = direction * knockbackForce;

  //mageable.OnHit(damage, knockback);
  //
  //
}
