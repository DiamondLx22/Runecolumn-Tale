using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public Image healthBarImage;

    private Rigidbody2D rb;
    
    void Start()
    {
        rb =  GetComponent<Rigidbody2D>();
        
        currentHealth = maxHealth;
        UpdateHealthBar();
    }


    void UpdateHealthBar()
    {
        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }


    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Debug.Log("Player ist tot!");

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


    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        UpdateHealthBar();
    }
}

