using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]private GameObject JumpAttackHitbox;
    [SerializeField]private Transform carrotProjectileSpawnPoint;
    [SerializeField]private GameObject carrotProjectilePrefab;
    public int jumpAttackStrength = 15;
    public int jumpAttackDmg = 5;
   
    private Collider2D jumpAttackHitbox;
    private bool canInteract;
    private string interactionTriggerName;   
    private IInteractable interactableObject;
    private GameObject playerCharacter;
    private Rigidbody2D rb2d;
    private PlayerStats playerStats;
    

    private bool isPerformingJumpAttack=false;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        jumpAttackHitbox = JumpAttackHitbox.GetComponent<Collider2D>();
        jumpAttackHitbox.enabled = false;
    }

    

    private void OnAttackInteract()
    {
        Debug.Log("attack button press");

        if (canInteract==true)
        {
            interactableObject.Interact();
        }
        else
        {
            if (playerStats.AmmoAmount > 0)
            {
                ThrowCarrotAttack();
                playerStats.AmmoAmount--;
            }
        }
    }

    void ThrowCarrotAttack()
    {
        GameObject carrotProjectile = Instantiate(carrotProjectilePrefab, carrotProjectileSpawnPoint);
    }

    public void JumpAttack()
    {
        if (isPerformingJumpAttack == false)
        {
            jumpAttackHitbox.enabled = true;
            isPerformingJumpAttack = true;

            rb2d.velocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
            rb2d.AddForce(new Vector2(0, -1*jumpAttackStrength), ForceMode2D.Impulse);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Ground") || (collision.tag == "Bounce")) // check if player landed
        {
            isPerformingJumpAttack = false;
            jumpAttackHitbox.enabled = false;
            Debug.Log("Attack landed");
        }
        else if (collision.GetComponent<IPickupable>() != null) // check if object can be picked up
        {
            Debug.Log("pickup");
            collision.GetComponent<IPickupable>().Pickup(); // removes object from game
            playerStats.Pickup(collision.tag); // triggers pickup effect based on pickup tag
        }
        else if (collision.GetComponent<IDamageable>() != null) // check if object can be damaged (only during jump attack, when jump attack hitbox is enabled)
        {
            IDamageable damageableObejct= collision.GetComponent<IDamageable>();
            damageableObejct.Damage(jumpAttackDmg);
            rb2d.AddForce(new Vector2(0, 30), ForceMode2D.Impulse);
            isPerformingJumpAttack = false;
            jumpAttackHitbox.enabled = false;
        }
      
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null) // check if object can be interacted with
        {
            Debug.Log("Can interact");
            interactableObject = collision.GetComponent<IInteractable>();
            canInteract = true;
            interactableObject.AutoInteract();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bounce")
        {
            rb2d.AddForce(new Vector2(0, 30), ForceMode2D.Impulse);
        }
    }
}
