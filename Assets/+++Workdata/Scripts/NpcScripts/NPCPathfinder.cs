using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathfinder : MonoBehaviour
{
    public Collider2D col;
    public float moveSpeed = 2.0f;
    public float obstacleAvoidanceDistance = 0.5f;

    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;
    
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
        lastPosition = transform.position;

        
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
            FollowRoute();  
        }
    }
    
    private void FollowRoute()
    {
        // Wenn das Ziel erreicht ist, wechsle zum nächsten Punkt
        if (Vector3.Distance(transform.position, nextDestination) < 0.1f)
        {
            nextDestination =
                nextDestination == pointA.position ? pointB.position :
                nextDestination == pointB.position ? pointC.position :
                nextDestination == pointC.position ? pointD.position : pointA.position;
        }

        // Bewege den NPC in Richtung des nächsten Ziels
        Vector3 movementDirection = (nextDestination - transform.position).normalized;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, nextDestination, moveSpeed * Time.deltaTime);

        
        newPosition = AvoidObstacles(movementDirection, newPosition);

        
        transform.position = newPosition;

        
        UpdateAnimation(movementDirection);

        
        lastPosition = transform.position;
    }

    
    private Vector3 AvoidObstacles(Vector3 movementDirection, Vector3 desiredPosition)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, movementDirection, obstacleAvoidanceDistance);
       
        foreach (RaycastHit2D hit in hits)
        {
            
            if (hit.collider != null && hit.collider != col && hit.collider != GetComponent<Collider2D>())
            {
                
                Vector3 avoidDirection = Vector3.Cross(movementDirection, Vector3.forward).normalized;
                desiredPosition += avoidDirection * moveSpeed * Time.deltaTime;
            }
        }

        return desiredPosition;
    }

    
    public void StartInteraction()
    {
        isInteracting = true;  
    }

    
    public void EndInteraction()
    {
        isInteracting = false;  
    }

    
    private void UpdateAnimation(Vector3 movementDirection)
    {
        if (animator != null)
        {
            
            animator.SetFloat("Horizontal", movementDirection.x);  
            animator.SetFloat("Vertical", movementDirection.y);    
        }
    }
}