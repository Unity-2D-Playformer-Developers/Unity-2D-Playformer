using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance { get => _instance; }

    [SerializeField]
    private GameObject playerCharacter;

    private Rigidbody2D playerRB;
    public Rigidbody2D PlayerRB { get => playerRB; }

    private PlayerStats playerStats;
    public PlayerStats PlayerStats { get => playerStats; }

    private void Awake()
    {
        if(_instance==null)
        {
            _instance = this;
        }
        else if(_instance!=this)
        {
            Destroy(gameObject);
        }


        playerStats = playerCharacter.GetComponent<PlayerStats>();
        playerRB = playerCharacter.GetComponent<Rigidbody2D>();
    }

    public void PickupCarrot()
    {
        playerStats.AddAmmo(1);
    }
    public void PickupCoin()
    {
        playerStats.AddCoins(1);
    }
    

    
}
