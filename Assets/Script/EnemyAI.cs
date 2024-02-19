using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour
{
    public int hitPoints = 1;   
    public GameObject swordSlashPrefab;
    public Transform playerTransform;
    Vector2 detectedDirection;    

    public float transitionTime = 0.5f; // Adjustable transition time in seconds
    void Start(){
            // Find the player object in the scene based on its tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Check if the player object is found
        if (playerObject != null)
        {
            // Get the Transform component of the player object
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found in the scene.");
        }

    }

    void Update()
    {
        // Check if the spacebar is pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MoveTowardsPlayer();
            }
    }

// Method to find the path from the enemy to the player and move the enemy
 public void MoveTowardsPlayer()
{
    // Find a path from the enemy's position to the player's position
    Path path = ABPath.Construct(transform.position, playerTransform.position, null);
    AstarPath.StartPath(path);
    path.BlockUntilCalculated();

    // Get the waypoints along the path
    Vector3[] waypoints = path.vectorPath.ToArray();

    // Check if there are waypoints to move towards
    if (waypoints.Length > 1)
    {
        // Start a coroutine for smooth movement
        StartCoroutine(MoveSmoothly(waypoints[1]));
    }
    
}

private IEnumerator MoveSmoothly(Vector3 targetPosition)
{
    float elapsedTime = 0f;
    Vector3 startingPosition = transform.position;

    while (elapsedTime < transitionTime)
    {
        // Calculate the interpolation factor
        float t = elapsedTime / transitionTime;

        // Move towards the target position smoothly using Lerp
        transform.position = Vector3.Lerp(startingPosition, targetPosition, t);

        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Wait for the next frame
        yield return null;
    }

    // Ensure the enemy reaches the exact target position
    transform.position = targetPosition;
}

    // Method to draw the path (similar to your existing paths method)
    void DrawPath(List<Vector3> waypoints)
    {
        if (waypoints != null)
        {
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                Debug.DrawLine(waypoints[i], waypoints[i + 1], Color.red);
            }
        }
    }


public bool PlayerDetectedByRaycasts(out Vector2 detectedDirection)
{
    detectedDirection = Vector2.zero; // Initialize to zero vector

    // Raycast directions (you can customize these based on your game's layout)
    Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    // Raycast distance
    float raycastDistance = 1f; // Adjust as needed

    foreach (Vector2 direction in directions)
    {
        // Cast a raycast from the center of the enemy in the specified direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)direction * 0.5f, direction, raycastDistance, LayerMask.GetMask("Player"));

         // Draw the raycast for visualization
        Debug.DrawRay(transform.position + (Vector3)direction * 0.5f, direction * raycastDistance, Color.red);

        // Check if the ray hits an object with the "Player" tag
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            //Debug.Log("Player detected by raycast in direction: " + direction);
            detectedDirection = direction; // Set detectedDirection to the current direction
            return true;
        }
    }

    return false;
}



    public void TakeDamage(int damageAmount)
    {
        hitPoints -= damageAmount;

        // Check if hit points are zero or less
        if (hitPoints <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Perform any death-related actions here (e.g., play death animation, spawn particles, etc.)
        Debug.Log("Enemy died!");

        // Destroy the game object
        Destroy(gameObject);
    }

    // Function to spawn a sword slash prefab in front of the enemy
    private void SpawnSwordSlash(Vector2 spawnDirection)
    {
        if (swordSlashPrefab != null)
        {
            // Calculate the spawn position one cell in front of the enemy
            Vector3 spawnPosition = transform.position + (Vector3)spawnDirection;

            // Instantiate the sword slash prefab at the calculated position
            GameObject swordSlash = Instantiate(swordSlashPrefab, spawnPosition, Quaternion.identity);

            // Destroy the sword slash prefab after a delay (you can adjust this value)
            Destroy(swordSlash, 1.5f);
        }
        else
        {
            Debug.LogError("Sword Slash Prefab not assigned in the Inspector.");
        }
    }

        // Function to get the facing direction of the enemy in grid coordinates
    private Vector3 GetFacingDirection()
    {
        // Assuming the enemy is facing in the positive X direction initially
        return transform.right;
    }


    public void enemyaction(){

        if(PlayerDetectedByRaycasts(out Vector2 detectedDirection)){
            SpawnSwordSlash(detectedDirection);
        } else {

            MoveTowardsPlayer();
        }
    }
}
