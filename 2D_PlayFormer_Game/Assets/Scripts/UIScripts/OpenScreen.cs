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
        // SceneManager.sceneLoaded += StartNewGame;
        level = SceneManager.GetActiveScene().buildIndex;
       

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
    //void StartNewGame(Scene scene, LoadSceneMode mode)
    //{
        // GameManager.Instance.LevelRestart();
        //GameManager.Instance.PlayerStats.LoadStats(5, 0, 0, 0);

        // Transform kek = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        // var shmek = GameObject.FindGameObjectsWithTag("Player")[1].transform.position;
        // kek.position = new Vector3(-21.88f - shmek.x, 1.92f - shmek.y, -13.08451f - shmek.z);
        //kek.position = new Vector3(0, 0, 0);
    //}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerData data = SaveLoadSystem.LoadPlayer();
        Debug.Log("OnSceneLoaded: " + scene.name);
        Transform kek = GameObject.FindGameObjectsWithTag("Player")[1].transform;
        if(kek.GetChild(0).tag == "Player")
        {
            kek = kek.GetChild(0).transform;
        }
        kek.position = new Vector2(playerData.x, playerData.y);
        GameManager.Instance.PlayerStats.LoadStats(data.health, data.ammo, data.score, data.coinsammoun);

        LoadSavedDataIntoGameObjects(playerData.enemiesNames, playerData.enemiesX, 
                                                              playerData.enemiesY, 
                                                              playerData.enemiesZ, 
                                                              GameObject.FindGameObjectsWithTag("Enemy"));

        LoadSavedDataIntoGameObjects(playerData.coinNames, playerData.coinX,
                                                              playerData.coinY,
                                                              playerData.coinZ,
                                                              GameObject.FindGameObjectsWithTag("PickupCoin"));

        LoadSavedDataIntoGameObjects(playerData.destroyableNames, playerData.destroyableX,
                                                              playerData.destroyableY,
                                                              playerData.destroyableZ,
                                                              GameObject.FindGameObjectsWithTag("DestroyableGround"));

        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");

        for (int i = 0; i < chests.Length; ++i)
        {
            if (playerData.chestOpened[i])
            {
                chests[i].GetComponent<ChestBehaviour>().chestClosed = false;
                chests[i].GetComponent<ChestBehaviour>().ReplaceSprite();
            }
        }

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        GameObject[] doorsContol = GameObject.FindGameObjectsWithTag("DoorsControl");

        for (int i = 0; i < doors.Length; i++)
        {
            if(playerData.doorOpened[i])
            {
                doorsContol[i].GetComponent<Doors>().Start();
                doorsContol[i].GetComponent<Doors>().Open();
                // doors[i].GetComponent<SpriteRenderer>().sprite = openedDoorsSprite;
            }
        }

        //LoadSavedDataIntoGameObjects(playerData.chestsNames, playerData.chestsX,
        //playerData.chestsY,
        //playerData.chestsZ,
        //GameObject.FindGameObjectsWithTag("Chest"));
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public Sprite openedDoorsSprite;

    void LoadSavedDataIntoGameObjects(string[] names, float[] x, float[] y, float[] z, GameObject[] gameObjects)
    {
        NameAndXYZ[] saved = new NameAndXYZ[names.Length];

        int i;

        for (i = 0; i < names.Length; ++i)
        {
            saved[i] = new NameAndXYZ();
            saved[i].name = names[i];
            saved[i].x = x[i];
            saved[i].y = y[i];
            saved[i].z = z[i];
        }

        Array.Sort(gameObjects, compareObjects);
        Array.Sort(saved);

        int j = 0;
        for (i = 0; i < names.Length; ++i, ++j)
        {
            if (gameObjects[j].name != saved[i].name)
            {
                Destroy(gameObjects[j].gameObject);
                --i;
                continue;
            }
            gameObjects[j].transform.position = new Vector3(saved[i].x, saved[i].y, saved[i].z);
        }
    }

    public int compareObjects(GameObject a, GameObject b)
    {
        return a.name.CompareTo(b.name);
    }

    class NameAndXYZ : IComparable
    {
        public string name;
        public float x;
        public float y;
        public float z;

        public int CompareTo(object obj)
        {
            NameAndXYZ o = (NameAndXYZ)obj;
            return name.CompareTo(o.name);
        }
    }

}
