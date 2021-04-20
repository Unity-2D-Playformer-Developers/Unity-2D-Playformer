using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private Rigidbody2D playerRB;

    void Start()
    {
        playerRB = GameManager.Instance.PlayerRB;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<IDamageable>() != null)
        {
            collision.collider.GetComponent<IDamageable>().Damage(1);
            //collision.collider.GetComponent<IDamageable>().Knockback(0.02f,1000,player.transform.position);
            
            float timer = 0;
           
            if (0.02f > timer)
            {
             
                float playerposition = playerRB.position.x;
                Debug.Log(playerposition);
                float spikesposition = this.transform.position.x;
                Debug.Log(spikesposition);
                timer += Time.deltaTime;

                if (spikesposition< playerposition)
                {
                    playerRB.AddForce(new Vector3(playerRB.transform.position.x * -100, playerRB.transform.position.y * -350, playerRB.transform.position.z));
                }
                else playerRB.AddForce(new Vector3(playerRB.transform.position.x * 100, playerRB.transform.position.y * -350, playerRB.transform.position.z));
                

            }
        }


    }
}
