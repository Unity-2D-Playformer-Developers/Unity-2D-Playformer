using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charcter2DController : MonoBehaviour
{
    private Rigidbody2D rb;

    //Animator anim;
    //int Life = 10;

    private float movementInputDirection;
    public float MovementSpeed = 10.0f;
    public float JumpForce = 16;
    private bool isfacingRight = true;
    private bool isWalking;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {

        CheckInput();
        CheckMoovementDirection();
        UpdateAnimations();
        //var movement = Input.GetAxis("Horizontal");
        //transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;

        //if(Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        //{
        //    rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        //}
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalkig", isWalking);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void CheckMoovementDirection()
    {
        if(isfacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isfacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if(rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    private void Flip()
    {
        isfacingRight = !isfacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector2(MovementSpeed * movementInputDirection, rb.velocity.y);
    }
}
