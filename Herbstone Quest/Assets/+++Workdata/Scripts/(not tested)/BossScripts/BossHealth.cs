using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    // Maximale Gesundheit des Bosses
    public int maxHealth = 200;
    
    // Aktuelle Gesundheit des Bosses
    private int currentHealth;

    // Referenz auf das UI-Slider-Element für die Anzeige der Gesundheit
    public Slider healthSlider;

    private void Start()
    {
        // Setzt die aktuelle Gesundheit auf die maximale Gesundheit
        currentHealth = maxHealth;

        // Initialisiert den Health Slider
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // Methode zum Zufügen von Schaden
    public void TakeDamage(int damage)
    {
        // Verringert die Gesundheit um den Schadenswert
        currentHealth -= damage;

        // Prüft, ob die Gesundheit 0 oder weniger erreicht hat
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        // Aktualisiert die UI-Anzeige
        UpdateHealthUI();
    }

    // Methode, die aufgerufen wird, wenn der Boss stirbt
    private void Die()
    {
        // Hier könnte man den Boss tot machen, z.B. die Spielfigur deaktivieren
        Debug.Log("Boss died!");
        Destroy(gameObject); // Optional: Boss GameObject zerstören
    }

    // Methode zum Aktualisieren der UI-Anzeige
    private void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
    }
}
