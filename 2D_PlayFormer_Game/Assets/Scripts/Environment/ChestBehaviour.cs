using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChestBehaviour : NetworkBehaviour, IInteractable
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
        //
    }

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

   void ReplaceSprite()
    {
        SpriteRenderer.sprite = OpenedChestSprite;
    }

}
