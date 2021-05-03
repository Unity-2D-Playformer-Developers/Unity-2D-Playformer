using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Spikes : NetworkBehaviour
{

    void Start()
    {

    }

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
