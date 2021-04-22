using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(int damageAmount);
    public void Destroy();
    public void Knockback(Vector2 damagingObjectPosition, Vector2 knockbackForce);
}
