using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageableObject = collision.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.Damage(1);
            damageableObject.Knockback(new Vector2(transform.position.x, transform.position.y), new Vector2(5f,10f));
            Debug.Log("player on spikes");
        }
    }
}
