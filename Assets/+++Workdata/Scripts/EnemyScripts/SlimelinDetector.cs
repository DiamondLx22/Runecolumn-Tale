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

    private Transform parentTransform;

    public event Action<Collider2D> OnTargetEnterAttackRange;
    public event Action<Collider2D> OnTargetExitAttackRange;

    public float constantDistance = 2.0f;

    public GameObject enemyCanvas;
    
    void Start()
    {
        enemyCanvas.SetActive(false);
        
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

        parentTransform = transform.parent;

        if (parentTransform == null)
        {
            
            Debug.LogError(("Parent Transform fehlt"));
        }
    }
    
    void Update()
    {
        if (parentTransform != null)
        {
            transform.position = parentTransform.position;
        }
        
        // Überprüfe jedes erfasste Objekt
        /*foreach (var target in detectObjects)
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
        }*/
    }

    public void TriggerOnTargetEnterAttackRangeEvent(Collider2D collider)
    {
        OnTargetEnterAttackRange?.Invoke(collider);
    }
    public void TriggerOnTargetExitAttackRangeEvent(Collider2D collider)
    {
        OnTargetExitAttackRange?.Invoke((collider));
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && tagTargets.Contains(collider.gameObject.tag))
        {
            detectObjects.Add(collider);
            enemyCanvas.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null && tagTargets.Contains(collider.gameObject.tag))
        {
            detectObjects.Remove(collider);
            enemyCanvas.SetActive(false);
        }
    }
}
