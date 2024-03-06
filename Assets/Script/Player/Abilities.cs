using UnityEngine;
using UnityEngine.TextCore;
using System;
using System.Collections;

public class Abilities : MonoBehaviour
{
    public float destroyDelay = 1.5f; // Adjustable delay before destroying the sword slash prefab
    public GameObject swordSlashPrefab;
    public Transform playerTransform;
    public Gameplay gameplay;

    public GameObject moveMarkerPrefab;
    //public DestroyMoveMarker destroyMoveMarker;

    public Rigidbody2D playerRigidbody2D;

    public float transitionTime = 0.5f; // Adjustable transition time in seconds
    
    public void Attack()
    {
        if (swordSlashPrefab != null && playerTransform != null)
        {
            // Check if there are directions stored in the list
            if (gameplay.playerDirections.Count > 0)
            {
                // Get the earliest direction from the list
                int earliestDirection = gameplay.playerDirections[0];

                // Convert the direction to a Vector3
                Vector3 attackDirection = GetVectorFromDirection(earliestDirection);

                // Calculate the position for spawning the sword slash
                Vector3 spawnPosition = playerTransform.position + attackDirection;

                // Instantiate the sword slash prefab at the calculated position
                GameObject swordSlash = Instantiate(swordSlashPrefab, spawnPosition, Quaternion.identity);

                // Set player rotation based on the earliest direction
                playerTransform.eulerAngles = new Vector3(0, 0, earliestDirection);

                // Destroy the sword slash prefab after the specified delay
                Destroy(swordSlash, destroyDelay);

                // Remove the earliest direction from the list
                gameplay.playerDirections.RemoveAt(0);
            }
            else
            {
                Debug.LogError("No directions stored in the list. Unable to determine attack direction.");
            }  
        }
        else
        {
            Debug.LogError("Sword Slash Prefab or Player Transform not assigned in Abilities script.");
        }
    }

    public void Move()
    {
        Debug.Log("player moves");
        // Check if there are directions stored in the list
        if (gameplay.playerDirections.Count > 0)
        {
            // Get the earliest direction from the list
            int earliestDirection = gameplay.playerDirections[0];

            // Convert the direction to a Vector3
            Vector3 moveDirection = GetVectorFromDirection(earliestDirection);

            // Perform a raycast to check for obstacles in the movement path
            RaycastHit2D hit = Physics2D.Raycast(playerTransform.position, moveDirection, 1f, LayerMask.GetMask("obstacle")); 
            // Set player rotation based on the earliest direction
            playerTransform.eulerAngles = new Vector3(0, 0, earliestDirection);

            if (hit.collider != null)
            {
                Debug.LogWarning("Obstacle detected! Movement canceled.");
                // Remove the earliest direction from the list
                gameplay.playerDirections.RemoveAt(0);
                return; // Cancel movement if obstacle detected
            }

            // Calculate the target position for smooth movement
            Vector3 targetPosition = playerTransform.position + moveDirection;

            // Instantiate a prefab at the center of the target cell
            Instantiate(moveMarkerPrefab, targetPosition, Quaternion.identity);

            // Move the player smoothly
            StartCoroutine(MoveSmoothly(targetPosition));

            // Remove the earliest direction from the list
            gameplay.playerDirections.RemoveAt(0);

            //Debug.Log("Moved player in direction: " + moveDirection);

            // Find all game objects with the specified tag
            GameObject prefabs = GameObject.FindGameObjectWithTag("PlayerMoveMarker");
            Destroy(prefabs, 0.2f);
            
        }
        else
        {
            Debug.LogError("No directions stored in the list. Unable to determine move direction.");
        }
    }

    private IEnumerator MoveSmoothly(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = playerTransform.position;

        while (elapsedTime < transitionTime)
        {
            // Calculate the interpolation factor
            float t = elapsedTime / transitionTime;

            // Interpolate between the starting position and the target position
            Vector3 newPosition = Vector3.Lerp(startingPosition, targetPosition, t);

            // Move the player using Rigidbody2D.MovePosition to respect colliders
            playerRigidbody2D.MovePosition(newPosition);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the player reaches the exact target position
        playerRigidbody2D.MovePosition(targetPosition);
    }


    public void Defend()
    {
        Debug.Log("Executing Defend ability");
        // Add logic for Defend ability here
    }

    // Function to convert a direction in degrees to a Vector3 direction
    Vector3 GetVectorFromDirection(int direction)
    {
        // Convert direction to a Vector3
        switch (direction)
        {
            case 0:
                return Vector3.right;
            case 90:
                return Vector3.up;
            case 180:
                return Vector3.left;
            case 270:
                return Vector3.down;
            default:
                Debug.LogWarning("Invalid direction: " + direction);
                return Vector3.zero;
        }
    }


    public void Wait()
    {
        //wait 
    }
}

