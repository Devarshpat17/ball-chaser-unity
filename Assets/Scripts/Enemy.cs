using UnityEngine;

public enum EnemyType
{
    Chaser,
    Stationary,
    Patrol
}

public class Enemy : MonoBehaviour
{
    [Header("Enemy Configuration")]
    public EnemyType enemyType = EnemyType.Chaser;
    
    [Tooltip("Movement speed for chasing or patrolling")]
    public float moveSpeed = 3f;
    
    [Tooltip("Detection radius for player tracking")]
    public float detectionRadius = 10f;
    
    [Tooltip("For patrol type, define patrol points")]
    public Transform[] patrolPoints;

    [Header("Patrol Settings")]
    public float waitTime = 2f;
    public float stoppingDistance = 0.5f;

    private Transform player;
    private GameManager gameManager;
    private int currentPatrolIndex = 0;
    private float waitCounter = 0f;
    private Rigidbody rb;
    private Vector3 initialPosition;

    [System.Obsolete]
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Find player and game manager
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();

        // Store initial position for stationary reset
        initialPosition = transform.position;

        // Validate patrol points for patrol type
        if (enemyType == EnemyType.Patrol && (patrolPoints == null || patrolPoints.Length == 0))
        {
            Debug.LogWarning("Patrol type enemy needs patrol points! Defaulting to chaser.");
            enemyType = EnemyType.Chaser;
        }
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        switch (enemyType)
        {
            case EnemyType.Chaser:
                ChasePlayer();
                break;
            case EnemyType.Stationary:
                DetectPlayerNearby();
                break;
            case EnemyType.Patrol:
                PatrolArea();
                break;
        }
    }

    [System.Obsolete]
    private void ChasePlayer()
    {
        if (player == null) return;

        // Calculate direction to player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        
        // Move towards player
        rb.velocity = directionToPlayer * moveSpeed;

        // Rotate to face player
        transform.LookAt(player);
    }

    private void DetectPlayerNearby()
    {
        if (player == null) return;

        // Check distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRadius)
        {
            // Optional: Trigger an alert or warning
            Debug.Log("Player detected near stationary enemy!");
        }
    }

    [System.Obsolete]
    private void PatrolArea()
    {
        if (patrolPoints.Length == 0) return;

        // Current patrol target
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        
        // Calculate direction to current patrol point
        Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;
        
        // Move towards patrol point
        rb.velocity = directionToTarget * moveSpeed;

        // Check if reached the patrol point
        if (Vector3.Distance(transform.position, targetPoint.position) <= stoppingDistance)
        {
            waitCounter += Time.fixedDeltaTime;
            
            // Wait at patrol point
            if (waitCounter >= waitTime)
            {
                // Move to next patrol point
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                waitCounter = 0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If player collides, log and potentially destroy
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager?.LogEnemyCollision();
            Debug.Log("Enemy made contact with player!");
        }
    }

    // Optional method to reset enemy position
    [System.Obsolete]
    public void ResetPosition()
    {
        transform.position = initialPosition;
        rb.velocity = Vector3.zero;
    }

    // Visualization of detection radius in editor
    private void OnDrawGizmosSelected()
    {
        // Draw detection radius in Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}