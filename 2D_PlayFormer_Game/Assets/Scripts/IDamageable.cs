using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(int damageAmount);
    public void Destroy();
    public void Knockback(float knockDur,float knockbackPwr,Vector3 knockbackDir);
}
