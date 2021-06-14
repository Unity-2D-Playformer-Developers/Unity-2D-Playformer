using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public bool FinalLevel;
    private Collider2D nextLevelTrigger;

    void Start()
    {
        nextLevelTrigger = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!FinalLevel)
        {
            if (collision.tag == "Player")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


            }
        }
        else
        {
            Debug.LogWarning("This is final level!"); // dorobic jakis end screen
        }
    }
}
