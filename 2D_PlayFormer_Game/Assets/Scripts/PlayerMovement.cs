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
    private float rb2dDrag;

    private float movementX;
    private float movementY;
    
    private bool canClimb;
    private bool isClimbing;
    private bool isJumping;
    private bool isGrounded;

    void Start()
    {
        canClimb = false;
        rb2d = GetComponent<Rigidbody2D>();
        rb2dGravityScale = rb2d.gravityScale;
        rb2dDrag = rb2d.drag;
        animator = GetComponentInChildren<Animator>();
    }

    void OnMove(InputValue movementValue) // get player movement X axis: -1 -> going left; -2 -> going right; 0 -> no input
    {
     
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        FlipPlayer();
    }


    void SetAnimationParameters()
    {
        animator.SetFloat("movementSpeedX", Mathf.Abs(movementX));
        animator.SetFloat("movementSpeedY", Mathf.Abs(movementY));
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isClimbing", isClimbing);
    }

    void FlipPlayer() // flip player sprite based on movement direction
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

    void OnClimb(InputValue movementValue) //action to perform when climbing buttons are pressed
    {
        if (isClimbing == true)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementY = movementVector.y;
            Debug.Log("movement y=" + movementVector.y);
        }
        else if (canClimb==true)
        {
            EnableDisableClimbingMode();

            Vector2 movementVector = movementValue.Get<Vector2>();
            movementY = movementVector.y;
            Debug.Log("movement y=" + movementVector.y);
        }
        else
        {
            movementY = 0.0f;
            //EnableDisableClimbingMode();
            Debug.Log("OnClimb last");
        }
    }

    void OnJump() //action to perform when jump button is pressed
    {
        if (isGrounded==true)
        {
            Debug.Log("Jump");
            rb2d.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            isJumping = true;
        }
        else if(isClimbing==true)
        {
            EnableDisableClimbingMode();
        }
    }

    void EnableDisableClimbingMode() 
    {
        if (isClimbing == false && canClimb==true)
        {
            Debug.Log("if climbing");
            rb2d.gravityScale = 0;
            rb2d.drag = 20;
            isClimbing = true;
        }
        else if(isClimbing==false && canClimb==false)
        {
            Debug.Log("if not climbing");
            rb2d.gravityScale = rb2dGravityScale;
            rb2d.drag = rb2dDrag;
            isClimbing = false;

        }
    }

    void FixedUpdate()
    {
        Vector2 movementLeftRightUpDown = new Vector2(movementX * movementSpeed, movementY*climbingSpeed);
        rb2d.AddForce(movementLeftRightUpDown);
        SetAnimationParameters();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.tag;

        switch(collisionTag) //perform action based on trigger tag
        {

            case "Climbing": // allows player to enable climbing mode
                {
                    canClimb = true;
                    Debug.Log("canClimb = true");
                }break;
                

            case "Ground": // detect if player is touching ground
                {
                    isGrounded = true;
                    isJumping = false; // player no longer in jumping state after landing
                    Debug.Log("On ground");
                }break;
                
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string collisionTag = collision.tag;

        switch (collisionTag) //perform action based on trigger tag
        {

            case "Climbing": // player no longer able to use climbing mode
                {
                    canClimb = false;
                    isClimbing = false;
                    EnableDisableClimbingMode();
                    Debug.Log("canClimb = false");
                }
                break;

            case "Ground": // detect if player is touching ground
                {
                    isGrounded = false;
                    Debug.Log("In air");
                }
                break;
        }
    }

}
