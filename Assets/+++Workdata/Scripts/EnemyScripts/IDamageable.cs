using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void OnHit(float damage);
    void OnHit(float damage, Vector2 knockback);
}
