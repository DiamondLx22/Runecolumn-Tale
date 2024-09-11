using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBossAttack : MonoBehaviour
{
    private BossDetector bossDetector;

    private void Start()
    {
        bossDetector = transform.parent.GetComponent<BossDetector>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null && bossDetector.tagTargets.Contains(collider.gameObject.tag))
        {
            bossDetector.TriggerOnTargetEnterAttackRangeEvent(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null && bossDetector.tagTargets.Contains(collider.gameObject.tag))
        {
            bossDetector.TriggerOnTargetExitAttackRangeEvent(collider);
        }
    }
}
