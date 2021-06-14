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
    public void LoadGame()
    {
        PlayerData data = SaveLoadSystem.LoadPlayer();
        level = data.Level;
        SceneManager.LoadScene(level);
    }
}
