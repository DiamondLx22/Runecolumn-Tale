using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
   private WeaponBehaviour weaponBehaviour;

   private void Start()
   {
      weaponBehaviour = transform.parent.GetComponent<WeaponBehaviour>();
   }

   private bool calledHit = false;
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (!calledHit)
      {
         weaponBehaviour.ColliderHit(other);
         calledHit = true; 
      }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      calledHit = false;
   }
}
