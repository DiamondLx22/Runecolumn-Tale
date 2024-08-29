using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public Animator[] anim;

    public void EndAttack()
    {
        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].gameObject.SetActive(false);
        }
    }
}
