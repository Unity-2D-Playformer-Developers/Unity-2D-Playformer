using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class MPlayerMovement : NetworkBehaviour
{
    public float movementSpeed = 10;
    public float slopeMovementSpeedMod = 30; // how fast player can move upwards when walking on slope
    public float jumpHeight = 5;
    public float climbingSpeed = 10;

    public LayerMask GroundLayers;

    private Rigidbody2D playerRB;
    private CapsuleCollider2D playerCollider;

    private float inputX;
    private float inputY;

    public bool isGrounded;
    public bool isJumping;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    #region Input

    void OnMove(InputValue movementValue) // get player movement X axis: -1 -> going left; 1 -> going right; 0 -> no input
    {
        if (this.isLocalPlayer)
        {
            inputX = movementValue.Get<float>();
            Debug.Log(inputX);
        }
    }
    void OnJump() //action to perform when jump button is pressed
    {
        if (this.isLocalPlayer)
        {
            PlayerJump();
        }
    }

    #endregion

    private void FixedUpdate()
    {
        if (this.isLocalPlayer)
        {
            MovePlayer();
        }
    }



    #region Commands

    [Command]
    void MovePlayer()
    {
        playerRB.AddForce(new Vector2(inputX * movementSpeed, 0f));
    }
    [Command]
    void PlayerJump()
    {

        if (isGrounded)
        {
            Debug.Log("Jump");
            playerRB.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            isGrounded = false;
            isJumping = true;
        }

    }

    [Command]
    private void GroundCheck()
    {
        if (this.isLocalPlayer)
        {
            float extraHeight = 0.1f;
            RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0.1f, Vector2.down, extraHeight, GroundLayers);

            Color rayColor = Color.green;
            if (raycastHit.collider == true)
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x, 0), Vector2.down * (playerCollider.bounds.extents.y + extraHeight), rayColor);
            Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, 0), Vector2.down * (playerCollider.bounds.extents.y + extraHeight), rayColor);
            Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y + extraHeight), Vector2.right * (playerCollider.bounds.extents.x * 2f), rayColor);

            isGrounded = raycastHit.collider;
        }
    }

    #endregion
}
