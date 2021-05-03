using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class HeartDisplayManager : NetworkBehaviour
{
    
   
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI Ammunition;
    
    void Start()
        
    {
       
       
    }

    // Update is called once per frame

    public void UpdateAmunition(int carommunition)
    {
        Ammunition.text =""+carommunition;
    }
    public void UpdateScore(int score)
    {
        Score.text = "Score: " + score; 
    }
    public void UpdateHealth(int health, int NumberOfHearts)
    {
        for(int i = 0; i < hearts.Length;i++)
        {
            if (i < health)
            {
                hearts[i].sprite =fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
           }
            
            if (i<NumberOfHearts)
            
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
