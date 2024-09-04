using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SlimelinDetector : MonoBehaviour
{
    public List<string> tagTargets = new List<string> { "Player", "AlphaSlimelin" };

    public HashSet<Collider2D> detectObjects = new HashSet<Collider2D>();
    public Collider2D col;

    public Collider2D attackRangeCollider;

    public event Action<Collider2D> OnTargetEnterAttackRange;
    public event Action<Collider2D> OnTargetExitAttackRange;

    public float constantDistance = 2.0f;
    
    void Start()
    {
        // Initialize col to be the Collider2D component attached to the same GameObject
        col = GetComponent<Collider2D>();

        if (col == null)
        {
            Debug.LogError("DetectorCollider nicht Verknüpft.");
        }

        if (attackRangeCollider == null)
        {
            Debug.LogError("AttackRangeCollider nicht Verknüpft");
        }
    }
    
    void Update()
    {
        // Überprüfe jedes erfasste Objekt
        foreach (var target in detectObjects)
        {
            if (target != null)
            {
              
                Vector3 directionToTarget = target.transform.position - transform.position;

               
                if (directionToTarget.magnitude < constantDistance)
                {
                    Vector3 newPosition = target.transform.position - directionToTarget.normalized * constantDistance;
                    
                    transform.position = newPosition;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && tagTargets.Contains(collider.gameObject.tag))
        {
            detectObjects.Add(collider);
            
            OnTargetEnterAttackRange?.Invoke(collider);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null && tagTargets.Contains(collider.gameObject.tag))
        {
            detectObjects.Remove(collider);
            
            OnTargetExitAttackRange?.Invoke((collider));
        }
    }
}
