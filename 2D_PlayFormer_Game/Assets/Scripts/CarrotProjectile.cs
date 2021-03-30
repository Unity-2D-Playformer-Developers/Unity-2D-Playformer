using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotProjectile : MonoBehaviour
{

    public int ProjectileDamage = 10;
    public int ProjectileThrowStrength = 10;
    private Rigidbody2D rb2d;
  

    void Start()
    {
        float throwDirection = transform.lossyScale.x;
        transform.parent = null;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddForce(new Vector2((10f* throwDirection), 0f), ForceMode2D.Impulse);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<IDamageable>() != null)
        {
            collision.collider.GetComponent<IDamageable>().Damage(ProjectileDamage);
        }

        Destroy(this.gameObject);
    }
}
