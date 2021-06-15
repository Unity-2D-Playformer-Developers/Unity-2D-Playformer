using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Class responsible for managing player movement - detects slopes, climping triggers, groundchecks, etc.
    /// </summary>

    [SerializeField] private LayerMask groundLayer;

    public float movementSpeed = 10;
    public float slopeMovementSpeedMod=30; // how fast player can move upwards when walking on slope
    public float jumpHeight = 5;
    public float climbingSpeed = 10;

    /// <summary>
    /// component references
    /// </summary>
    private Rigidbody2D rb2d;
    private Animator animator;
    private CapsuleCollider2D playerCollider;
    private PlayerActions playerActions;
    private SpriteRenderer sprite;

    private float rb2dGravityScale;
    private float rb2dDrag;

    private float movementX; // used to determine player movement direction in X axis
    private float movementY; // used to determine player movement direction in Y axis
    private Vector2 slopeRaycastNormal;

    private Vector3 movementDirection = new Vector3();
    private Quaternion rotation; // rotation of player around Z axis when walking on the slope

    private bool canClimb; // used to determine if player can enable climbing mode
    private bool isClimbing; // used to determine if player is currently climbing
    private bool isJumping; // used to determine if player is jumping
    private bool isGrounded; // used to determine if player is on the ground
    private bool onSlope; // used to determine if player is walking on slope

    public bool IsClimbing { get => isClimbing; }

    void Start()
    {
        
        playerActions = GetComponent<PlayerActions>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb2dGravityScale = rb2d.gravityScale;
        rb2dDrag = rb2d.drag;
    }

    /// <summary>
    /// get player movement X axis: -1 -> going left; 1 -> going right; 0 -> no input
    /// </summary>
    /// <param name="movementValue"></param>
    void OnMove(InputValue movementValue)
    {        
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        FlipPlayer();
    }

    /// <summary>
    /// method responsible for animating player
    /// </summary>
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

    /// <summary>
    /// flip player sprite based on movement direction
    /// </summary>
    void FlipPlayer() 
    {
        if(movementX>0) // movement right
        {
            sprite.flipX = false;
        }
        else if(movementX<0) // movement left
        {
            sprite.flipX = true;           
        }
    }

    /// <summary>
    /// rotates sprite, when players walks on slope
    /// </summary>
    void RotatePlayer() 
    {
        if (onSlope)
        {
            rotation = Quaternion.Euler(0f, 0f, -slopeRaycastNormal.x * 20);
        }
        else
        {
            rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        transform.rotation = rotation;

    }

    /// <summary>
    /// action to perform when climbing buttons are pressed
    /// </summary>
    /// <param name="movementValue"></param>
    void OnClimb(InputValue movementValue)
    {
        if (isClimbing == true)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementY = movementVector.y;
            Debug.Log("movement y=" + movementVector.y);
        }
        else if (canClimb==true)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            print("climbing:\t" + isGrounded + "\t" + movementVector.y);
            if (isGrounded && movementVector.y <= 0)
            {
                return;
            }
            EnableClimbingMode(true);
            movementY = movementVector.y;
            Debug.Log("movement y=" + movementVector.y);
        }
        else // when player is not allowed to use climbing mode
        {
            movementY = 0.0f;
        }
    }

    /// <summary>
    /// action to perform when jump button is pressed
    /// </summary>
    void OnJump()
    {
        if (isGrounded==true)
        {
            Debug.Log("Jump");
            rb2d.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            isGrounded = false;
            isJumping = true;
        }
        else if(isClimbing==true)
        {
            EnableClimbingMode(false);
        }
        else if (isGrounded == false)
        {
            playerActions.JumpAttack(true);
        }
    }

    /// <summary>
    /// enables/disables climbing mode, sets up movement values
    /// </summary>
    /// <param name="enable"></param>
    public void EnableClimbingMode(bool enable) 
    {
        if (enable == true && isClimbing == false && canClimb==true)
        {
            rb2d.gravityScale = 0;
            rb2d.drag = 20;
            isClimbing = true;
        }
        else if(enable==false)
        {
            isClimbing = false;
            movementY = 0f;
            rb2d.gravityScale = rb2dGravityScale;
            rb2d.drag = rb2dDrag;
        }
    }

    /// <summary>
    /// checks using boxcast if player is on the ground
    /// </summary>
    /// <returns>grounded bool value</returns>
    private bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0.1f, Vector2.down, extraHeight, groundLayer);

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

    /// <summary>
    /// checks using raycast and raycast normal if player is on the slope
    /// </summary>
    private void SlopeCheck()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + 0.5f, groundLayer);
        slopeRaycastNormal = raycastHit.normal;
        Debug.DrawRay(playerCollider.bounds.center, -slopeRaycastNormal, Color.cyan);
        
        if (slopeRaycastNormal.x!=0)
        {
            onSlope = true;
            RotatePlayer();
        }
        else
        {
            onSlope = false;
            RotatePlayer();
        }
        
        //Debug.Log(slopeRaycastNormal = raycastHit.normal);
    }

    private void Update()
    {
    }

    /// <summary>
    /// updates movement and animations
    /// </summary>
    void FixedUpdate()
    {
        Vector2 movementLeftRightUpDown;
        Vector2 slopeMovement;
        
        SlopeCheck();
        if (onSlope)
        {
            slopeMovement = new Vector2(-slopeRaycastNormal.x * slopeMovementSpeedMod, slopeRaycastNormal.y*1.5f);
            rb2d.AddForce(slopeMovement);
        }
        movementLeftRightUpDown = new Vector2(movementX * movementSpeed, movementY * climbingSpeed);
        rb2d.AddForce( movementLeftRightUpDown);

        if (IsGrounded())
        {
            isGrounded = true;
            playerActions.JumpAttack(false);
        }
        else
        {
            isGrounded = false;
        }

        if(!isClimbing && !playerActions.IsPerformingJumpAttack)
        {
            rb2d.gravityScale = rb2dGravityScale;
            rb2d.drag = rb2dDrag;
        }


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
        if(collision.collider.IsTouchingLayers(groundLayer))
        {
            isJumping = false;
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
