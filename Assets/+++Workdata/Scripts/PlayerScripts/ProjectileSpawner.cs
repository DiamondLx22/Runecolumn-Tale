using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;       // Prefab des Projektils
    public float projectileSpeed = 10f;       // Geschwindigkeit des Projektils
    public float projectileLifetime = 5f;     // Lebensdauer des Projektils

    public void SpawnProjectile()
    {
        // Hole die Mausposition in der Welt
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Berechne die Richtung des Projektils
        Vector2 aimDirection = (mousePosition - (Vector2)transform.position).normalized;

        // Instanziiere das Projektil an der Position des Spielers
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Falls das Projektil einen Rigidbody2D hat, setzen wir seine Geschwindigkeit auf die Richtung
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = aimDirection * projectileSpeed;  // Setze Geschwindigkeit des Projektils
        }

        // Optional: Setze die Rotation des Projektils basierend auf der Richtung
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Zugriff auf das Projektil-Skript, um Lebensdauer und Schaden zu setzen
        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.SetDirection(aimDirection);   // Setzt die Richtung (falls im Projektil-Skript)
            projScript.damage = 10;                  // Beispielhaft Schaden gesetzt, anpassbar
            projScript.lifetime = projectileLifetime; // Setze die Lebensdauer
        }

        // Zerstöre das Projektil nach einer bestimmten Zeit, um Speicher zu sparen
        Destroy(projectile, projectileLifetime);
    }
}
