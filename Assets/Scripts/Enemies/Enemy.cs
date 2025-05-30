using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] protected float aggroRange = 5f;
    [SerializeField] protected float disengageRange = 10f;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float stoppingDistance = 3f;

    [Space(20)]
    public float maxHealth = 10f;
    public float currentHealth;

    protected Transform player;
    protected Rigidbody2D rb;
    protected bool isAggro;
    protected bool canAttack;
    public bool IsAggro => isAggro;
    public bool CanAttack => canAttack && isAggro;

    // Set to static since I don't think we will care much which enemy dies
    //      in cases where we are tracking enemy death. If we do, we can
    //      remove the static declaration.
    public static Action<Enemy> OnEnemyDeath;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>().transform; // Might be better to reference the player class/component

        currentHealth = maxHealth;
    }

    #region AGGRO LOGIC
    protected virtual void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (ShouldChasePlayer()) {
            if (!isAggro) {
                isAggro = true;
                OnChaseStart();
            }
        }else {
            if (isAggro) {
                isAggro = false;
                OnChaseEnd();
            }
        }
    }

    // Empty body in parent class to allow for enemies that do not require these methods (ie. Hazard-style enemies)
    protected virtual void OnChaseStart() { }
    protected virtual void OnChaseEnd() { }

    protected virtual bool ShouldChasePlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        return !isAggro && distance <= aggroRange || isAggro && distance <= disengageRange;
        // Additional check logic in child classes can be written as such:
        // -- "return base.ShouldChasePlayer() && <Insert Additional Logic Here>" ie. !Player.Instance.IsHidden
    }
    #endregion

    #region HEALTH
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    protected void Die()
    {
        // Death Animation
        // Other Death Logic (events, delays, etc)
        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }
    #endregion

    #region HELPER FUNCTIONS
    protected float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, player.position);
    }

    protected Vector2 GetDirectionToPlayer()
    {
        return player.position - transform.position;
    }
    #endregion

    #region DEBUG
    protected virtual void OnDrawGizmos()
    {
        // Aggro range visual
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        // Disengage range visual
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, disengageRange);
    }
    #endregion
}
