using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathfinder : MonoBehaviour
{
    public Collider2D col;
    public float moveSpeed = 2.0f;
    public float obstacleAvoidanceDistance = 0.5f; // Abstand zu Hindernissen

    public Transform pointA;
    public Transform pointB;
    private Vector3 nextDestination;

    private bool isInteracting = false;
    private Vector3 lastPosition;

    // Animator für den NPC
    private Animator animator;

    void Start()
    {
        //col = GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError("DetectorCollider nicht verknüpft.");
        }

        nextDestination = pointA.position;
        lastPosition = transform.position; // Speichere die aktuelle Position

        // Animator-Komponente holen
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator nicht verknüpft.");
        }
    }

    void Update()
    {
        if (!isInteracting)
        {
            FollowRoute();  // Route von A nach B ablaufen
        }
    }

    // Funktion, um den NPC von A nach B zu bewegen
    private void FollowRoute()
    {
        // Wenn das Ziel erreicht ist, wechsle zum nächsten Punkt
        if (Vector3.Distance(transform.position, nextDestination) < 0.1f)
        {
            nextDestination = nextDestination == pointA.position ? pointB.position : pointA.position;
        }

        // Bewege den NPC in Richtung des nächsten Ziels
        Vector3 movementDirection = (nextDestination - transform.position).normalized;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, nextDestination, moveSpeed * Time.deltaTime);

        // Verhindere Kollisionen mit Hindernissen
        newPosition = AvoidObstacles(movementDirection, newPosition);

        // Setze die Position des NPCs
        transform.position = newPosition;

        // Nutze die Bewegungsrichtung für die Animation
        UpdateAnimation(movementDirection);

        // Speichere die aktuelle Position für das nächste Frame
        lastPosition = transform.position;
    }

    // Vermeide Hindernisse, indem ein Mindestabstand eingehalten wird
    private Vector3 AvoidObstacles(Vector3 movementDirection, Vector3 desiredPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, movementDirection, obstacleAvoidanceDistance);

        // Wenn ein Hindernis erkannt wird, weiche seitlich aus
        if (hit.collider != null && hit.collider != col && hit.collider != GetComponent<Collider2D>())
        {
            // Berechne die Richtung des Ausweichens (senkrecht zur Bewegungsrichtung)
            Vector3 avoidDirection = Vector3.Cross(movementDirection, Vector3.forward).normalized;
            desiredPosition += avoidDirection * moveSpeed * Time.deltaTime;
        }

        return desiredPosition;
    }

    // Funktion zum Stoppen der Bewegung bei Interaktionen
    public void StartInteraction()
    {
        isInteracting = true;  // Stoppt die Bewegung während der Interaktion
    }

    // Funktion zum Fortsetzen der Bewegung nach Interaktion
    public void EndInteraction()
    {
        isInteracting = false;  // Setzt die Bewegung nach der Interaktion fort
    }

    // Funktion zum Aktualisieren der Animation basierend auf der Bewegungsrichtung
    private void UpdateAnimation(Vector3 movementDirection)
    {
        if (animator != null)
        {
            // Setze die Bewegungsrichtung in den Animator (Horizontal = X, Vertical = Y)
            animator.SetFloat("Horizontal", movementDirection.x);  
            animator.SetFloat("Vertical", movementDirection.y);    
        }
    }
}