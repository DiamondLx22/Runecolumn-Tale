using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDetector : MonoBehaviour
{
    public List<string> tagTargets = new List<string> { "Player", "AlphaSlimelin" };
    public HashSet<Collider2D> detectObjects = new HashSet<Collider2D>();
    public Collider2D col;
    public Collider2D attackRangeCollider;

    private Transform parentTransform;

    public event Action<Collider2D> OnTargetEnterAttackRange;
    public event Action<Collider2D> OnTargetExitAttackRange;

    public GameObject enemyCanvas;

    void Start()
    {
        enemyCanvas.SetActive(false);
        col = GetComponent<Collider2D>();
        parentTransform = transform.parent;
    }

    void Update()
    {
        if (parentTransform != null)
        {
            transform.position = parentTransform.position;  // Update der Position des Detektors
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && tagTargets.Contains(collider.gameObject.tag))
        {
            detectObjects.Add(collider);
            enemyCanvas.SetActive(true);
            OnTargetEnterAttackRange?.Invoke(collider);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null && tagTargets.Contains(collider.gameObject.tag))
        {
            detectObjects.Remove(collider);
            enemyCanvas.SetActive(false);
            OnTargetExitAttackRange?.Invoke(collider);
        }
    }
}
