using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDrone : Enemy
{
    [Header("Platform Patrol")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;

    private bool movingRight = true;

    protected override void Update()
    {
        base.Update();

        if (isAggro)
            Chase();
        else
            Patrol();
    }

    private void Patrol()
    {
        if (!GroundAhead() || WallAhead())
            TurnAround();

        float dir = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
    }

    private void Chase()
    {
        float dir = player.position.x - transform.position.x;
        rb.velocity = new Vector2(Mathf.Sign(dir) * moveSpeed, rb.velocity.y);

        // Flip sprite to face player if needed
        if ((dir > 0 && !movingRight) || (dir < 0 && movingRight))
            TurnAround();
    }

    private void TurnAround()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private bool GroundAhead()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);
    }

    private bool WallAhead()
    {
        return Physics2D.Raycast(wallCheck.position, transform.right, 0.1f, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 0.2f);
        }

        if (wallCheck != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + transform.right * 0.1f);
        }
    }
}
