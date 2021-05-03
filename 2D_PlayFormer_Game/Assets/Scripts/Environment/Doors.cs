using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Doors : NetworkBehaviour, IInteractable
{
    public int price;

    [SerializeField]
    private GameObject doors;
    [SerializeField]
    private Sprite openedDoorsSprite;

    private Animator animator;

    private BoxCollider2D doorsCollider;
    private SpriteRenderer doorsSpriteRenderer;


    public void AutoInteract()
    {
        throw new System.NotImplementedException();
    }

    public void Interact(Transform playerPosition)
    {
        if(GameManager.Instance.PlayerStats.GetCoinsAmount>=price)
        {
            GameManager.Instance.SpendCoin(price);
            animator.SetBool("DoorOpen", true);
            doorsCollider.enabled = false;
            doorsSpriteRenderer.sprite = openedDoorsSprite;
        }
    }

    void Start()
    {
        doorsCollider = doors.GetComponent<BoxCollider2D>();
        doorsSpriteRenderer= doors.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    
}
