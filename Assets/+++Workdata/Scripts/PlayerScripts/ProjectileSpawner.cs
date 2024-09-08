using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab des Projektils
    public Transform spawnPosition;
    public Transform alternativeSpawnPosition;
    public float projectileSpeed = 10f; // Geschwindigkeit des Projektils
    public float projectileLifetime = 5f; // Lebensdauer des Projektils

    private Vector2 mousePosition;

    private Player_InputActions inputActions;

    public Vector2 targetDirection;

    private void Awake()
    {
        inputActions = new Player_InputActions();
        inputActions.Player.Enable();

        inputActions.Player.MousePosition.performed += OnMousePositionInput;
    }

    private void OnMousePositionInput(InputAction.CallbackContext context)
    {
        //mousePosition = context.ReadValue<Vector2>();
    }

    public void ShootProjectile()
    {
        SpawnProjectile();
    }

    public void SpawnProjectile()
    {
        Vector2 direction = Vector2.zero;
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;
        
        

       /* Vector2 hit = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        print(hit);
        
        if (hit.x < player.transform.position.x)
        {
            //I clicked on the left side of the player
            direction = Vector2.left;
        }
        else if (hit.x >= player.transform.position.x)
        {
            //I clicked on the right side of the player
            direction = Vector2.right;
        }

        if (hit.y < player.transform.position.y)
        {
            //under player
            direction = Vector2.down;
        }
        else if (hit.y > player.transform.position.y)
        {
            //above Player
            direction = Vector2.up;
        }*/


        GameObject projectile = Instantiate(projectilePrefab, spawnPosition.position, Quaternion.identity);
     
        if (direction.y == 0f)
        {
            projectile.transform.position = alternativeSpawnPosition.position;
            projectile.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
        }

        /* Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
         if (rb != null)
         {
             rb.velocity = aimDirection * projectileSpeed;
         }*/

        // Zugriff auf das Projektil-Skript, um Lebensdauer und Schaden zu setzen
        Projectile projScript = projectile.GetComponentInChildren<Projectile>();
        if (projScript != null)
        {
            //projScript.SetDirection(aimDirection);   // Setzt die Richtung (falls im Projektil-Skript)
            projScript.damage = 10; // Beispielhaft Schaden gesetzt, anpassbar
            projScript.direction = (Vector3)targetDirection;
            projScript.RotateObject();
        }

        // Zerstöre das Projektil nach einer bestimmten Zeit, um Speicher zu sparen
        Destroy(projectile, projectileLifetime);
    }
}