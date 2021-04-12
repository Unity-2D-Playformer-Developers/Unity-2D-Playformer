using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBoxBehaviour : MonoBehaviour, IDamageable
{
    public GameObject BoxContent;
    public bool ContentRandomAmount = true;
    public int ContentMaxAmount = 5;
    public Transform ContentSpawnPosition;


    void Start()
    {
    }

    public void Damage(int damageAmount)
    {
        Destroy();
    }
    public void Destroy()
    {
        GetComponent<Animator>().SetTrigger("Destroy");
        GetComponent<BoxCollider2D>().enabled = false;
        SpawnContent();
    }

    void SpawnContent()
    {
        int spawnAmount;

        if (ContentRandomAmount)
        {
            spawnAmount = Random.Range(1, ContentMaxAmount);
        }
        else
        {
            spawnAmount = ContentMaxAmount;
        }

        for (int i = spawnAmount; i > 0; i--)
        {
            GameObject pickup = Instantiate(BoxContent, ContentSpawnPosition);
            pickup.transform.parent = null;
        }

    }


}
