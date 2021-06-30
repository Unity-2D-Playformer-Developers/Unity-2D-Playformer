using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour, IInteractable
{
    public int price;

    [SerializeField]
    private GameObject doors;
    [SerializeField]
    private Sprite openedDoorsSprite;

    public Sprite OpenedDoorsSprite
    {
        get
        {
            return openedDoorsSprite;
        }
    }

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
            Open();
        }
    }

    public void Open()
    {
        animator.SetBool("DoorOpen", true);
        doorsCollider.enabled = false;
        doorsSpriteRenderer.sprite = openedDoorsSprite;
    }

    public bool IsOpened()
    {
        return animator.GetBool("DoorOpen");
    }

    public void Start()
    {
        doorsCollider = doors.GetComponent<BoxCollider2D>();
        doorsSpriteRenderer= doors.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    
}
