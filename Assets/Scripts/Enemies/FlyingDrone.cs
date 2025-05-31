using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FlyingDrone : Enemy
{
    [Header("Waypoint Patrol")]
    [SerializeField] private Transform pathContainer;
    [SerializeField] private bool loopPath = true;
    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;
    private bool reversed;

    [Header("Chase Settings")]
    
    [SerializeField] private LayerMask obstaclemask;
    [SerializeField] private float sightCheckInterval = 0.5f;
    private float sightTimer;
    private bool hasLOS; // has line of sight (LOS) to the player;

    protected override void Start()
    {
        base.Start();
        foreach (Transform point in pathContainer) waypoints.Add(point);
        transform.position = waypoints[currentWaypointIndex].position;
        currentHealth = maxHealth;
    }

    protected override void Update()
    {
        base.Update();

        IntervalSightCheck();

        if (isAggro && hasLOS) Chase();
        else if (!isAggro) Patrol();
    }

    private void Chase() 
    {
        if (GetDistanceToPlayer() <= stoppingDistance) {
            rb.velocity = Vector2.zero;
            if (!canAttack) canAttack = true;
            return;
        }

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * moveSpeed;
        if (canAttack) canAttack = false;
    }

    private void Patrol()
    {
        if (waypoints.Count <= 0) return; // Stationary unless waypoints are defined

        Transform target = waypoints[currentWaypointIndex];
        Vector2 dir = (target.position - transform.position).normalized;
        rb.velocity = dir * moveSpeed;

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
            SetNextWaypoint();
    }

    private void SetNextWaypoint()
    {
        if (loopPath) {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }else {
            if (reversed) currentWaypointIndex--;
            else currentWaypointIndex++;

            if (currentWaypointIndex == 0 || currentWaypointIndex == waypoints.Count - 1)
                reversed = !reversed;
        }
    }

    private void IntervalSightCheck()
    {
        sightTimer -= Time.deltaTime;
        if (sightTimer <= 0f) {
            sightTimer = sightCheckInterval;
            hasLOS = HasLineOfSight();
        }
    }

    protected override bool ShouldChasePlayer()
    {
        return base.ShouldChasePlayer() && HasLineOfSight();
    }

    private bool HasLineOfSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, GetDirectionToPlayer().normalized, aggroRange, obstaclemask);
        return hit.collider == null || hit.collider.CompareTag("Player");
        // Detect for no obstacle or the player to decide LOS
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // Enemy line of sight
        //Gizmos.color = hasLOS ? Color.green : Color.red;
        //Gizmos.DrawLine(transform.position, player.position);

        // Patrol path
        //Gizmos.color = Color.cyan;
        //for (int i = 0; i < waypoints.Count - 1; i++)
        //    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);

        //if (loopPath)
        //    Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
    }
}
