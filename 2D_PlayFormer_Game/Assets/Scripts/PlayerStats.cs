using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public int PlayerMaxHealth;
    public int PlayerMaxAmmo;

    private int health;
    private int ammoAmount;
    private int coinsAmount;

    public int GetHealth { get => health; }
    public int GetAmmoAmount { get => ammoAmount; }
    public int GetCoinsAmount { get => coinsAmount; }

    private void Start()
    {
        health = PlayerMaxHealth;

        ammoAmount = 0;

        coinsAmount = 0;
    }

    public void AddAmmo(int amount)
    {
        ammoAmount = ammoAmount + amount;
        if(ammoAmount>PlayerMaxAmmo)
        {
            ammoAmount = PlayerMaxAmmo;
        }
    }

    public void AddCoins(int amount)
    {
        coinsAmount = coinsAmount + amount;
    }

    public void Damage(int damageAmount)
    {
        this.health = health - damageAmount;
        if(health<=0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(this);
    }


}

