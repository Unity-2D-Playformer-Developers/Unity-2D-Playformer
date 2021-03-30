using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBoxBehaviour : MonoBehaviour, IDamageable
{

    public void Damage(int damageAmount)
    {
        Destroy();
    }
    public  void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
