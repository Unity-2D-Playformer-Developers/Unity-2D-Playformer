using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            IDamageable player = collision.GetComponent<IDamageable>();
            if(player!=null)
            {
                player.TakeDamage(999);
            }
        }
    }
}
