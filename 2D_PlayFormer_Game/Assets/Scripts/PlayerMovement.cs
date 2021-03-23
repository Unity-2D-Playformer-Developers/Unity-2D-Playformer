using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10;
    public float jumpHeight = 5;
    public float climbingSpeed = 10;


    private Rigidbody2D rb2d;
    private Animator animator;
    private Vector3 movementDirection;

    private float rb2dGravityScale;

    private float movementX;
    private float movementY;
    
    private bool canClimb=false;




    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2dGravityScale = rb2d.gravityScale;
        animator = GetComponentInChildren<Animator>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        FlipPlayer();
        SetAnimationParameters();
    }


    void SetAnimationParameters()
    {
        animator.SetFloat("movementSpeedX", Mathf.Abs(movementX));
    }

    void FlipPlayer()
    {     
        if(movementX>0) // movement right
        {
            movementDirection = new Vector3(1f, 1f, 1f);
        }
        else if(movementX<0) // movement left
        {
            movementDirection = new Vector3(-1f, 1f, 1f);
        }

        transform.localScale = movementDirection;              
    }

    void OnClimb(InputValue movementValue) //do poprawy
    {
        if (canClimb == true)
        {
            rb2d.gravityScale = 0;
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementY = movementVector.y;
            Debug.Log("movement y=" + movementVector.y);
        }
        else
        {
            movementY = 0.0f;
        }
    }

    void OnJump()
    {
        Debug.Log("Jump");
        if (Mathf.Abs(rb2d.velocity.y) < 0.001f)
        {
            rb2d.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 movementLeftRightUpDown = new Vector2(movementX * movementSpeed, movementY*climbingSpeed);
        rb2d.AddForce(movementLeftRightUpDown);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "ClimbingTrigger")
        {
            canClimb = true;
            Debug.Log("canClimb = true");
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "ClimbingTrigger")
        {
            rb2d.gravityScale = rb2dGravityScale;
            canClimb = false;
            Debug.Log("canClimb = false");
        }
    }


}
