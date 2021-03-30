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

    private void Start()
    {
        health = PlayerMaxHealth;

        ammoAmount = 0;

        coinsAmount = 0;
    }

    public void Pickup(string itemType)
    {
        switch(itemType)
        {
            case "PickupCarrot":
                if (ammoAmount < PlayerMaxAmmo)
                {
                    ammoAmount++;
                    Debug.Log("carrot amout: " + ammoAmount.ToString());
                }
                break;
            
            case "PickupHealth":
                if (health < PlayerMaxHealth)
                {
                    health++;
                }
                break;

            case "PickupCoin":
                coinsAmount++;
                Debug.Log("coins amout: " + coinsAmount.ToString());
                break;          
                
        }
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

    public int AmmoAmount
    {
        get
        {
            return ammoAmount;
        }
        set
        {
            ammoAmount = value;
        }
    }

}

