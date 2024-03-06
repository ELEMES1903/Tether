using UnityEngine;
using System.Collections;

public class Playermove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float tileSize = 1f; // Size of each tile
    public  Vector3 targetPosition; // Target position for movement
    public bool isMoving = false; // Flag to track if the player is currently moving

    public TimerController timerController;
    public CamTrigger camTrigger;

    // Update is called once per frame
    void Update()
    {
        // Check if the player is not already moving
        if (!isMoving)
        {
            // Check for horizontal or vertical input
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Check if there is any horizontal or vertical input (ignore diagonal)
            if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(verticalInput) == 0)
            {
                // Move horizontally
                MovePlayer(new Vector3(horizontalInput, 0f, 0f));
            }
            else if (Mathf.Abs(verticalInput) > 0 && Mathf.Abs(horizontalInput) == 0)
            {
                // Move vertically
                MovePlayer(new Vector3(0f, verticalInput, 0f));
            }
        }
    }

    // Function to move the player in the specified direction
    public void MovePlayer(Vector3 direction)
    {
        // Rotate player based on movement direction
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);

        // Calculate target position for movement
        targetPosition = transform.position + direction * tileSize;

        // Perform a raycast to check for obstacles in the movement path
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, tileSize, LayerMask.GetMask("obstacle")|LayerMask.GetMask("transitionArea"));

        // Check if the raycast hit an obstacle
        if (hit.collider != null)
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("obstacle"))
            {
                //Debug.LogWarning("Obstacle detected! Movement canceled.");
                return; // Cancel movement if obstacle detected
            } else 
            {
                //find the camTrigger script from the object we hit
                var TriggerScript = hit.collider.gameObject.GetComponent<CamTrigger>();
                camTrigger = TriggerScript;

                //Transition camera to next Room and additionally move to compensate
                targetPosition = transform.position + direction * 2f;
                camTrigger.CameraTransition();
            }
            
        }

        // Start moving towards the target position
        StartCoroutine(MoveToTarget(targetPosition));
    }

    // Coroutine to smoothly move the player to the target position
    public IEnumerator MoveToTarget(Vector3 target)
    {
        if(timerController.inCombat == false){

            isMoving = true; // Set flag to indicate movement
            Vector3 startPosition = transform.position;
            float distance = Vector3.Distance(startPosition, target);
            float duration = distance / moveSpeed;
            float startTime = Time.time;

            while (Time.time < startTime + duration)
            {
                float fraction = (Time.time - startTime) / duration;
                transform.position = Vector3.Lerp(startPosition, target, fraction);
                yield return null;
            }

            transform.position = target; // Ensure final position is accurate
            isMoving = false; // Reset flag
            
        }
    }
}