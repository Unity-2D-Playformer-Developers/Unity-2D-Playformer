using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBoxBehaviour : MonoBehaviour, IDamageable
{
    public GameObject BoxContent;
    public bool ContentRandomAmount = true;
    public int ContentMaxAmount = 5;
    public Transform ContentSpawnPosition;

    private BoxCollider2D boxCollider;
    private SpriteRenderer sprite;
    private ParticleSystem destroyParticle;


    void Start()
    {
        destroyParticle = GetComponentInChildren<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damageAmount)
    {
        StartCoroutine(DestroyEffect());
    }

    IEnumerator DestroyEffect()
    {
        destroyParticle.Play();
        sprite.enabled = false;
        boxCollider.enabled = false;
        SpawnContent();
        yield return new WaitForSeconds(destroyParticle.main.startLifetime.constantMax);
        Destroy();
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    void SpawnContent()
    {
        int spawnAmount;

        if (ContentRandomAmount)
        {
            spawnAmount = Random.Range(1, ContentMaxAmount);
        }
        else
        {
            spawnAmount = ContentMaxAmount;
        }

        for (int i = spawnAmount; i > 0; i--)
        {
            GameObject pickup = Instantiate(BoxContent, ContentSpawnPosition);
            pickup.GetComponent<PickupBehaviour>().SpawnAndRandomThrow();
            pickup.transform.parent = null;
        }

    }

    public void Knockback(Vector3 damagingObjectPosition, Vector2 knockbackForce)
    {
        throw new System.NotImplementedException();
    }
}
