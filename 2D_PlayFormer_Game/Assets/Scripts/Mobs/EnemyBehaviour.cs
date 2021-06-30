using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviour : MonoBehaviour, IDamageable
{

    public float IdleVisionRange;
    public float ChasingVisionRange;
    public float MovementSpeed;
    public int AttackDamage;
    public float MaxHealth;
    public bool ChasesPlayer;

    private float health;
    private bool isChasingPlayer;
    private AIPath aiPath;
    private Patrol patrol;
    private AIDestinationSetter destinationSetter;
    private CircleCollider2D visionRange;
    private BoxCollider2D enemyCollider;
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if(health<=0)
        {
            EnemyDeath();
        }
    }

    void EnemyDeath()
    {
        patrol.enabled = false;
        destinationSetter.enabled = false;
        enemyCollider.enabled = false;
        if(rb!=null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
        aiPath.canMove = false;

        animator.SetBool("isDead", true);
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

    void Rotate()
    {
        if(aiPath.desiredVelocity.x>0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        patrol = GetComponent<Patrol>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        visionRange = GetComponent<CircleCollider2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        visionRange.radius = IdleVisionRange;
        health = MaxHealth;
        aiPath.maxSpeed = MovementSpeed;
    }

    void Update()
    {
        Rotate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && ChasesPlayer)
        {
            patrol.enabled = false;
            destinationSetter.enabled = true;
            isChasingPlayer = true;
            visionRange.radius = ChasingVisionRange;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        destinationSetter.enabled = false;
        patrol.enabled = true;
        isChasingPlayer = false;
        visionRange.radius = IdleVisionRange;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            IDamageable player = collision.collider.GetComponent<IDamageable>();
            player.TakeDamage(AttackDamage);
            player.Knockback(transform.position, new Vector2(5f, 10f));
        }
    }

}
