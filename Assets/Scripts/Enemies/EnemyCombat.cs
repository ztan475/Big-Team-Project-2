using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCombat : MonoBehaviour
{
    [SerializeField] protected float cooldown = 1f;
    protected float cooldownTimer;

    protected Enemy enemy;
    protected Transform player;
    protected PlayerMovement playerMovement; // Change to reference player health script

    protected virtual void Start()
    {
        enemy = GetComponent<Enemy>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        player = playerMovement.transform;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (CanAttack()) {
            PerformAttack();
            cooldownTimer = cooldown;
        }
    }

    protected virtual bool CanAttack()
    {
        // Attack is off cooldown
        // Player exists
        // Enemy is aggroed and within attack range
        return cooldownTimer <= 0f && playerMovement != null && enemy.CanAttack;
    }

    protected void SetEnemy(Enemy enemyReference) => enemy = enemyReference;

    protected abstract void PerformAttack();
}
