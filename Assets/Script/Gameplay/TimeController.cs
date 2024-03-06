using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{


    public float totalTime = 6f; // Total time for the timer
    public float currentTime { get; private set; } // Current time left, accessible publicly

    public ActionEconomy actionEconomy;
    public Gameplay gameplay;

    public Slider timerSlider;
    public TextMeshProUGUI timerText;

    // Reference to the player's transform
    public Transform playerTransform;

    public bool consequencePhaseOver = false; // Added bool to control timer reset
    public bool inCombat = false;
    // Radius to check for enemies
    public float checkRadius = 5f;

    void Start()
    {
        currentTime = totalTime;
        UpdateUI();
    }

    void Update()
    {
        CheckForEnemies();

        if(inCombat == true){

            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                UpdateUI();
            }
            else
            {
                if (consequencePhaseOver)
                {
                    // Reset the timer only when consequencePhaseOver is true
                    currentTime = totalTime;
                    actionEconomy.ClearImages();
                    consequencePhaseOver = false; // Reset the bool
                    UpdateUI();
                }
            }
        } else 
        {
            currentTime = totalTime;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        // Update the UI elements
        if (timerSlider != null)
            timerSlider.value = 1 - (currentTime / totalTime);

        int roundedTime = Mathf.RoundToInt(currentTime);
        timerText.text = roundedTime.ToString();
    }

    // Function to check for enemies within the specified radius
void CheckForEnemies()
{

    // Find all colliders within the check radius around the player
    Collider2D[] colliders = Physics2D.OverlapCircleAll(playerTransform.position, checkRadius);

    // Check if any of the colliders belong to enemies

    foreach (Collider2D collider in colliders)
    {
        if (collider.CompareTag("Enemy"))
        {
            inCombat = true;
            break;
        } else
        {
            inCombat = false;
        }
    }

}

    public Color radiusColor = Color.red; // Color of the radius in the scene view
    private void OnDrawGizmosSelected()
    {
        // Draw wire sphere in the scene view to represent the check radius
        Gizmos.color = radiusColor;
        Gizmos.DrawWireSphere(playerTransform.position, checkRadius);
    }


}





