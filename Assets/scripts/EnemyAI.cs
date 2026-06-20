using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Ensures the enemy always has physics/gravity
public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }
    public EnemyState currentState;

    [Header("References")]
    public Transform player;
    private Rigidbody2D rb;

    [Header("Movement Settings")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("Distance Thresholds")]
    public float detectionRadius = 6f;
    public float attackRange = 1.5f;
    public float loseInterestRadius = 8f;

    [Header("Patrol Setup")]
    public Transform pointA;
    public Transform pointB;
    private Transform currentPatrolTarget;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    private float lastAttackTime;

    private float patrolSwitchCooldown = 0.5f;
    private float lastPatrolSwitchTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Dynamically find the player by tag. 
        // This solves your cross-scene issue perfectly!
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("No 'Player' tag found! Did you tag your dummy player in the test scene?");
            }
        }

        currentState = EnemyState.Patrol;
        currentPatrolTarget = pointA;
    }

    void Update()
    {
        // Prevent errors if there is no player
        if (player == null) return;

        CheckStateTransitions();
    }

    // Use FixedUpdate for physics-based movement (Rigidbody)
    void FixedUpdate()
    {
        if (player == null) return;

        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolBehavior();
                break;
            case EnemyState.Chase:
                ChaseBehavior();
                break;
            case EnemyState.Attack:
                AttackBehavior();
                break;
        }
    }

    void CheckStateTransitions()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (currentState == EnemyState.Patrol)
        {
            if (distanceToPlayer <= detectionRadius)
                currentState = EnemyState.Chase;
        }
        else if (currentState == EnemyState.Chase)
        {
            if (distanceToPlayer <= attackRange)
                currentState = EnemyState.Attack;
            else if (distanceToPlayer > loseInterestRadius)
                currentState = EnemyState.Patrol;
        }
        else if (currentState == EnemyState.Attack)
        {
            if (distanceToPlayer > attackRange)
                currentState = EnemyState.Chase;
        }
    }

    // --- PLATFORMER BEHAVIORS ---
    void PatrolBehavior()
    {
        float distToTarget = Mathf.Abs(transform.position.x - currentPatrolTarget.position.x);

        if (distToTarget < 0.5f && Time.time > lastPatrolSwitchTime + patrolSwitchCooldown)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            currentPatrolTarget = (currentPatrolTarget == pointA) ? pointB : pointA;
            lastPatrolSwitchTime = Time.time;
            return;
        }

        MoveTowardsTarget(currentPatrolTarget.position, patrolSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if what we hit has a Health component
        if (collision.TryGetComponent<Health>(out Health targetHealth))
        {
            targetHealth.TakeDamage(15); // Deals 15 damage
        }
    }

    void ChaseBehavior()
    {
        MoveTowardsTarget(player.position, chaseSpeed);
    }

    void AttackBehavior()
    {
        // Stop moving completely on the X axis to attack
        rb.velocity = new Vector2(0, rb.velocity.y);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Enemy Attacks the Player!");
            OnTriggerEnter2D(player.GetComponent<Collider2D>());
            lastAttackTime = Time.time;
        }
    }

    // --- THE PLATFORMER MOVEMENT LOGIC ---
    void MoveTowardsTarget(Vector3 targetPosition, float speed)
    {
        // 1. Calculate direction (-1 for Left, 1 for Right)
        float direction = Mathf.Sign(targetPosition.x - transform.position.x);
        // debug
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        //Debug.Log($"Direction: {direction} | Velocity set to: {rb.velocity}");
        //

        // 2. Apply velocity on X, but keep the current Y velocity so gravity still pulls them down
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        // 3. Flip the sprite to face the movement direction
        if (direction > 0)
            transform.localScale = new Vector3(1, 1, 1); // Facing Right
        else if (direction < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Facing Left
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}