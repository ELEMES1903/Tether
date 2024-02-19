using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public int damageAmount = 1;

    // Function to handle collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the "enemy" tag
        if (other.CompareTag("Enemy"))
        {
            // Try to get the EnemyAI component on the collided object
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();

            // If EnemyAI component is found, apply damage
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damageAmount);
            }

            // Destroy the sword slash prefab
            Destroy(gameObject, 1.5f);
        }   
    }
}
