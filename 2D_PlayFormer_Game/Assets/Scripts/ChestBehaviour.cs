using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour, IInteractable
{
    public SpriteRenderer SpriteRenderer;
    public Sprite OpenedChestSprite;
    public GameObject ChestContent;
    public Transform ContentSpawnPosition;

    private bool chestClosed=true;

    public void Interact()
    {
        if (chestClosed)
        {
            ReplaceSprite();
            SpawnContent();
            Debug.Log("Chest opened!");
            chestClosed = false;
            this.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void AutoInteract()
    {
        //
    }

    void SpawnContent()
    {
        GameObject pickup = Instantiate(ChestContent, ContentSpawnPosition);
    }

   void ReplaceSprite()
    {
        SpriteRenderer.sprite = OpenedChestSprite;
    }

}
