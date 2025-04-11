using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Follow Settings")]
    [Tooltip("Offset from the player's position")]
    public Vector3 offset = new Vector3(0, 5, -10);
    
    [Tooltip("Smoothness of camera movement")]
    [Range(1f, 10f)]
    public float smoothSpeed = 5f;
    
    [Tooltip("Whether the camera should look at the player")]
    public bool lookAtPlayer = true;

    private Transform player;

    private void Start()
    {
        // Find the player by tag with error handling
        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log($"Camera successfully found player: {player.name}");
        }
        else
        {
            Debug.LogError("No GameObject found with the tag 'Player'! Camera follow will not work.");
        }
    }

    private void LateUpdate()
    {
        if (player == null) return;

        // Desired position with offset
        Vector3 targetPosition = player.position + offset;

        // Smooth transition using Vector3.Lerp
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Optional: Make the camera always look at the player
        if (lookAtPlayer)
        {
            transform.LookAt(player);
        }
    }

    // Optional method to manually set the player if not found automatically
    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
        Debug.Log($"Player manually set for camera: {player.name}");
    }
}