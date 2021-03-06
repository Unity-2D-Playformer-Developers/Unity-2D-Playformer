using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform itemSpawnPoint;
    [SerializeField]
    private GameObject carrot;

    private Animator animator;

    private bool isOnCooldown;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AutoInteract()
    {
        throw new System.NotImplementedException();
    }

    public void Interact(Transform playerPosition)
    {

        if (!isOnCooldown)
        {
            if(GameManager.Instance.PlayerStats.GetCoinsAmount>=10)
            {
                GameObject pickup;

                GameManager.Instance.SpendCoin(10);
                pickup = Instantiate(carrot, itemSpawnPoint);
                pickup.GetComponent<PickupBehaviour>().SpawnAndThrow(playerPosition, new Vector2(200f, 300f));
                pickup.transform.parent = null;
                pickup.transform.position = new Vector3(itemSpawnPoint.position.x, itemSpawnPoint.position.y, transform.position.z - 1);
                isOnCooldown = true;
                animator.SetTrigger("BuyCarrot");
            }
        }
    }

    public void FinishBuyingAnimation()
    {
        isOnCooldown = false;
    }
}