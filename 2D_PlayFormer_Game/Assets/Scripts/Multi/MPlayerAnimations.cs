using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MPlayerAnimations : NetworkBehaviour
{
    private MPlayerMovement playerMovement;
    private Animator animator;

    private void Start()
    {
        playerMovement = GetComponent<MPlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(this.isLocalPlayer)
        {
            if(playerMovement.InputX==0)
            {
                animator.SetBool("movementX", false);
            }
            else
            {
                animator.SetBool("movementX", true);
            }
            animator.SetBool("isGrounded", playerMovement.isGrounded);
        }
    }

}
