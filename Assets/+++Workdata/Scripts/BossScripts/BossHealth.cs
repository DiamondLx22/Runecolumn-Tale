using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
      private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    
    public float maxHealth = 500f;  // Höhere Lebenspunkte für einen Boss
    private float currentHealth;
    public Image healthBar;

    public float hitColorTime = 0.1f;
    public Color32 normalColor = new Color32(255, 255, 255, 255);  // Standardfarbe
    public Color32 hitColor = new Color32(255, 0, 0, 255);  // Farbe bei Treffer
    
    private BossController bossController;  // Verknüpfung zum BossController

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        UpdateHealthBar();

        bossController = GetComponent<BossController>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();
        
        // Färbt den Boss bei Schaden rot ein
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
        Debug.Log("Boss died!");
        // Benachrichtige den BossController über den Tod
        if (bossController != null)
        {
            bossController.OnBossDeath();
        }
    }

    // Reagiert auf Kollisionen, z.B. mit dem Spieler
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TakeDamage(10f);  // Schaden bei Kollision mit Spieler
        }
    }
}
