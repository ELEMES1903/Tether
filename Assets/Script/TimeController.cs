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

    public bool consequencePhaseOver = false; // Added bool to control timer reset

    void Start()
    {
        currentTime = totalTime;
        UpdateUI();
    }

    void Update()
    {
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
    }

    void UpdateUI()
    {
        // Update the UI elements
        if (timerSlider != null)
            timerSlider.value = 1 - (currentTime / totalTime);

        int roundedTime = Mathf.RoundToInt(currentTime);
        timerText.text = roundedTime.ToString();
    }

    // Function to clear all images

}





