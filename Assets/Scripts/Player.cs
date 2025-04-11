using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Player movement speed")]
    public float speed = 5f;
    
    [Tooltip("Force applied when bouncing off boundaries")]
    public float bounceForce = 10f;
    
    [Header("Rotation Settings")]
    [Tooltip("Rotation speed around X axis")]
    public float rotationSpeedX = 15f;
    
    [Tooltip("Rotation speed around Y axis")]
    public float rotationSpeedY = 30f;
    
    [Tooltip("Rotation speed around Z axis")]
    public float rotationSpeedZ = 45f;

    [Header("Boundary Settings")]
    [Tooltip("Y position below which player resets")]
    public float resetYPosition = -10f;

    private Rigidbody rb;
    private GameManager gameManager;

    [System.Obsolete]
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();

        ConfigureRigidbody();
    }

    [System.Obsolete]
    private void ConfigureRigidbody()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing!");
            return;
        }

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.drag = 0.5f; // Optional: Add some drag for more natural movement
        rb.angularDrag = 0.05f;
    }

    [System.Obsolete]
    private void Update()
    {
        RotatePlayer();
        CheckBoundaries();
    }

    private void RotatePlayer()
    {
        transform.Rotate(
            new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime
        );
    }

    [System.Obsolete]
    private void CheckBoundaries()
    {
        if (transform.position.y < resetYPosition)
        {
            gameManager?.LogPlayerReset();
            ResetPosition();
        }
    }

    [System.Obsolete]
    private void ResetPosition()
    {
        transform.position = new Vector3(0, 1, 0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (rb == null) return;
        
        HandleMovement();
    }

    [System.Obsolete]
    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
        
        // Limit velocity
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Dome"))
        {
            gameManager?.LogDomeBounce();
            BounceOffDome(collision);
        }
    }

    private void BounceOffDome(Collision collision)
    {
        Vector3 bounceDirection = collision.contacts[0].normal;
        rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameManager?.LogEnemyCollision();
            Destroy(gameObject);
        }
    }
}