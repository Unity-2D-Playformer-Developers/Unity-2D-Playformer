using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    public int Level;

    public float x, y;

    public string[] enemiesNames;
    public float[] enemiesX;
    public float[] enemiesY;
    public float[] enemiesZ;
    public string[] coinNames;
    public float[] coinX;
    public float[] coinY;
    public float[] coinZ;
    public int ammo { get => Ammo; }
    private int Ammo;
    public int coinsammoun { get => Coinsammoun; }
    private int Coinsammoun;
    public int score { get => Score; }
    private int Score;
    public int health { get => Health; }
    private int Health;
    public PlayerData(OpenScreen openScreen)
    {
        Ammo = GameManager.Instance.PlayerStats.GetAmmoAmount;
        Coinsammoun = GameManager.Instance.PlayerStats.GetCoinsAmount;
        Health = GameManager.Instance.PlayerStats.GetHealth;
        Score = GameManager.Instance.PlayerStats.ScoreAmount;
        Level = openScreen.Scene();
       GameObject[] coins = GameObject.FindGameObjectsWithTag("PickupCoin");
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[1];
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        coinX = new float[coins.Length];
        coinY = new float[coins.Length];
        coinZ = new float[coins.Length];
        coinNames = new string[coins.Length];
        enemiesX = new float[enemies.Length];
        enemiesY = new float[enemies.Length];
        enemiesZ = new float[enemies.Length];
        enemiesNames = new string[enemies.Length];
        for (int i = 0; i < coins.Length; ++i)
        {
            coinNames[i] = coins[i].name;
            coinX[i] = coins[i].transform.position.x;
            coinY[i] = coins[i].transform.position.y;
            coinZ[i] = coins[i].transform.position.z;
            
        }
        for (int i = 0; i < enemies.Length; ++i)
        {
            enemiesNames[i] = enemies[i].name;
            enemiesX[i] = enemies[i].transform.position.x;
            enemiesY[i] = enemies[i].transform.position.y;
            enemiesZ[i] = enemies[i].transform.position.z;
        }
        x = player.transform.position.x;
        y = player.transform.position.y;
    }
    
}
