using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class PauseMenu : NetworkBehaviour
{
    public static bool GameIsPaused=false;
    public GameObject PauseM;
    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnPause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        OnPause();
    
            

     }
    void Pause()
    {
        PauseM.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        PauseM.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    } 

}
