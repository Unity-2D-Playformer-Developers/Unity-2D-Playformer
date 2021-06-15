using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class deifning behaviour of moving platforms.
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    public float MovementSpeed;
    
    /// <summary>
    /// Platform moves between point A and B
    /// </summary>
    public Transform PointA;
    public Transform PointB;
    private Vector3 pointA;
    private Vector3 pointB;

    private Vector3 nextPosition;/// <summary>
    private Vector3 startPosition;/// <summary>
    private Rigidbody2D rb;


    private void Start()
    {
        pointA = PointA.position;
        pointB = PointB.position;
        nextPosition = pointB;
        startPosition = transform.position;


        rb = GetComponent<Rigidbody2D>();

    }

    /// <summary>
    /// Method managing platform movement.
    /// </summary>
    private void FixedUpdate()
    {
        if (transform.position.x <= pointA.x)
        {
            nextPosition = pointB;
        }
        if(transform.position.x >= pointB.x)
        {
            nextPosition = pointA;
        }

        if(nextPosition == pointA)
        {
            rb.velocity=(new Vector2(-MovementSpeed, 0f));
        }
        else if (nextPosition == pointB)
        {
            rb.velocity=(new Vector2(MovementSpeed, 0f));
        }
    }

    /// <summary>
    /// Platform becomes player transform parent when he collides with it.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag=="Player")
        {
            GameObject player = collision.collider.gameObject;
            player.transform.parent.parent = transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            GameObject player = collision.collider.gameObject;
            player.transform.parent.parent = null;
   
        }
    }



}
