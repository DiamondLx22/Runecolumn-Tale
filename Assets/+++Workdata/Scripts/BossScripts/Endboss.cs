using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Endboss : MonoBehaviour
{
    public float meleeDamage = 10f;
    public float rangeDamage = 10f;
    public float knockbackForce = 5f;
    private Rigidbody2D rb;

    public float hitColorTime = 0.1f;
    public Color32 normalColor;
    public Color32 hitColor;

    private SpriteRenderer spriteRenderer;
    public BossDetector bossDetect;
    private Animator animator;
    public float moveSpeed;

    public Image healthBar;
    
    public Collider2D hitboxColliderTopDown;
    public Collider2D hitboxColliderRightLeft;
    
    public GameObject bossprojectilePrefab;  
    public Transform firePoint;  

    [System.Serializable]
    public enum AttackState
    {
        attack1,
        attack2,
        attack3
    }

    public enum BossForm
    {
        Hugin,
        Munin,
        Skalli
    }

    

    private BossForm currentBossForm = BossForm.Hugin;
    public AttackState currentAttackState;

    private BossHealth bossHealth;

    

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        bossHealth = GetComponent<BossHealth>(); // Verknüpfung mit BossHealth-Skript
        UpdateAttackState();
      

        BossDetector bossDetect = GetComponentInChildren<BossDetector>();
    
    }

    private void HandleTargetEnterAttackRange(Collider2D target)
    {
        HandleAttack(target);
    }

    private void HandleTargetExitAttackRange(Collider2D target)
    {
        if (target)
        {
            EndAttack();
        }
    }

    private void HandleAttack(Collider2D target)
    {
        bool meleeRange = Vector2.Distance(transform.position, target.transform.position) < 3f;

        if (meleeRange)
        {
            StartMeleeAttack();
        }
        else
        {
            animator.SetTrigger("PrepareRangeAttack");
        }
    }

    public void FireBossProjectile()
    {
        Collider2D target = bossDetect.detectObjects.FirstOrDefault();
        if (target != null)
        {
            FireProjectile(target);
        }
    }

    private void FireProjectile(Collider2D target)
    {
        if (bossprojectilePrefab != null && firePoint != null)
        {
            GameObject bossprojectile = Instantiate(bossprojectilePrefab, firePoint.position, Quaternion.identity);
            BossProjectile bossprojectileScript = bossprojectile.GetComponent<BossProjectile>();

            if (bossprojectileScript != null)
            {
                Vector2 attackDirection = new Vector2(animator.GetFloat("dirX"), animator.GetFloat("dirY"));
                bossprojectileScript.Initialize(target.transform, rangeDamage, attackDirection);
            }
        }
    }

    public void StartMeleeAttack()
    {
        switch (currentAttackState)
        {
            case AttackState.attack1:
                meleeDamage = 10f; 
                animator.SetTrigger("HuginsStorm");
                ActivateHitbox();
                break;

            case AttackState.attack2:
                meleeDamage = 20f;
                animator.SetTrigger("Attack2");
                break;

            case AttackState.attack3:
                meleeDamage = 30f;
                animator.SetTrigger("Attack3");
                break;
        }
    }

    private void ActivateHitbox()
    {
        float dirX = animator.GetFloat("dirX");
        float dirY = animator.GetFloat("dirY");

        hitboxColliderTopDown.enabled = false;
        hitboxColliderRightLeft.enabled = false;

        if (Mathf.Abs(dirX) > Mathf.Abs(dirY))
        {
            hitboxColliderRightLeft.enabled = true;
        }
        else
        {
            hitboxColliderTopDown.enabled = true;
        }
    }

    
    public void UpdateAttackState()
    {
        float healthPercentage = bossHealth.GetHealthPercentage();

        if (healthPercentage > 0.66f)
        {
            currentAttackState = AttackState.attack1;
        }
        else if (healthPercentage > 0.33f)
        {
            currentAttackState = AttackState.attack2;
        }
        else
        {
            currentAttackState = AttackState.attack3;
        }

        healthBar.fillAmount = healthPercentage;
    }
    
    public void UpdateBossForm()
    {
        float healthPercentage = bossHealth.GetHealthPercentage();

        BossForm nextBossForm;
        
        if (healthPercentage > 0.66f)
        {
            nextBossForm = BossForm.Hugin;
        }
        else if (healthPercentage > 0.33f)
        {
            nextBossForm  = BossForm.Munin;
        }
        else //if
        {
            nextBossForm  = BossForm.Skalli;
        }
        //else

        bool shouldTransform = nextBossForm != currentBossForm;

        if (shouldTransform)
        {
            switch (nextBossForm)
            {
                case BossForm.Hugin:
                    animator.SetTrigger("HuginTransform");
                    break;
                
                case BossForm.Munin:
                    animator.SetTrigger("MuninTransform");
                    break;
                
                case BossForm.Skalli:
                    animator.SetTrigger("Skalli");
                    break;
            }

            currentBossForm = nextBossForm;
        }
    }
    
    
    public void EndAttack()
    {
        hitboxColliderRightLeft.enabled = false;
        hitboxColliderTopDown.enabled = false;
    }

    
    public void ColliderHit(Collider2D collider)
    {
        PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Vector3 parentPosition = transform.position;
            Vector2 direction = (collider.transform.position - parentPosition).normalized;
            Vector2 knockback = direction * knockbackForce;

            playerHealth.TakeDamage((int)meleeDamage);
            ApplyKnockback(knockback);
        }
        else
        {
            Debug.LogWarning("Collider has no PlayerHealth component");
        }
    }

    public void ApplyKnockback(Vector2 knockback)
    {
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitboxColliderTopDown.enabled || hitboxColliderRightLeft.enabled)
        {
            ColliderHit(GetComponent<Collider2D>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockbackForce;

            enemyHealth.OnHit(meleeDamage, knockback);
        }
    }

    void FixedUpdate()
    {
       
            Collider2D target = bossDetect.detectObjects.FirstOrDefault();

            if (target != null)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    target.transform.position, moveSpeed * Time.deltaTime);
                HandleAttack(target);
            }
            
            Vector2 moveDirection = target.transform.position - transform.position;
            animator.SetFloat("dirX", moveDirection.x);
            animator.SetFloat("dirY", moveDirection.y);
            
            UpdateBossForm();
    }
}
