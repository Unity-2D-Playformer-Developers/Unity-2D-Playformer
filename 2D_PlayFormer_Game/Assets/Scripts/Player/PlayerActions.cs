using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class defining player actions such as interactions with obejcts and attacks.
/// </summary>
public class PlayerActions : MonoBehaviour
{
    /// <summary>
    /// area that can damage objects during jump attack
    /// </summary>
    [SerializeField]private GameObject JumpAttackHitbox;

    /// <summary>
    /// place where projectile is spawned
    /// </summary>
    [SerializeField]private Transform carrotProjectileSpawnPointRight;
    [SerializeField] private Transform carrotProjectileSpawnPointLeft;

    [SerializeField]private GameObject carrotProjectilePrefab;

    public int jumpAttackStrength = 15;
    public int jumpAttackDmg = 5;
    
    private bool canInteract;

    /// <summary>
    /// component references
    /// </summary>
    private CapsuleCollider2D jumpAttackHitbox;  
    private IInteractable interactableObject;
    private Rigidbody2D rb2d;
    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private Animator animator;
    private ParticleSystem jumpAttackParticle;

    private bool isPerformingJumpAttack=false;
    public bool IsPerformingJumpAttack { get => isPerformingJumpAttack; }
    private float rb2dGravcity;


    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        rb2d = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        jumpAttackHitbox = JumpAttackHitbox.GetComponentInChildren<CapsuleCollider2D>();
        jumpAttackHitbox.enabled = false;
        animator = GetComponent<Animator>();
        jumpAttackParticle = GetComponentInChildren<ParticleSystem>();
        rb2dGravcity = rb2d.gravityScale;
    }

    
    /// <summary>
    /// method managing action to perform when attack/interact button is pressed
    /// </summary>
    private void OnAttackInteract()
    {
        Debug.Log("attack button press");

        if(playerMovement.IsClimbing)
        {
            return;
        }

        if (canInteract==true)
        {
            interactableObject.Interact(transform);
        }
        else
        {
            if (playerStats.GetAmmoAmount > 0)
            {
                ThrowCarrotAttack();
                playerStats.AddAmmo(-1);
            }
        }
    }

    /// <summary>
    /// method for spawning carrot projectile from correnct points, depending on direction player is facing
    /// </summary>
    void ThrowCarrotAttack()
    {
        if (GameManager.Instance.GetIsPlayerFacingLeft == true) 
        {
            GameObject carrotProjectile = Instantiate(carrotProjectilePrefab, carrotProjectileSpawnPointLeft);
        }
        else
        {
            GameObject carrotProjectile = Instantiate(carrotProjectilePrefab, carrotProjectileSpawnPointRight);
        }
       
    }

    /// <summary>
    /// method for triggering and canceling jump attack
    /// </summary>
    /// <param name="performJumpAttack">bool for deciding if jump attack should be performed or cancelled</param>
    public void JumpAttack(bool performJumpAttack)
    {
        if (performJumpAttack && !isPerformingJumpAttack)
        {
            isPerformingJumpAttack = true;
            animator.SetBool("jumpAttack", isPerformingJumpAttack);

            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
            rb2d.gravityScale = 0;         
        }
        else if(!performJumpAttack)
        {
            isPerformingJumpAttack = false;
            jumpAttackHitbox.enabled = false;
            animator.SetBool("jumpAttack", isPerformingJumpAttack);
        }
    }

    /// <summary>
    /// method for performing jump attack. Gets called via animation event, at the end of "Spin" animation.
    /// </summary>
    public void JumpAttackPerform()
    {
        rb2d.gravityScale = rb2dGravcity;
        jumpAttackHitbox.enabled = true;
        rb2d.AddForce(new Vector2(0, -1*jumpAttackStrength), ForceMode2D.Impulse);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null) // check if object can be interacted with
        {
            Debug.Log("Can interact");
            interactableObject = collision.GetComponent<IInteractable>();
            canInteract = true;
            //interactableObject.AutoInteract();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageableObject = collision.collider.GetComponent<IDamageable>();
        if (damageableObject != null && isPerformingJumpAttack) // check if object can be damaged (only during jump attack, when jump attack hitbox is enabled)
        {
            damageableObject.TakeDamage(jumpAttackDmg);
            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
            rb2d.AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
            jumpAttackParticle.Play();
            JumpAttack(false);
        }
        else if(isPerformingJumpAttack)
        {
            jumpAttackParticle.Play();
        }
        if(collision.collider.tag=="Ground")
        {
            JumpAttack(false);
        }
    }
}
