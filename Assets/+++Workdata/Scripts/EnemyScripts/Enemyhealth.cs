using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    public float maxHealth = 100f;
    private float currentHealth;
    public Image healthBar;

    public float hitColorTime;
    public Color32 normalColor;
    public Color32 hitColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Setzt die Gesundheit des Feindes auf den Maximalwert
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    // Diese Methode wird aufgerufen, wenn der Feind Schaden nimmt
    public void TakeDamage(float damage)
    {
        // Verringert die aktuelle Gesundheit
        currentHealth -= damage;

        // Sicherstellen, dass die Gesundheit nicht negativ wird
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Aktualisiert die Healthbar
        UpdateHealthBar();

        // Hier wird �berpr�ft , ob der Feind gestorben ist
        if (currentHealth <= 0f)
        {
            Die();
        }

        spriteRenderer.color = hitColor;
        Invoke("ChangeToNormalColor", hitColorTime);
    }

    private void ChangeToNormalColor()
    {
        spriteRenderer.color = normalColor;
    }
    
    // Diese Methode aktualisiert die Healthbar entsprechend der aktuellen Gesundheit
    void UpdateHealthBar()
    {
        // Setzt die Healthbar-F�llmenge (0 bis 1)
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    // Diese Methode wird aufgerufen, wenn der Feind stirbt
    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            TakeDamage(5);
        }
    }
}