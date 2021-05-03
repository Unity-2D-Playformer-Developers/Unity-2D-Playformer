using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class PlayerActions : NetworkBehaviour
{
    [SerializeField]private GameObject JumpAttackHitbox;
    [SerializeField]private Transform carrotProjectileSpawnPointRight;
    [SerializeField] private Transform carrotProjectileSpawnPointLeft;
    [SerializeField]private GameObject carrotProjectilePrefab;
    public int jumpAttackStrength = 15;
    public int jumpAttackDmg = 5;
   
    private CapsuleCollider2D jumpAttackHitbox;
    private bool canInteract;
    private string interactionTriggerName;   
    private IInteractable interactableObject;
    private GameObject playerCharacter;
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

    public override void OnStartLocalPlayer()
    {
        var camera = Camera.main;
        camera.GetComponent<CameraFollow>().target = transform;
    }

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
    public void JumpAttackPerform()
    {
        rb2d.gravityScale = rb2dGravcity;
        jumpAttackHitbox.enabled = true;
        rb2d.AddForce(new Vector2(0, -1*jumpAttackStrength), ForceMode2D.Impulse);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
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
    }
}
