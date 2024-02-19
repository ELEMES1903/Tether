using UnityEngine;
using UnityEngine.UI;

public class ActionEconomy : MonoBehaviour
{
    public int maxPoints = 3;
    public int currentPoints;

    public Image image1;
    public Image image2;
    public Image image3;
    public Image freeMoveImage;  // Add this line to reference the FreeMove image
    public Sprite actionIconFreeMove;
    public Sprite actionIconFreeMoveUnused;

public Sprite actionIconMove;
    public TimerController timerController;
    

    public Sprite actionIconFrame; // Reference to the "Action Icon Frame" sprite

    void Start()
    {
        // Initialize current points to max points
        currentPoints = maxPoints;
    }

    void Update()
    {
        // Check if the timer has reached totalTime in the TimerController script
        if (timerController != null && timerController.currentTime >= timerController.totalTime)
        {
            // Set current points to max points
            currentPoints = maxPoints;

        }
    }

    // Function to change the current points and update the UI
    public void ChangePoints(int delta)
    {
        currentPoints += delta;

        // Ensure current points don't go below 0 or above max points
        currentPoints = Mathf.Clamp(currentPoints, 0, maxPoints);
    }

    public void ClearImages()
    {
        SetImageSource(image1, actionIconFrame);
        SetImageSource(image2, actionIconFrame);
        SetImageSource(image3, actionIconFrame);
        freeMoveImage.sprite = actionIconFreeMoveUnused;
    }

    // Function to set the lowest empty image with the specified sprite
    public void SetLowestImage(Sprite sprite)
    {
        if (image1 != null && image1.sprite != null && image1.sprite.name == "Action Icon Frame")
        {
            SetImageSource(image1, sprite);
        }
        else if (image2 != null && image2.sprite != null && image2.sprite.name == "Action Icon Frame")
        {
            SetImageSource(image2, sprite);
        }
        else if (image3 != null && image3.sprite != null && image3.sprite.name == "Action Icon Frame")
        {
            SetImageSource(image3, sprite);
        }
    }

public void SetHighestImage()
    {
        if (image3.sprite.name != "Action Icon Frame")
        {
            SetImageSource(image3, actionIconFrame);
        }
        else if (image2.sprite.name != "Action Icon Frame")
        {
            SetImageSource(image2, actionIconFrame);
        }
        else if (image1.sprite.name != "Action Icon Frame")
        {
            SetImageSource(image1, actionIconFrame);
        }
    }
    // Function to set the source image of an image component
    private void SetImageSource(Image image, Sprite sprite)
    {
        if (image != null)
        {
            image.sprite = sprite;
        }
    }

}
