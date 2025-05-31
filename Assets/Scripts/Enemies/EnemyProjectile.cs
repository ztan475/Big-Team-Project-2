using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 4;
    [SerializeField] private float lifetime = 5f;
    private PlayerAbility player;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            // Deal Damage Player
            player = (player == null) ? other.gameObject.GetComponent<PlayerAbility>() : player;
            player.Damage(damage);
        }

        Destroy(gameObject);
    }
}
