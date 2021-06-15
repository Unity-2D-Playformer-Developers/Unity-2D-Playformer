using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class defining behaviour of projectiles spawned when player uses ranged attack.
/// </summary>
public class CarrotProjectile : MonoBehaviour
{

    public int ProjectileDamage = 10;
    public int ProjectileThrowStrength = 10;

    /// <summary>
    /// components references
    /// </summary>
    private Rigidbody2D rb2d;
    private SpriteRenderer sprite;
    private ParticleSystem destroyParticle;
    private CapsuleCollider2D carrotCollider;
  

    void Start()
    {
        float throwDirection = 1f;


        transform.parent = null;
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        carrotCollider = GetComponent<CapsuleCollider2D>();

        destroyParticle = GetComponentInChildren<ParticleSystem>();

        if (GameManager.Instance.GetIsPlayerFacingLeft == true)
        {
            throwDirection = -1f;
            sprite.flipY = true;
            Debug.LogWarning("flip");
        }
        rb2d.AddForce(new Vector2((10f* throwDirection), 0f), ForceMode2D.Impulse);


    }

    /// <summary>
    /// method checking if projectile collided with damageable object
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<IDamageable>() != null)
        {
            collision.collider.GetComponent<IDamageable>().TakeDamage(ProjectileDamage);         
        }
        StartCoroutine(DestroyEffect());
    }

    /// <summary>
    /// method for spawning particles, after projectiles collides with something
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyEffect()
    {
        carrotCollider.enabled = false;
        sprite.enabled = false;
        destroyParticle.Play();
        yield return new WaitForSeconds(destroyParticle.main.startLifetime.constantMax);
        Destroy();

    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
