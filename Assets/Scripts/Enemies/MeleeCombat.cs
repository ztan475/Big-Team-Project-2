using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : EnemyCombat
{
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerMask;

    protected override bool CanAttack()
    {
        return base.CanAttack() && InRange();
    }

    private bool InRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerMask);
        return hit != null;
    }

    protected override void PerformAttack()
    {
        Debug.Log("Perform Attack");
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerMask);
        if (hit != null && hit.CompareTag("Player")) {
            Debug.Log("DAMAGE PLAYER");
            // Player.Instance.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
