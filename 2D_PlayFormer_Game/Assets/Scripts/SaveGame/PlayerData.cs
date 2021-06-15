using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    public int Level;

    public float x, y;

        public PlayerData(OpenScreen openScreen)
    {
        Level = openScreen.Scene();
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[1];
        x = player.transform.position.x;
        y = player.transform.position.y;
    }
    
}
