using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance { get => _instance; }

    [SerializeField]
    private GameObject playerCharacter;

    [SerializeField]
    private Camera mainCamera;

    private Rigidbody2D playerRB;
    public Rigidbody2D PlayerRB { get => playerRB; }


    private PlayerStats playerStats;
    public PlayerStats PlayerStats { get => playerStats; }

    private SpriteRenderer playerSprite;


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


        playerStats = playerCharacter.GetComponent<PlayerStats>();
        playerRB = playerCharacter.GetComponent<Rigidbody2D>();
        playerSprite = playerCharacter.GetComponent<SpriteRenderer>();
    }

    public void PickupCarrot(GameObject pickup)
    {
        bool pickupSuccessfull = playerStats.AddAmmo(1);
        if (pickupSuccessfull)
        {
            Destroy(pickup);
        }
    }
    public void PickupCoin(GameObject pickup)
    {
        bool pickupSuccessfull = playerStats.AddCoins(1);
        if (pickupSuccessfull)
        {
            playerStats.AddScore(1);
            Destroy(pickup);
        }
    }
    public void SpendCoin(int amount)
    {
        if (playerStats.GetCoinsAmount >= amount)
        {
            playerStats.AddCoins(-amount);
            UIManager.Instance.UpdateCoins(playerStats.GetCoinsAmount);
        }
    }

    public void PickupHealth(GameObject pickup)
    {
        bool pickupSuccessfull = playerStats.AddHealth(1);
        if (pickupSuccessfull)
        {
            Destroy(pickup);
        }
    }

    public void LevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void DisableCameraFollow()
    {
        mainCamera.GetComponent<CameraFollow>().enabled = false;
    }
    public void EnableCameraFollow()
    {
        mainCamera.GetComponent<CameraFollow>().enabled = true;
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public bool GetIsPlayerFacingLeft
    {
        get { return playerSprite.flipX; }
    }
}
