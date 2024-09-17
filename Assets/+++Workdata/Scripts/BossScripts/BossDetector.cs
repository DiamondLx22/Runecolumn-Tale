using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BossDetector : MonoBehaviour
{
    public List<string> tagTargets = new List<string> { "Player"};
    public HashSet<Collider2D> detectObjects = new HashSet<Collider2D>();
    private Transform parentTransform;
    
    
    public float constantDistance = 2.0f;

    public GameObject bossCanvas;

    void Start()
    {
        bossCanvas.SetActive(false);

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && tagTargets.Contains(collider.gameObject.tag))
        {
            detectObjects.Add(collider);
            bossCanvas.SetActive(true);
            //OnTargetEnterAttackRange?.Invoke(collider);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null && tagTargets.Contains(collider.gameObject.tag))
        {
            detectObjects.Remove(collider);
            
            if (tagTargets.Count == 0)
            {
                bossCanvas.SetActive(false);
            }
            //OnTargetExitAttackRange?.Invoke(collider);
        }
    }
    
}
