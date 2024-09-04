using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{ 
        private EnemyAttackBehaviour enemyAttackBehaviour;

        private void Start()
        {
            enemyAttackBehaviour = transform.parent.GetComponent<EnemyAttackBehaviour>();
            if (enemyAttackBehaviour == null)
            {
                Debug.LogError("EnemyAttackBehaviour fehlt");
            }
        }

        private bool calledHit = false;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!calledHit && enemyAttackBehaviour != null)
            {
                enemyAttackBehaviour.ColliderHit(other);
                calledHit = true; 
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            calledHit = false;
        }
}


