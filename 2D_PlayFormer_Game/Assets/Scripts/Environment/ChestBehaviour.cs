using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class defining openable chests.
/// </summary>

public class ChestBehaviour : MonoBehaviour, IInteractable
{
    public GameObject ChestContent;
    public bool ContentRandomAmount=true;
    public int ContentMaxAmount=5;
    public Transform ContentSpawnPosition;

    public SpriteRenderer SpriteRenderer;
    public Sprite OpenedChestSprite;
    

    private bool chestClosed=true;
    private PickupBehaviour pickupBehaviour;


    void Start()
    {
        pickupBehaviour = ChestContent.GetComponent<PickupBehaviour>();
    }


    /// <summary>
    /// Method called when player interacts with chest.
    /// </summary>
    /// <param name="playerPosition">Position on player. Determines direction in which spawned pickups will be thrown.</param>
    public void Interact(Transform playerPosition)
    {
        if (chestClosed)
        {
            ReplaceSprite();
            SpawnContent();
            Debug.Log("Chest opened!");
            chestClosed = false;
        }
    }

    public void AutoInteract()
    {
        //not used
    }

    /// <summary>
    /// Method used for spawning chest content. It can spawn random or defined amounts.
    /// </summary>
    void SpawnContent()
    {
        int spawnAmount;
        this.GetComponent<Collider2D>().enabled = false;

        if (ContentRandomAmount)
        {
            spawnAmount=Random.Range(1, ContentMaxAmount);
        }
        else
        {
            spawnAmount = ContentMaxAmount;
        }

        for (int i = spawnAmount; i > 0; i--)
        {
            GameObject pickup = Instantiate(ChestContent, ContentSpawnPosition);
            pickup.GetComponent<PickupBehaviour>().SpawnAndRandomThrow();
        }

    }
    
    /// <summary>
    /// Replaces sprite of closed chest with sprite of opened chest.,
    /// </summary>
   void ReplaceSprite()
    {
        SpriteRenderer.sprite = OpenedChestSprite;
    }

}
