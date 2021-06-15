using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class definining behaviout of doors machine and droos that are assigned to it.
/// </summary>

public class Doors : MonoBehaviour, IInteractable
{
    public int price;
    ///
    [SerializeField]
    private GameObject doors; /// <summary>
    /// door gameobject placed on the map
    /// </summary>
    [SerializeField]
    private Sprite openedDoorsSprite;

    private Animator animator;

    private BoxCollider2D doorsCollider;
    private SpriteRenderer doorsSpriteRenderer;


    public void AutoInteract()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Method called when player interacts with machine. Disabled collider and replaces sprite of assigned doors.
    /// </summary>
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
