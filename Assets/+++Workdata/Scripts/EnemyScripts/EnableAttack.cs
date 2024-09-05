using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAttack : MonoBehaviour
{
    private SlimelinDetector slimelinDetector;

    private void Start()
    {
        slimelinDetector = transform.parent.GetComponent<SlimelinDetector>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && slimelinDetector.tagTargets.Contains(collider.gameObject.tag))
        {
            slimelinDetector.TriggerOnTargetEnterAttackRangeEvent(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null && slimelinDetector.tagTargets.Contains(collider.gameObject.tag))
        {
            slimelinDetector.TriggerOnTargetExitAttackRangeEvent(collider);
        }
    }
}