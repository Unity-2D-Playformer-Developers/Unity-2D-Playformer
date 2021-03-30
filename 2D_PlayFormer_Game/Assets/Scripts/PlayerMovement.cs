using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    public float movementSpeed = 10;
    public float jumpHeight = 5;
    public float climbingSpeed = 10;

    private Rigidbody2D rb2d;
    private Animator animator;
    private CapsuleCollider2D playerCollider;
    private PlayerActions playerActions;
    private Vector3 movementDirection;

    private float rb2dGravityScale;
    private float rb2dDrag;

    private float movementX; // used to determine player movement direction in X axis
    private float movementY; // used to determine player movement direction in Y axis

    private bool canClimb; // used to determine if player can enable climbing mode
    private bool isClimbing; // used to determine if player is currently climbing
    private bool isJumping; // used to determine if player is jumping
    private bool isGrounded; // used to determine if player is on the ground

    void Start()
    {
        
        playerActions = GetComponent<PlayerActions>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        rb2dGravityScale = rb2d.gravityScale;
        rb2dDrag = rb2d.drag;
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
        if(isJumping==true)
        {
            animator.SetTrigger("jumpTrigger");
            isJumping = false; // player no longer considered jumping after animation starts
        }
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isClimbing", isClimbing);
    }

    void FlipPlayer() // flip player sprite based on movement direction
    {     
        if(movementX>0) // movement right
        {
            movementDirection = new Vector3(0.8f, 0.8f, 0.8f);
        }
        else if(movementX<0) // movement left
        {
            movementDirection = new Vector3(-0.8f, 0.8f, 0.8f);
        }

        transform.localScale = movementDirection;
    }

    void OnClimb(InputValue movementValue) // action to perform when climbing buttons are pressed
    {
        if (isClimbing == true)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementY = movementVector.y;
            Debug.Log("movement y=" + movementVector.y);
        }
        else if (canClimb==true)
        {
            EnableClimbingMode(true);
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementY = movementVector.y;
            Debug.Log("movement y=" + movementVector.y);
        }
        else // when player is not allowed to use climbing mode
        {
            movementY = 0.0f;
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
            EnableClimbingMode(false);
        }
        else if (isGrounded == false)
        {
            playerActions.JumpAttack();
        }
    }

    void EnableClimbingMode(bool enable) 
    {
        if (enable == true && isClimbing == false && canClimb==true)
        {
            rb2d.gravityScale = 0;
            rb2d.drag = 20;
            isClimbing = true;
        }
        else if(enable==false)
        {
            movementY = 0f;
            rb2d.gravityScale = rb2dGravityScale;
            rb2d.drag = rb2dDrag;
            isClimbing = false;
        }
    }

    private bool CheckIfGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, extraHeight, groundLayer);

        Color rayColor = Color.green;
        if (raycastHit.collider == true)
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x, 0), Vector2.down * (playerCollider.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, 0), Vector2.down * (playerCollider.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y + extraHeight), Vector2.right * (playerCollider.bounds.extents.x * 2f), rayColor);
            
        return raycastHit.collider != null;

    }

    private void Update()
    {
    }

    void FixedUpdate()
    {
        Vector2 movementLeftRightUpDown = new Vector2(movementX * movementSpeed, movementY * climbingSpeed);
        rb2d.AddForce(movementLeftRightUpDown);
        isGrounded = CheckIfGrounded();
        SetAnimationParameters();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag) //perform action based on trigger tag
        {

            case "Climbing": // allows player to enable climbing mode
                canClimb = true;
                Debug.Log("canClimb = true");
                break;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag) //perform action based on trigger tag
        {

            case "Climbing": // player no longer able to use climbing mode
                canClimb = false;
                if (isClimbing == true)
                {
                    EnableClimbingMode(false);
                }
                Debug.Log("canClimb = false");
                break;

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag) //perform action based on trigger tag
        {

            case "Ground":
                isJumping = false;
                break;

            case "Bounce":
                isJumping = false;
                break;
        }
    }

    bool GetIsPlayerGrounded()
    {
        return isGrounded;
    }
    bool GetIsPlayerClimbing()
    {
        return isClimbing;
    }
    float GetPlayerDirection()
    {
        return movementX;
    }

}
