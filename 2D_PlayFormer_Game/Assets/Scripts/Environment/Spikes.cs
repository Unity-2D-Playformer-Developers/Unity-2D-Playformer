using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class defining behaviour of spikes.
/// </summary>
public class Spikes : MonoBehaviour
{

    void Start()
    {

    }

    /// <summary>
    /// Deals damage to player when he collider with spikes.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageableObject = collision.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(1);
            damageableObject.Knockback(transform.position, new Vector2(5f,10f));
            Debug.Log("player on spikes");
        }
    }
}
