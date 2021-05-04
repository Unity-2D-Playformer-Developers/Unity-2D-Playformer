using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PickupBehaviour : NetworkBehaviour
{
    public bool simulatePhysics;
    public LayerMask groundLayer;

    [SerializeField]
    private Rigidbody2D rb2d;
    private string pickupType;

    private void OnEnable()
    {
        pickupType = gameObject.tag;
        if (!simulatePhysics)
        {
            Spawn();
        }

    }

    void Spawn()
    {
        CircleCollider2D pickupTrigger = GetComponent<CircleCollider2D>();
        pickupTrigger.enabled = true;
    }

    public void SpawnAndThrow(Transform playerPosition, Vector2 throwStrenght)
    {
        if(playerPosition.position.x > transform.position.x)
        {
            throwStrenght.x = throwStrenght.x * -1;
        }
        rb2d.AddForce(throwStrenght);
    }
    public void SpawnAndRandomThrow()
    {
        Vector2 throwStrenght = new Vector2(Random.Range(-400f, 400f), Random.Range(50f, 400f));
        rb2d.AddForce(throwStrenght);
    }

    public void Pickup()
    {
        Object pickupObject = gameObject;
        GameManager.Instance.SendMessage(pickupType, pickupObject);   
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
