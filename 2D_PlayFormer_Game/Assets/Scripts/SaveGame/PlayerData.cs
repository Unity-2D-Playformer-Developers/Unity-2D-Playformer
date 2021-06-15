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

    public PlayerData(OpenScreen openScreen)
    {
        Level = openScreen.Scene();
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[1];
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesX = new float[enemies.Length];
        enemiesY = new float[enemies.Length];
        enemiesZ = new float[enemies.Length];
        enemiesNames = new string[enemies.Length];
        for(int i = 0; i < enemies.Length; ++i)
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
