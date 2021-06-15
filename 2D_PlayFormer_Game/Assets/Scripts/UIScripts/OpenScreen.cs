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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        SceneManager.LoadScene(level);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Transform kek = GameObject.FindGameObjectsWithTag("Player")[1].transform;
        kek.position = new Vector2(playerData.x, playerData.y);

        SavedEnemy[] savedEnemies = new SavedEnemy[playerData.enemiesNames.Length];

        int i;
        int j = 0;

        for (i = 0; i < playerData.enemiesNames.Length; ++i)
        {
            savedEnemies[i] = new SavedEnemy();
            savedEnemies[i].enemyName = playerData.enemiesNames[i];
            savedEnemies[i].enemyX = playerData.enemiesX[i];
            savedEnemies[i].enemyY = playerData.enemiesY[i];
            savedEnemies[i].enemyZ = playerData.enemiesZ[i];
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Array.Sort(enemies, compareObjects);
        Array.Sort(savedEnemies);

        for(i = 0; i < playerData.enemiesNames.Length; ++i, ++j)
        {
            if(enemies[j].name != savedEnemies[i].enemyName)
            {
                Destroy(enemies[j].gameObject);
                --i;
                continue;
            }
            enemies[j].transform.position = new Vector3(savedEnemies[i].enemyX, savedEnemies[i].enemyY, savedEnemies[i].enemyZ);
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
}
