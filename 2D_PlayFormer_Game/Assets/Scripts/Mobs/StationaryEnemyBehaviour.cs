using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class managing enemy behaviour/
/// </summary>
public class StationaryEnemyBehaviour : MonoBehaviour, IDamageable
{
    public int AttackDamage;
    public float MaxHealth;
    public float AttackDelay;

    private float health;
    private bool isAttacking=false;
    private bool playerInAttackRange = false;
    private Animator animator;
    private SpriteRenderer sprite;
    private CapsuleCollider2D attackRange;
    private BoxCollider2D enemyCollider;
    private IDamageable damageableObject;

    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attackRange = GetComponent<CapsuleCollider2D>();
        enemyCollider = GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        
    }

    /// <summary>
    /// triggers attack after players enters view range
    /// </summary>
    /// <returns></returns>
    IEnumerator PrepareAttack()
    {
        yield return new WaitForSeconds(AttackDelay);
        if(damageableObject!=null)
        {
            animator.SetTrigger("Attack");   
        }
        yield return 0;
    }

    /// <summary>
    /// checks if player is in range - delas damage if true.
    /// </summary>
    public void Attack()
    {
        if (damageableObject != null && playerInAttackRange)
        {
            damageableObject.TakeDamage(AttackDamage);
            damageableObject.Knockback(transform.position, new Vector2(8f, 2f));
        }
        isAttacking = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        damageableObject = collision.GetComponent<IDamageable>();
        if (collision.tag ==  "Player" && damageableObject!=null && !isAttacking)
       {
            playerInAttackRange = true;
            isAttacking = true;
            StartCoroutine(PrepareAttack());
            if (collision.transform.position.x>transform.position.x)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && damageableObject != null && !isAttacking)
        {
            playerInAttackRange = true;
            isAttacking = true;
            StartCoroutine(PrepareAttack());
            if (collision.transform.position.x > transform.position.x)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInAttackRange = false;
        }
    }



    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            EnemyDeath();
        }
    }

    void EnemyDeath()
    {
        animator.SetBool("isDead", true);
        enemyCollider.enabled = false;
        attackRange.enabled = false;
    }
    public void DeathAnimationFinished()
    {
        Destroy();
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void Knockback(Vector3 damagingObjectPosition, Vector2 knockbackForce)
    {
        throw new System.NotImplementedException();
    }
}
