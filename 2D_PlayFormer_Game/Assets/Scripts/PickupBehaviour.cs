using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour, IPickupable
{
    public bool simulatePhysics;
    public LayerMask groundLayer;

    private Rigidbody2D rb2d;

    private void OnEnable()
    {
        if (simulatePhysics == true)
        {
            rb2d = this.GetComponentInChildren<Rigidbody2D>();  
            Vector2 throwStrenght = new Vector2(Random.Range(-400f, 400f), Random.Range(50f, 400f));
            rb2d.AddForce(throwStrenght);
        }
        else
        {
            CircleCollider2D pickupTrigger = GetComponent<CircleCollider2D>();
            pickupTrigger.enabled = true;
        }
    }

    public void Pickup()
    {
         Debug.Log("pickup destroyed");
         GameObject pickup = this.gameObject;
         GameObject.Destroy(pickup);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
         CircleCollider2D pickupTrigger = GetComponent<CircleCollider2D>();
         pickupTrigger.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Pickup();
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Pickup();
        }
    }
}
