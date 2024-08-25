using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Maximale Gesundheit des Spielers
    public int maxHealth = 100;
    
    // Aktuelle Gesundheit des Spielers
    private int currentHealth;

    // Referenz auf das UI-Textfeld für die Anzeige der Gesundheit
    public Text healthText;

    private void Start()
    {
        // Setzt die aktuelle Gesundheit auf die maximale Gesundheit
        currentHealth = maxHealth;

        // Aktualisiert die UI-Anzeige
        UpdateHealthUI();
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

    // Methode zum Heilen
    public void Heal(int healAmount)
    {
        // Erhöht die Gesundheit um den Heilwert, überschreitet aber nicht die maximale Gesundheit
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Aktualisiert die UI-Anzeige
        UpdateHealthUI();
    }

    // Methode, die aufgerufen wird, wenn der Spieler stirbt
    private void Die()
    {
        // Hier könnte man den Spieler tot machen, z.B. die Spielfigur deaktivieren
        Debug.Log("Player died!");
        // Optional: Spielende Logik hier einfügen
    }

    // Methode zum Aktualisieren der UI-Anzeige
    private void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth;
    }
}
