using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
      public int maxHealth = 100;  // Maximale Gesundheit des Bosses
    private int currentHealth;   // Aktuelle Gesundheit des Bosses

    public Transform[] attackPoints;  // Positionen, von denen der Boss seine Projektile abschießt
    public GameObject projectilePrefab;  // Projektil-Objekt, das der Boss abschießt
    public float attackCooldown = 2f;  // Abklingzeit zwischen Angriffen

    public GameObject[] armorParts;  // Rüstungsteile, die der Boss verliert
    private int armorIndex = 0;      // Index des aktuellen Rüstungsteils

    private int attackIndex = 0;  // Verfolgt, welcher Angriff als nächstes ausgeführt wird
    private Animator animator;    // Referenz zum Animator-Component

    private void Start()
    {
        currentHealth = maxHealth;  // Setzt die aktuelle Gesundheit auf die maximale Gesundheit
        animator = GetComponent<Animator>();  // Holt den Animator-Component
        StartCoroutine(AttackRoutine());  // Startet die Angriffsroutine
    }

    private IEnumerator AttackRoutine()
    {
        while (true)  // Endlose Schleife für kontinuierliche Angriffe
        {
            PerformAttack();  // Führt den aktuellen Angriff aus

            // Boss wird nach jedem Angriff stärker
            currentHealth += 10;

            // Entfernt ein Rüstungsteil nach dem Angriff
            RemoveArmorPart();

            // Warte für die festgelegte Abklingzeit, bevor der nächste Angriff startet
            yield return new WaitForSeconds(attackCooldown);

            // Aktualisiert den Angriffstyp für den nächsten Angriff
            attackIndex = (attackIndex + 1) % 3;
        }
    }

    private void PerformAttack()
    {
        switch (attackIndex)
        {
            case 0:
                // Angriff 1: Einfacher Projektilangriff
                animator.SetTrigger("Attack1");
                LaunchProjectiles(1);
                break;
            case 1:
                // Angriff 2: Streuschuss
                animator.SetTrigger("Attack2");
                LaunchProjectiles(3);
                break;
            case 2:
                // Angriff 3: Schnellfeuer
                animator.SetTrigger("Attack3");
                LaunchProjectiles(5);
                break;
        }
    }

    private void LaunchProjectiles(int projectileCount)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            foreach (var attackPoint in attackPoints)
            {
                // Erzeugt das Projektil an der angegebenen Angriffspunktposition
                Instantiate(projectilePrefab, attackPoint.position, attackPoint.rotation);
            }
        }
    }

    private void RemoveArmorPart()
    {
        if (armorIndex < armorParts.Length)
        {
            armorParts[armorIndex].SetActive(false);  // Deaktiviert das Rüstungsteil
            armorIndex++;  // Erhöht den Index, um das nächste Rüstungsteil zu entfernen
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;  // Verringert die Gesundheit des Bosses
        animator.SetTrigger("TakeDamage");
        if (currentHealth <= 0)
        {
            Die();  // Führt die Sterbelogik aus, wenn die Gesundheit auf 0 oder weniger fällt
        }
    }

    private void Die()
    {
        // Spielt die Sterbeanimation ab und zerstört das GameObject nach einer Verzögerung
        animator.SetTrigger("Die");
        Destroy(gameObject, 1.5f);  // Zerstört das GameObject nach 1.5 Sekunden
    }
}
