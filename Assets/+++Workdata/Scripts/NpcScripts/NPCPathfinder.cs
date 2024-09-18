using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPathfinder : MonoBehaviour
{
    public Collider2D col;
    public float moveSpeed = 2.0f;

    public Transform[] points;

    private int currentTargetIndex;

    private bool isInteracting = false;
  

    // Animator für den NPC
    private Animator animator;

    void Start()
    {
        //col = GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError("DetectorCollider nicht verknüpft.");
        }
        

        
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
        Vector3 nextTarget = points[currentTargetIndex].position;
        Vector3 direction = nextTarget - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, nextTarget, moveSpeed * Time.deltaTime);

        if (transform.position == nextTarget)
        {
            currentTargetIndex += 1; 
            if (currentTargetIndex >= points.Length)
            {
                currentTargetIndex = 0;
            }
        }
        
        UpdateAnimation(direction);
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