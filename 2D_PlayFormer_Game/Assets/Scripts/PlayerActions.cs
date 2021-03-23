using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{

    private bool canInteract;
    private string interactionTriggerName;

    private IInteractable interactableObject;


    private void OnAttackInteract()
    {
        Debug.Log("attack button press");

        if (canInteract==true)
        {
            interactableObject.Interact();
        }
        else
        {
            Attack();
        }
    }

    private void Attack()
    {
        //to do
    }

    private void PickupEffect()
    {
        //to do
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<IPickupable>() != null) // check if object can be picked up
        {
            Debug.Log("pickup");
            IPickupable pickupableObject = collision.GetComponent<IPickupable>();
            collision.GetComponent<IPickupable>().Pickup();
            PickupEffect();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null) // check if object can be interacted with
        {
            Debug.Log("Can interact");
            interactableObject = collision.GetComponent<IInteractable>();
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canInteract = false;
    }
}
