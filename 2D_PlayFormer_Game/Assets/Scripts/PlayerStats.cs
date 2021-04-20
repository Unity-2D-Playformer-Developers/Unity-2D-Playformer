using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public int PlayerMaxHealth;
    public int PlayerMaxAmmo;
    public GameObject AA;
    private Rigidbody2D rb2d;
    private int health;
    private int ammoAmount;
    private int coinsAmount;
    [SerializeField]private HeartDisplayManager HeartDisplayManager;
    public int GetHealth { get => health; }
    public int GetAmmoAmount { get => ammoAmount; }
    public int GetCoinsAmount { get => coinsAmount; }

    public void Knockback(float knockDur, float knockbackPwr, Vector3 knockbackDir)
    {
        
        float timer = 0;
            if(knockDur>timer)
        {
            timer += Time.deltaTime;
            Debug.Log("jest");
            rb2d.AddForce(new Vector3(knockbackDir.x * -100, knockbackDir.y * knockbackPwr, transform.position.z));
        }
       
    }
    private void Start()
    {

        health = PlayerMaxHealth;
        ammoAmount = 0; 


        coinsAmount = 0;
        HeartDisplayManager.UpdateScore(coinsAmount);
        HeartDisplayManager.UpdateAmunition(ammoAmount);
        HeartDisplayManager.UpdateHealth(health, PlayerMaxHealth);
    }

    public void AddAmmo(int amount)
    {
        ammoAmount = ammoAmount + amount;
        HeartDisplayManager.UpdateAmunition(ammoAmount);
        if (ammoAmount>PlayerMaxAmmo)
        {
            ammoAmount = PlayerMaxAmmo;
        }
    }

    public void AddCoins(int amount)
    {
        coinsAmount = coinsAmount + amount;
        HeartDisplayManager.UpdateScore(coinsAmount);
    }

    public void Damage(int damageAmount)
    {
        this.health = health - damageAmount;
        HeartDisplayManager.UpdateHealth(health, PlayerMaxHealth);
        Debug.Log(health);
        if (health <= 0)
        {
            Destroy();
            
        }
    }

    public void Destroy()
    {
        Destroy(AA);
    }


}

