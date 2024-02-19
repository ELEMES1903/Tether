using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 180f; // Adjust this value to set the rotation speed
    public int maxHealth = 6 * 2; // Each heart represents 2 health points
    public int currentHealth;

    public Image[] heartImages; // Array to store UI heart images
    public Sprite fullHeartSprite; // Sprite for a full heart
    public Sprite halfHeartSprite; // Sprite for a half heart
    public Sprite emptyHeartSprite; // Sprite for an empty heart
    
    public bool ConPhaseInProgress = false;
    void Start()
    {
        currentHealth = maxHealth;

        // Initialize UI hearts
        UpdateHeartsUI();
    }
    void Update()
    {
        if (ConPhaseInProgress == false){
        // Get input from the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the angle based on the input
        float targetAngle = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;

        // Snap the angle to the nearest multiple of 90 degrees
        targetAngle = SnapToNearestAngle(targetAngle);

        // Rotate the player instantly towards the snapped target angle when a key is pressed
        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            transform.eulerAngles = new Vector3(0, 0, targetAngle);
        }
        }
    }

    float SnapToNearestAngle(float angle)
    {
        // Snap the angle to the nearest multiple of 90 degrees
        float snappedAngle = Mathf.Round(angle / 90) * 90;
        return snappedAngle;
    }

 void UpdateHeartsUI()
    {
        int fullHearts = currentHealth / 2;
        int halfHeart = currentHealth % 2;

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < fullHearts)
            {
                heartImages[i].sprite = fullHeartSprite;
            }
            else if (i == fullHearts && halfHeart > 0)
            {
                heartImages[i].sprite = halfHeartSprite;
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite;
            }
        }
    }

    // Example function to handle damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Ensure health doesn't go below 0
        currentHealth = Mathf.Max(currentHealth, 0);

        // Update UI hearts
        UpdateHeartsUI();

        // Check if the player is defeated (health reaches 0)
        if (currentHealth == 0)
        {
            Defeat();
        }
    }

    void Defeat()
    {
        // Add logic for player defeat (e.g., game over screen, respawn, etc.)
        Debug.Log("Player defeated!");
    }

    // Example function to handle healing
    public void Heal(int healing)
    {
        currentHealth += healing;

        // Ensure health doesn't exceed the maximum
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        // Update UI hearts
        UpdateHeartsUI();
    }

    // Add more player control logic as needed
}




