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

    public string[] destroyableNames;
    public float[] destroyableX;
    public float[] destroyableY;
    public float[] destroyableZ;
    
    public string[] chestsNames;
    public bool[] chestOpened;

    public string[] doorNames;
    public bool[] doorOpened;

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

        GameObject player = GameObject.FindGameObjectsWithTag("Player")[1];
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] coins = GameObject.FindGameObjectsWithTag("PickupCoin");
        GameObject[] destroyable = GameObject.FindGameObjectsWithTag("DestroyableGround");
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        GameObject[] doorsControls = GameObject.FindGameObjectsWithTag("DoorsControl");

        saveObjectsNamesAndPositions(enemies, ref enemiesNames, ref enemiesX, ref enemiesY, ref enemiesZ);
        saveObjectsNamesAndPositionsWithoutPhysics(coins, ref coinNames, ref coinX, ref coinY, ref coinZ);
        saveObjectsNamesAndPositions(destroyable, ref destroyableNames, ref destroyableX, ref destroyableY, ref destroyableZ);
        //saveObjectsNamesAndPositions(chests, ref chestsNames, ref chestsX, ref chestsY, ref chestsZ);

        saveOpenedChests(chests, ref chestsNames, ref chestOpened);
        saveOpenedDoors(doors, ref doorNames, ref doorOpened, doorsControls);
        
        x = player.transform.position.x;
        y = player.transform.position.y;
    }

    void saveOpenedChests(GameObject[] gameObjects, ref string[] names, ref bool[] opened)
    {
        names = new string[gameObjects.Length];
        opened = new bool[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; ++i)
        {
            names[i] = gameObjects[i].name;
            opened[i] = !gameObjects[i].GetComponent<ChestBehaviour>().chestClosed;
        }
    }

    void saveOpenedDoors(GameObject[] gameObjects, ref string[] names, ref bool[] opened, GameObject[] controls)
    {
        names = new string[gameObjects.Length];
        opened = new bool[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; ++i)
        {
            names[i] = gameObjects[i].name;
            // TODO: opened doors
            opened[i] = gameObjects[i].GetComponent<SpriteRenderer>().sprite == controls[i].GetComponent<Doors>().OpenedDoorsSprite;
        }
    }

    void saveObjectsNamesAndPositionsWithoutPhysics(GameObject[] gameObjects, ref string[] names, ref float[] x, ref float[] y, ref float[] z)
    {
        List<GameObject> objectsWithoutPhysics = new List<GameObject>();
        foreach (var obj in gameObjects)
        {
            if(obj.GetComponent<Rigidbody2D>() == null)
            {
                objectsWithoutPhysics.Add(obj);
            }
        }
        saveObjectsNamesAndPositions(objectsWithoutPhysics.ToArray(), ref names, ref x, ref y, ref z);
    }

    void saveObjectsNamesAndPositions(GameObject[] gameObjects, ref string[] names, ref float[] x, ref float[] y, ref float[] z)
    {
        names = new string[gameObjects.Length];
        x = new float[gameObjects.Length];
        y = new float[gameObjects.Length];
        z = new float[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; ++i)
        {
            names[i] = gameObjects[i].name;
            x[i] = gameObjects[i].transform.position.x;
            y[i] = gameObjects[i].transform.position.y;
            z[i] = gameObjects[i].transform.position.z;
        }
    }
}
