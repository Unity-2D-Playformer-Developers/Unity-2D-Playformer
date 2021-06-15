using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScreen : MonoBehaviour
{
    private int level;
    
    
    // Start is called before the first frame update
    
    public void StartGame()
    {
      
           SceneManager.LoadScene(1);
        level= SceneManager.GetActiveScene().buildIndex;

    }

    public void ExitGame()
    {
        Debug.Log("Quit is working");
        Application.Quit();
    }
    public int Scene()
    {
       return level = SceneManager.GetActiveScene().buildIndex;

    }

    public void SaveGame()
    {
        SaveLoadSystem.SavePlayer(this);
    }

    PlayerData playerData;

    public void LoadGame()
    {
        
        
        PlayerData data = SaveLoadSystem.LoadPlayer();
        playerData = data;
        level = data.Level;
        GameManager.Instance.PlayerStats.LoadStats(data.health, data.ammo, data.score, data.coinsammoun);
        SceneManager.LoadScene(level);
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Transform kek = GameObject.FindGameObjectsWithTag("Player")[1].transform;
        kek.position = new Vector2(playerData.x, playerData.y);

        SavedEnemy[] savedEnemies = new SavedEnemy[playerData.enemiesNames.Length];
        SavedCoins[] savedCoins= new SavedCoins[playerData.coinNames.Length];

        int i;
        int j = 0;
        int jj = 0;

        for (i = 0; i < playerData.enemiesNames.Length; ++i)
        {
            savedEnemies[i] = new SavedEnemy();
            savedEnemies[i].enemyName = playerData.enemiesNames[i];
            savedEnemies[i].enemyX = playerData.enemiesX[i];
            savedEnemies[i].enemyY = playerData.enemiesY[i];
            savedEnemies[i].enemyZ = playerData.enemiesZ[i];
        }
        for (i = 0; i < playerData.coinNames.Length; ++i)
        {
            savedCoins[i] = new SavedCoins();
            savedCoins[i].coinsName = playerData.coinNames[i];
            savedCoins[i].coinX = playerData.coinX[i];
            savedCoins[i].coinY = playerData.coinY[i];
            savedCoins[i].coinZ = playerData.coinZ[i];
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Array.Sort(enemies, compareObjects);
        Array.Sort(savedEnemies);

        GameObject[] coins = GameObject.FindGameObjectsWithTag("PickupCoin");
        Array.Sort(coins, compareObjects);
        Array.Sort(savedCoins);

        for (i = 0; i < playerData.enemiesNames.Length; ++i, ++j)
        {
            if(enemies[j].name != savedEnemies[i].enemyName)
            {
                Destroy(enemies[j].gameObject);
                --i;
                continue;
            }
            enemies[j].transform.position = new Vector3(savedEnemies[i].enemyX, savedEnemies[i].enemyY, savedEnemies[i].enemyZ);
        }

        for (i = 0; i < playerData.coinNames.Length; ++i, ++jj)
        {
            if (coins[jj].name != savedCoins[i].coinsName)
            {
                Destroy(coins[jj].gameObject);
                --i;
                continue;
            }
            coins[jj].transform.position = new Vector3(savedCoins[i].coinX, savedCoins[i].coinY, savedCoins[i].coinZ);
        }

    }

    public int compareObjects(GameObject a, GameObject b)
    {
        return a.name.CompareTo(b.name);
    }

    class SavedEnemy : IComparable
    {
        public string enemyName;
        public float enemyX;
        public float enemyY;
        public float enemyZ;

        public int CompareTo(object obj)
        {
            SavedEnemy o = (SavedEnemy)obj;
            return enemyName.CompareTo(o.enemyName);
        }
    }

    class SavedCoins : IComparable
    {
        public string coinsName;
        public float coinX;
        public float coinY;
        public float coinZ;

        public int CompareTo(object obj)
        {
            SavedCoins o = (SavedCoins)obj;
            return coinsName.CompareTo(o.coinsName);
        }
    }

}
