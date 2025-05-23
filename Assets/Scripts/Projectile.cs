using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   
    // Start is called before the first frame update
    public float projectileSpeed = 10f;
    Rigidbody2D rb;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();   
       ;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
            Destroy(gameObject);
if(collision.gameObject.tag=="Dummy"){
  Instantiate(PlayerAbility.Energy, collision.gameObject.transform.position, transform.rotation).SetActive(true);
}
    }
}
