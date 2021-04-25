using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public int PlayerMaxHealth;
    public int PlayerMaxAmmo;

    private Rigidbody2D rb2d;
    private Collider2D playerCollider;
    private SpriteRenderer playerSprite;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private Animator animator;

    private int health;
    private int ammoAmount;
    private int scoreAmount;
    private int coinsAmount;

    private bool canReceiveDamage=true;
    private bool receivedDamage=false;
    private bool isDead=false;

    public int GetHealth { get => health; }
    public int GetAmmoAmount { get => ammoAmount; }
    public int GetCoinsAmount { get => coinsAmount; }
    public int ScoreAmount { get => scoreAmount; }

    public void Knockback(Vector3 damagingObjectPosition, Vector2 knockbackForce)
    {
        if (!isDead)
        {
            if (damagingObjectPosition.x < rb2d.position.x)
            {
                rb2d.AddRelativeForce(knockbackForce, ForceMode2D.Impulse);
            }
            else
            {
                rb2d.AddRelativeForce(new Vector2(knockbackForce.x * -1, knockbackForce.y), ForceMode2D.Impulse);
            }
        }
    }
     
    void Start()
    {

        health = PlayerMaxHealth;
        ammoAmount = 0;


        coinsAmount = 0;
        UIManager.Instance.UpdateScore(coinsAmount);
        UIManager.Instance.UpdateAmunition(ammoAmount, PlayerMaxAmmo);
        UIManager.Instance.UpdateHealth(health, PlayerMaxHealth);

        playerSprite = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        playerCollider = GetComponent<Collider2D>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public bool AddAmmo(int amount)
    {
        bool canPickUp=false;
        if(ammoAmount+amount<=PlayerMaxAmmo)
        {
            ammoAmount = ammoAmount + amount;
            UIManager.Instance.UpdateAmunition(ammoAmount, PlayerMaxAmmo);
            canPickUp = true;
        }

        return canPickUp;
    }

    public bool AddHealth(int amount)
    {
        bool canPickUp = false;
        if (health + amount <= PlayerMaxHealth)
        {
            health += amount;
            UIManager.Instance.UpdateHealth(health, PlayerMaxHealth);
            canPickUp = true;
        }

        return canPickUp;
    }

    public bool AddCoins(int amount)
    {
        coinsAmount = coinsAmount + amount;
        UIManager.Instance.UpdateCoins(coinsAmount);
        return true;
    }
    public bool AddScore(int amount)
    {
        scoreAmount = scoreAmount + amount;
        UIManager.Instance.UpdateScore(scoreAmount);
        return true;
    }

    public void TakeDamage(int damageAmount)
    {
        if (canReceiveDamage)
        {
            health -=damageAmount;
            UIManager.Instance.UpdateHealth(health, PlayerMaxHealth);
            if (health <= 0)
            {
                isDead = true;
                PlayerDeath();
            }
            else
            {
                playerInput.DeactivateInput();
                playerMovement.EnableClimbingMode(false);
                animator.SetBool("receivedDamage", true);
                animator.SetTrigger("receiveDamage");     
                StartCoroutine(Iframe(2f));
                Debug.Log(health);
            }
        }
    }

    IEnumerator FlashEffect()
    {
        Color spriteColor = playerSprite.color;

        while (!canReceiveDamage)
        {
            spriteColor.a = 0.5f;
            playerSprite.color = spriteColor;
            yield return new WaitForSeconds(0.1f);

            spriteColor.a = 1f;
            playerSprite.color = spriteColor;
            yield return new WaitForSeconds(0.1f);
        }

        yield return 0;
    }

    public void ReceiveDamageAnimationStarted()
    {       
        StopCoroutine(FlashEffect());
    }
    public void ReceiveDamageAnimationFinished()
    {
        animator.SetBool("receivedDamage", false);
        playerInput.ActivateInput();
        StartCoroutine(FlashEffect());
    }

    IEnumerator Iframe(float duration)
    {
        canReceiveDamage = false;
        Physics2D.IgnoreLayerCollision(7, 13, true);
        yield return new WaitForSeconds(duration);
       
        canReceiveDamage = true;
        Physics2D.IgnoreLayerCollision(7, 13, false);
        yield return 0;
    }

    void PlayerDeath()
    {
        GameManager.Instance.DisableCameraFollow();
        playerInput.DeactivateInput();
        animator.SetBool("playerDied", true);
        playerCollider.enabled = false;
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
        Invoke("Destroy", 10f);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}


