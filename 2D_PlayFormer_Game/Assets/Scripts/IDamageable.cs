using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damageAmount);
    public void Destroy();
    public void Knockback(Vector3 damagingObjectPosition, Vector2 knockbackForce);
}
