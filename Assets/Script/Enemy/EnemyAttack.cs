using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageAmount = 6;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Collision detected!");
        // Check if the collided object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player detected!");
            // Get the PlayerController script from the collided object
            PlayerController playerController = other.GetComponent<PlayerController>();

            // Check if the PlayerController script is found
            if (playerController != null)
            {
                // Deal damage to the player
                playerController.TakeDamage(damageAmount);
            }

            // Destroy the sword slash when it hits the player
            Destroy(gameObject);
        }
    }
}
