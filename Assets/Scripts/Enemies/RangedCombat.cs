using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCombat : EnemyCombat
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float fxDuration;

    protected override void Start()
    {
        base.Start();

        if (sprite != null) sprite.enabled = false;
    }

    protected override void PerformAttack()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (player.position - firePoint.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

        sprite.enabled = true;
        Invoke("DisableFX", fxDuration);
    }

    private void DisableFX() => sprite.enabled = false;
}
