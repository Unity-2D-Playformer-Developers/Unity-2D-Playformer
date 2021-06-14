using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    public static UIManager Instance { get => _instance; }

    //Pause
    private bool GameIsPaused = false;
    public GameObject PauseM;

    //Health
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI Ammunition;
    public TextMeshProUGUI Coins;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region Pause Menu   
    private void OnPause()
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
    #endregion

    
    #region UI
    public void UpdateAmunition(int carrotAmmunition, int maxAmmo)
    {
        Ammunition.text = "" + carrotAmmunition + "/" + maxAmmo;
    }
    public void UpdateScore(int score)
    {
        Score.text = "Score: " + score;
    }
    public void UpdateCoins(int coins)
    {
        Coins.text = "" + coins;
    }
    public void UpdateHealth(int health, int NumberOfHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < NumberOfHearts)

            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
    #endregion
}
