using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    public Animator[] meleeAnim;
    public Animator[] rangeAnim;
    public Animator[] anim;

    public GameObject[] phase1AnimGameObjects;
    public GameObject[] phase2AnimGameObjects;
    public GameObject[] phase3AnimGameObjects;

    public GameObject[] phase1MeleeAnimGameObjects;
    public GameObject[] phase2MeleeAnimGameObjects;
    public GameObject[] phase3MeleeAnimGameObjects;

    public GameObject[] phase1RangeAnimGameObjects;
    public GameObject[] phase2RangeAnimGameObjects;
    public GameObject[] phase3RangeAnimGameObjects;

    public Collider2D hitboxColliderTopDown;
    public Collider2D hitboxColliderRightLeft;

    public bool canMeleeAttack;
    public bool canRangeAttack;
    public float dirX;
    public float dirY;
    
    public GameObject bossprojectilePrefab;  
    public Transform firePoint;  

    [System.Serializable]
    public enum AttackState
    {
        attack1,
        attack2,
        attack3
    }

    public enum MoveState
    {
        move1,
        move2,
        move3
    }

    public MoveState currentMoveState;

    public AttackState currentAttackState;

    private BossHealth bossHealth;

    private Collider2D playerInAttackRange;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        bossHealth = GetComponent<BossHealth>(); // Verknüpfung mit BossHealth-Skript
        UpdateAttackState();
        UpdateMoveState();
        ActivateAnimationObjects();
        ActivateMeleeAttackObjects();
        ActivateRangeAttackObjects();

        BossDetector bossDetect = GetComponentInChildren<BossDetector>();
        bossDetect.OnTargetEnterAttackRange += HandleTargetEnterAttackRange;
        bossDetect.OnTargetExitAttackRange += HandleTargetExitAttackRange;
    }

    private void HandleTargetEnterAttackRange(Collider2D target)
    {
        playerInAttackRange = target;
        canMeleeAttack = true;
        HandleMeleeAttack(target);
    }

    private void HandleTargetExitAttackRange(Collider2D target)
    {
        if (target == playerInAttackRange)
        {
            playerInAttackRange = null;
            canMeleeAttack = false;
            canRangeAttack = false;
            EndAttack();
        }
    }

    private void HandleMeleeAttack(Collider2D target)
    {
        if (canMeleeAttack)
        {
            StartMeleeAttack();
            ColliderHit(target);

            for (int i = 0; i < meleeAnim.Length; i++)
            {
                meleeAnim[i].gameObject.SetActive(true);
                BossAttackBehaviour enemyAttackBehaviour = meleeAnim[i].GetComponent<BossAttackBehaviour>();
                if (enemyAttackBehaviour != null)
                {
                    enemyAttackBehaviour.StartAttack(new Vector2(dirX, dirY));
                }

                Vector2 moveDirection = target.transform.position - transform.position;
                meleeAnim[i].SetFloat("dirX", moveDirection.x);
                meleeAnim[i].SetFloat("dirY", moveDirection.y);
            }

            TriggerAttackAnimation("meleeAttack"); // Angriff nur triggern, wenn tatsächlich angegriffen wird
        }
    }

    private void HandleRangeAttack(Collider2D target)
    {
        if (canRangeAttack && target == playerInAttackRange)
        {
            StartRangeAttack();
            ColliderHit(target);

            for (int i = 0; i < rangeAnim.Length; i++)
            {
                rangeAnim[i].gameObject.SetActive(true);
                BossAttackBehaviour enemyAttackBehaviour = rangeAnim[i].GetComponent<BossAttackBehaviour>();
                if (enemyAttackBehaviour != null)
                {
                    enemyAttackBehaviour.StartRangeAttack(new Vector2(dirX, dirY));
                }

                Vector2 moveDirection = target.transform.position - transform.position;
                rangeAnim[i].SetFloat("dirX", moveDirection.x);
                rangeAnim[i].SetFloat("dirY", moveDirection.y);
            }

            TriggerAttackAnimation("rangeAttack"); // Angriff nur triggern, wenn tatsächlich angegriffen wird

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
        // Animation und Schaden basierend auf dem Angriffszustand setzen
        switch (currentAttackState)
        {
            case AttackState.attack1:
                meleeDamage = 10f;  // Geringster Schaden
                animator.SetTrigger("HuginMeleeAttack");
                break;

            case AttackState.attack2:
                meleeDamage = 20f;  // Mittlerer Schaden
                animator.SetTrigger("Attack2");
                break;

            case AttackState.attack3:
                meleeDamage = 30f;  // Höchster Schaden
                animator.SetTrigger("Attack3");
                break;
        }
    }

    public void StartRangeAttack()
    {
        switch (currentAttackState)
        {
            case AttackState.attack1:
                rangeDamage = 15f;
                animator.SetTrigger("RangeAttack1");
                break;

            case AttackState.attack2:
                rangeDamage = 25f;
                animator.SetTrigger("RangeAttack2");
                break;

            case AttackState.attack3:
                rangeDamage = 35f;
                animator.SetTrigger("RangeAttack3");
                break;
        }

        // Hitbox basierend auf Richtung aktivieren
        if (dirX != 0)
        {
            hitboxColliderRightLeft.enabled = true;
            Vector2 offset = hitboxColliderRightLeft.offset;
            offset.x = Mathf.Abs(offset.x) * (dirX > 0 ? 1 : -1);
            hitboxColliderRightLeft.offset = offset;
        }
        else if (dirY != 0)
        {
            hitboxColliderTopDown.enabled = true;
            Vector2 offset = hitboxColliderTopDown.offset;
            offset.y = Mathf.Abs(offset.y) * (dirY > 0 ? 1 : -1);
            hitboxColliderTopDown.offset = offset;
        }
    }

    private void TriggerAttackAnimation(string attackType)
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].SetTrigger(attackType);
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

    public void UpdateMoveState()
    {
        float healthPercentage = bossHealth.GetHealthPercentage();

        if (healthPercentage > 0.66f)
        {
            currentMoveState = MoveState.move1;
            animator.SetTrigger("HuginMoveState1");
        }
        else if (healthPercentage > 0.33f)
        {
            currentMoveState = MoveState.move2;
            animator.SetTrigger("MuninMoveState1");
        }
        else
        {
            currentMoveState = MoveState.move3;
            animator.SetTrigger("HeavyMove");
        }

        ActivateAnimationObjects();
        ActivateMeleeAttackObjects();
        ActivateRangeAttackObjects();
    }

    private void ActivateAnimationObjects()
    {
        foreach (GameObject obj in phase1AnimGameObjects)
        {
            obj.SetActive(currentMoveState == MoveState.move1);
        }
        foreach (GameObject obj in phase2AnimGameObjects)
        {
            obj.SetActive(currentMoveState == MoveState.move2);
        }
        foreach (GameObject obj in phase3AnimGameObjects)
        {
            obj.SetActive(currentMoveState == MoveState.move3);
        }
    }

    public void ActivateMeleeAttackObjects()
    {
        foreach (GameObject obj in phase1MeleeAnimGameObjects)
        {
            obj.SetActive(currentMoveState == MoveState.move1);
        }
        foreach (GameObject obj in phase2MeleeAnimGameObjects)
        {
            obj.SetActive(currentMoveState == MoveState.move2);
        }
        foreach (GameObject obj in phase3MeleeAnimGameObjects)
        {
            obj.SetActive(currentMoveState == MoveState.move3);
        }
    }

    public void ActivateRangeAttackObjects()
    {
        foreach (GameObject obj in phase1RangeAnimGameObjects)
        {
            obj.SetActive(currentMoveState == MoveState.move1);
        }
        foreach (GameObject obj in phase2RangeAnimGameObjects)
        {
            obj.SetActive(currentMoveState == MoveState.move2);
        }
        foreach (GameObject obj in phase3RangeAnimGameObjects)
        {
            obj.SetActive(currentMoveState == MoveState.move3);
        }
    }
    
    private void DeactivateAttackObjects()
    {
        foreach (Animator anim in meleeAnim)
        {
            anim.gameObject.SetActive(false); // Deaktivieren der Child-Objekte nach Angriff
        }

        foreach (Animator anim in rangeAnim)
        {
            anim.gameObject.SetActive(false); // Deaktivieren der Child-Objekte nach Angriff
        }
    }
    
    public void EndAttack()
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].ResetTrigger("meleeAttack");
            anim[i].SetTrigger("StopAttack");  
            DeactivateAttackObjects();
        }

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

    private void OnDestroy()
    {
        BossDetector bossDetect = GetComponentInChildren<BossDetector>();
        bossDetect.OnTargetEnterAttackRange -= HandleTargetEnterAttackRange;
        bossDetect.OnTargetExitAttackRange -= HandleTargetExitAttackRange;
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
        if (bossDetect.detectObjects.Count > 0)
        {
            Collider2D target = null;
            foreach (var obj in bossDetect.detectObjects)
            {
                target = obj;
                break;
            }

            if (target != null)
            {
                Vector2 direction = Vector2.MoveTowards(transform.position,
                    bossDetect.detectObjects.First().transform.position, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(direction, bossDetect.detectObjects.First().transform.position) < 1.2f)
                {
                    HandleMeleeAttack(target);
                }
                else
                {
                    canRangeAttack = true;
                    HandleRangeAttack(target);
                }

                Vector2 moveDirection = target.transform.position - transform.position;
                animator.SetFloat("dirX", moveDirection.x);
                animator.SetFloat("dirY", moveDirection.y);
                transform.position = direction;
            }
            UpdateMoveState();
        }
    }
}
