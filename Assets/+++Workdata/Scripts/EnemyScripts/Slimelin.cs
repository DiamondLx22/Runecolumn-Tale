using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

  public Image healthBar;
  public Animator[] meleeAnim;
  public Animator[] anim;

  private bool canMeleeAttack;
  
  public void Start()
  {
    currentHealth = maxHealth;
    rb = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    
    animator = GetComponent<Animator>();

    slimelinDetect.OnTargetEnterAttackRange += HandleTargetEnterAttackRange;
    slimelinDetect.OnTargetExitAttackRange += HandleTargetExitAttackRange;
  }

  private void HandleTargetEnterAttackRange(Collider2D target)
  {
    canMeleeAttack = true;
    HandleMeleeAttack(target);
  }

  private void HandleTargetExitAttackRange(Collider2D target)
  {
    canMeleeAttack = false;
  }
  
  private void HandleMeleeAttack(Collider2D target)
  {
    if (canMeleeAttack)
    {
      for (int i = 0; i < meleeAnim.Length; i++)
      {
        meleeAnim[i].gameObject.SetActive(true);
        EnemyAttackBehaviour enemyAttackBehaviour = meleeAnim[i].GetComponent<EnemyAttackBehaviour>();
        if (enemyAttackBehaviour != null)
        {
          enemyAttackBehaviour.StartAttack();
        }
        
        Vector2 moveDirection = target.transform.position - transform.position;
        meleeAnim[i].SetFloat("dirX", moveDirection.x); //anim[0].GetFloat("dirX"));
        meleeAnim[i].SetFloat("dirY", moveDirection.y); //anim[0].GetFloat("dirY"));
      }

      for (int i = 0; i < anim.Length; i++)
      {
        anim[i].SetTrigger("meleeAttack");
      }
    }
  }

  public void ApplyDamage(float damage)
  {
    currentHealth -= damage;
    spriteRenderer.color = hitColor;
    Invoke("ResetColor", hitColorTime);
    
    healthBar.fillAmount = GetHealthNormalized();
    
    if (currentHealth <= 0f)
    {
      Die();
    }
  }
  
  public void ApplyKnockback(Vector2 knockback)
  {
    //Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null)
    {
      rb.isKinematic = false;
      rb.AddForce(knockback, ForceMode2D.Impulse);
      rb.isKinematic = true;
      Invoke("ResetVelocity", 1);
    }
  }

  private void ResetVelocity()
  {
    rb.velocity = Vector2.zero;
  }
  
  private void ResetColor()
  {
    spriteRenderer.color = normalColor;
  }

  void Die()
  {
    Destroy(gameObject);
  }

  private void OnDestroy()
  {
    slimelinDetect.OnTargetEnterAttackRange -= HandleTargetEnterAttackRange;
    slimelinDetect.OnTargetEnterAttackRange -= HandleTargetExitAttackRange;
  }
  /* public void OnHit(float damage)
   {
     currentHealth -= this.damage;
     spriteRenderer.color = hitColor;
     Invoke("ResetColor", hitColorTime);
     
     if (currentHealth <= 0)
     {
       Die();
     }
   }*/

//public bool canMeleeAttack = true;
//{
//  if (context.performed && canMeleeAttack)
//  {
//    for (int i = 0; i < meleeAnim.Length; i++)
//    {
//      meleeAnim[i].gameObject.SetActive(true);
//      EnemyAttackBehaviour enemyAttackBehaviour = meleeAnim[i].GetComponent<EnemyAttackBehaviour>();
//      if (enemyAttackBehaviour != null)
//      {
//        enemyAttackBehaviour.StartAttack();
//      }
//      meleeAnim[i].SetFloat("dirX", anim[0].GetFloat("dirX"));
//      meleeAnim[i].SetFloat("dirY", anim[0].GetFloat("dirY"));
//    }
//
//    for (int i = 0; i < anim.Length; i++)
//    {
//      anim[i].SetTrigger("meleeAttack");
//    }
//
//    canMeleeAttack = false;
//  }
//}


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
       Collider2D target = null;
       foreach (var obj in slimelinDetect.detectObjects)
       {
         target = obj;
         break;
       }

       if (target != null)
       {

         Vector2 direction = Vector2.MoveTowards(transform.position,
           slimelinDetect.detectObjects.First().transform.position, moveSpeed * Time.deltaTime);

         if (Vector2.Distance(direction, slimelinDetect.detectObjects.First().transform.position) < 1.2f)
         {
           return;
         }

         Vector2 moveDirection = target.transform.position - transform.position;
         animator.SetFloat("dirX", moveDirection.x);
         animator.SetFloat("dirY", moveDirection.y);
         transform.position = direction;
       }
     }
   }

   private float GetHealthNormalized()
  {
    return currentHealth / maxHealth;
  }
}
