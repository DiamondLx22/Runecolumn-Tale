using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    
    public float maxHealth = 100f;
    private float currentHealth;
    public Image healthBar;

    public float hitColorTime;
    public Color32 normalColor;
    public Color32 hitColor;
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();
        
        spriteRenderer.color = hitColor;
        Invoke("ChangeToNormalColor", hitColorTime);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    

    public void OnHit(float damage)
    {
        TakeDamage(damage);
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        TakeDamage(damage);
        ApplyKnockback(knockback);
    }

    private void ApplyKnockback(Vector2 knockback)
    {
        if (rb != null)
        {
            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }

    private void ChangeToNormalColor()
    {
        spriteRenderer.color = normalColor;
    }
    
   
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            TakeDamage(5f);
        }
    }
}