using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   Vector3 min= Vector3.one*-1000;
    // Start is called before the first frame update
    public float projectileSpeed = 10f;
    Rigidbody2D rb;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();   
       
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
    if (collision.gameObject.CompareTag("Enemy")) {
            Instantiate(PlayerAbility.Energy, collision.gameObject.transform.position, transform.rotation).SetActive(true);
            Destroy(collision.gameObject);
    }

    if (!collision.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }
}
