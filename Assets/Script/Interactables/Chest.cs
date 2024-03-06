using UnityEngine;

public class Chest : MonoBehaviour
{
    public float interactRadius = 2f; // Radius within which the player can interact with the chest
    public KeyCode interactKey = KeyCode.E; // Key to interact with the chest
    public LayerMask playerLayer; // Layer mask for detecting the player
    public GameObject[] lootPrefabs; // Array of loot prefabs to spawn

    void Update()
    {
        // Check if the player is in range and presses the interact key
        if (Input.GetKeyDown(interactKey))
        {
            Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, interactRadius, playerLayer);
            if (playerCollider != null)
            {
                // Destroy the chest
                Destroy(gameObject);

                // Spawn a random loot prefab from the list at the chest's position
                if (lootPrefabs.Length > 0)
                {
                    int randomIndex = Random.Range(0, lootPrefabs.Length);
                    Instantiate(lootPrefabs[randomIndex], transform.position, Quaternion.identity);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the interaction radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
