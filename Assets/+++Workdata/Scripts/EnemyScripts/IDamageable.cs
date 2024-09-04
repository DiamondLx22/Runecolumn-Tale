using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float Health { set; get; }
    void OnHit(Vector2 knockback);
    void OnHit(float damage);
    void OnHit(float damage, Vector2 knockback);
}
