using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charcter2DController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    int Life = 10;

    public float MovementSpeed = 1;
    public float JumpForce = 2; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;

        if(Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
    }
}
