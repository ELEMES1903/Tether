using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the player's movement speed in the Inspector

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
    }

    void Update()
    {
        // Get input from horizontal and vertical axes
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector based on the input
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Normalize the movement vector to ensure constant speed diagonally
        movement.Normalize();

        // Move the player using Rigidbody2D's velocity
        rb.velocity = movement * moveSpeed;
    }
}
    