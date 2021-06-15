using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// Class managing enemy behaviour
/// </summary>
public class EnemyBehaviour : MonoBehaviour, IDamageable
{

    public float IdleVisionRange; /// <summary>
    /// range at which enemy detects player
    /// </summary>
    public float ChasingVisionRange;/// <summary>
    /// range at which enemy sees player. if playher leaves this trigger, enemy goes back to patrol point.
    /// </summary>
    public float MovementSpeed;
    public int AttackDamage;
    public float MaxHealth;
    public bool ChasesPlayer;/// <summary>
    /// bool defining if enemy chases player
    /// </summary>

    private float health;
    private bool isChasingPlayer;
  
        /// <summary>
        /// component references
        /// </summary>
    private AIPath aiPath;
    private Patrol patrol;
    private AIDestinationSetter destinationSetter;
    private CircleCollider2D visionRange;
    private BoxCollider2D enemyCollider;
    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    /// <summary>
    /// method used to deal damage to enemy
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if(health<=0)
        {
            EnemyDeath();
        }
    }

    /// <summary>
    /// method that gets called after enemy HP is >=0.
    /// </summary>
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

    /// <summary>
    /// method that gets called via animation event at the end of Death animation. Destroys enemy gameobject.
    /// </summary>
    public void DeathAnimationFinished()
    {
        Destroy();
    }


    public void Destroy()
    {
        Destroy(this.gameObject);
    }


    /// <summary>
    /// method for rotating sprite, depending on which X direction enemy moves at. 
    /// </summary>
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

    /// <summary>
    /// not used
    /// </summary>
    /// <param name="damagingObjectPosition"></param>
    /// <param name="knockbackForce"></param>
    public void Knockback(Vector3 damagingObjectPosition, Vector2 knockbackForce)
    {
        throw new System.NotImplementedException();
    }
}
