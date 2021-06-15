using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class that stores player data - health, ammo, coins amount. Has methods for dealing damage to player, managing player iframes.
/// </summary>

public class PlayerStats : MonoBehaviour, IDamageable
{
    public int PlayerMaxHealth;
    public int PlayerMaxAmmo;

    /// <summary>
    /// components references
    /// </summary>
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

    /// <summary>
    /// method used to knockback player when he receives damage
    /// </summary>
    /// <param name="damagingObjectPosition">position of object that damaged player</param>
    /// <param name="knockbackForce">force that gets added to player rigidbody</param>
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

    /// <summary>
    /// method used to add ammo, usually called from GameManager when player enters pickup trigger.
    /// </summary>
    /// <param name="amount">amount of ammo to add</param>
    /// <returns></returns>
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

    /// <summary>
    /// method used to add health, usually called from GameManager when player enters pickup trigger.
    /// </summary>
    /// <param name="amount">amount of helath to add</param>
    /// <returns></returns>
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

    /// <summary>
    /// method used to add/spend coins, usually called from GameManager when player enters pickup trigger, or uses vending machines to spend coins.
    /// </summary>
    /// <param name="amount">amount of coins to add</param>
    /// <returns></returns>
    public bool AddCoins(int amount)
    {
        coinsAmount = coinsAmount + amount;
        UIManager.Instance.UpdateCoins(coinsAmount);
        return true;
    }
    /// <summary>
    /// method used to add score, player get score point for every coin picked up.
    /// </summary>
    /// <param name="amount">amount of score to add</param>
    /// <returns></returns>
    public bool AddScore(int amount)
    {
        scoreAmount = scoreAmount + amount;
        UIManager.Instance.UpdateScore(scoreAmount);
        return true;
    }

    /// <summary>
    /// method used to decrease player health
    /// </summary>
    /// <param name="damageAmount">amount of health that gets subtracted </param>
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

    /// <summary>
    /// Flashes player sprite after player received damage. Lasts as long, as player iframe.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Method that stops sprite flashing effect.Gets called via animation event, at the start of "Receive Damage" animation.
    /// </summary>
    public void ReceiveDamageAnimationStarted()
    {       
        StopCoroutine(FlashEffect());
    }

    /// <summary>
    /// Method that starts sprite flashing effect. Gets called via animation event, at the end of "Receive Damage" animation.
    /// </summary>
    public void ReceiveDamageAnimationFinished()
    {
        animator.SetBool("receivedDamage", false);
        playerInput.ActivateInput();
        StartCoroutine(FlashEffect());
    }

    /// <summary>
    /// Method that disables player collision with damaging objects(enemies, enviromental hazards) for defined time - player cant be damaged during iframe.
    /// </summary>
    IEnumerator Iframe(float duration)
    {
        canReceiveDamage = false;
        Physics2D.IgnoreLayerCollision(7, 13, true);
        yield return new WaitForSeconds(duration);
       
        canReceiveDamage = true;
        Physics2D.IgnoreLayerCollision(7, 13, false);
        yield return 0;
    }

    /// <summary>
    /// Method that plays player death animation, disables player input and invokes Destroy() method.
    /// </summary>
    void PlayerDeath()
    {
        GameManager.Instance.DisableCameraFollow();
        playerInput.DeactivateInput();
        animator.SetBool("playerDied", true);
        playerCollider.enabled = false;
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
        Invoke("Destroy", 2f);        
    }

    /// <summary>
    /// Method that destroys player gameobject and restarts level.
    /// </summary>
    public void Destroy()
    {
        Destroy(this.gameObject);
        GameManager.Instance.LevelRestart();
    }
}


