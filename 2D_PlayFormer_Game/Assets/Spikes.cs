using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private PlayerActions player;
    public Rigidbody2D playercharacter;
    // Start is called before the first frame update
    void Start()
    {
        player = playercharacter.GetComponent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
             
                 float playerposition = playercharacter.position.x;
                Debug.Log(playerposition);
                float spikesposition = this.transform.position.x;
                Debug.Log(spikesposition);
                timer += Time.deltaTime;

                if (spikesposition< playerposition)
                {
                    playercharacter.AddForce(new Vector3(playercharacter.transform.position.x * -100, playercharacter.transform.position.y * -350, playercharacter.transform.position.z));
                }
                else playercharacter.AddForce(new Vector3(playercharacter.transform.position.x * 100, playercharacter.transform.position.y * -350, playercharacter.transform.position.z));


                

            }
        }


    }
}
