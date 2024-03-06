using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Reflection;

[System.Serializable]
public class GameElement
{
    public string name;
    public int actionPointCost;
    public Sprite actionSprite;
    public Button[] uiButtons;
}

public class Gameplay : MonoBehaviour
{
    public GameElement[] elements;
    public TimerController timerController;
    public ActionEconomy actionEconomy;
    public Abilities abilities; 
    public EnemyAI enemyAI;
    public PlayerController playerController;   

    [Header("Delays")]
    public float initialDelay = 0.5f;
    public float inBetweenDelay = 0.2f;
    public float finalDelay = 1.0f;
    public Transform playerTransform; // Assuming you have a reference to the player's transform

    [Header("Clicked Elements List")]
    [SerializeField]
    public List<string> clickedElementNames = new List<string>();
    public List<int> playerDirections = new List<int>(); // New list to store player directions
    
    private bool isProcessing = false;

    public bool isFirstMove = true;

void Start()
{
    AddListenerToButton();
}

public void AddListenerToButton()
{
    foreach (GameElement element in elements)
    {
        if (element.uiButtons != null)
        {
            foreach (Button button in element.uiButtons)
            {
                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        if (timerController != null && timerController.currentTime > 0)
                        {
                            if (actionEconomy != null && actionEconomy.currentPoints >= element.actionPointCost)
                            {
                                actionEconomy.currentPoints -= element.actionPointCost;
                                clickedElementNames.Add(element.name);
                                //Debug.Log("Button Clicked for Element: " + element.name);

                                // Check if the element is "Move" and it's the first time
                                if (element.name == "Move" && isFirstMove)
                                {

                                        // Set Action Point Cost to 1 for the "Move" element
                                    GameElement moveElement = Array.Find(elements, el => el.name == "Move");
                                    if (moveElement != null)
                                    {
                                        moveElement.actionPointCost = 1;
                                    }

                                    // Skip the SetLowestImage code
                                    isFirstMove = false; // Set the flag to false after the first "Move" entry

                                    // Set the source image for FreeMove to ActionIconFreeMove
                                    if (actionEconomy != null && actionEconomy.freeMoveImage != null)
                                    {
                                        actionEconomy.freeMoveImage.sprite = actionEconomy.actionIconFreeMove;
                                    }
                                }
                                else
                                {
                                    // Update the lowest empty image with the action sprite
                                    actionEconomy.SetLowestImage(element.actionSprite);
                                }

                                int currentPlayerDirection = GetPlayerDirection();
                                playerDirections.Add(currentPlayerDirection);
                                //Debug.Log("Player Direction: " + currentPlayerDirection);

                            }
                            else
                            {
                                Debug.Log("Insufficient points for Element: " + element.name);
                            }
                        }
                        else
                        {
                            Debug.Log("Timer is not above 0 seconds. Button click ignored.");
                        }             
                    });
                }
            }
        }
    }
}




void EnsureMinimumEntries()
{
    int currentSize = clickedElementNames.Count;

    // Check if the number of entries is less than 3
    if (currentSize < 3)
    {
        // Calculate how many "wait" entries are needed
        int entriesToAdd = 3 - currentSize;

        // Add "wait" entries to meet the minimum requirement
        for (int i = 0; i < entriesToAdd; i++)
        {
            clickedElementNames.Add("Wait");
        }
    }
}

   int GetPlayerDirection()
    {
        if (playerTransform != null)
        {
            // Get the player's rotation in degrees
            float playerRotation = playerTransform.eulerAngles.z;

            // Round the rotation to the nearest 90 degrees
            int roundedRotation = Mathf.RoundToInt(playerRotation / 90) * 90;

            // Ensure the result is within the valid range (0, 90, 180, or 270)
            return Mathf.Clamp(roundedRotation, 0, 270);
        }
        else
        {
            Debug.LogWarning("Player transform not assigned in Gameplay script.");
            return 0; // Default to 0 degrees if the player transform is not assigned
        }
    }



    void Update()
    {
        // For testing: Press backspace to remove the latest entry from the list
        if (Input.GetKeyDown(KeyCode.Backspace)||(Input.GetKeyDown(KeyCode.Tab)))
        {
            RemoveLatestEntry();
        } 

        if (timerController != null && timerController.currentTime <= 0 && !isProcessing)
        {
            StartCoroutine(ProcessClickedElementNames());
        }
    }

    System.Collections.IEnumerator ProcessClickedElementNames()
    {
        playerController.ConPhaseInProgress = true;
        isProcessing = true;
        isFirstMove = true;
        EnsureMinimumEntries();
        yield return new WaitForSeconds(initialDelay);        
        List<string> namesCopy = new List<string>(clickedElementNames);

        foreach (string elementName in namesCopy)
        {

            //Debug.Log("Processing Clicked Element: " + elementName);

            // Call the function with the same name as the element from the Abilities script
            MethodInfo method = typeof(Abilities).GetMethod(elementName);
            if (method != null && abilities != null)
            {
                method.Invoke(abilities, null);
                
                // Execute MoveTowardsPlayer on all instances of the prefab with EnemyAI script
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                    if (enemyAI != null)
                    {
                        enemyAI.enemyaction();
                    }
                }
                // Start a pathfinder grid scan
                
            }
            
            AstarPath.active.Scan();
            yield return new WaitForSeconds(inBetweenDelay);
        }
        // Reset Action Point Cost to 0 for the "Move" element
            GameElement moveElement = Array.Find(elements, el => el.name == "Move");
            if (moveElement != null)
            {
                moveElement.actionPointCost = 0;
            }

        clickedElementNames.Clear();
        timerController.consequencePhaseOver = true;
        yield return new WaitForSeconds(finalDelay);

        isProcessing = false;
        playerController.ConPhaseInProgress = false;
    }

    void RemoveLatestEntry()
{
    if (clickedElementNames.Count > 0)
    {

        int lastIndex = clickedElementNames.Count - 1;
        string removedElement = clickedElementNames[lastIndex];

        clickedElementNames.RemoveAt(lastIndex);
        playerDirections.RemoveAt(playerDirections.Count - 1);
        Debug.Log("Removed latest entry: " + removedElement);

        // Find the corresponding GameElement for the removed entry
        GameElement removedGameElement = Array.Find(elements, element => element.name == removedElement);

        // Check if the removedGameElement is not null and if ActionEconomy is assigned
        if (removedGameElement != null && actionEconomy != null)
        {
            // Add the action point cost back to current points
            actionEconomy.currentPoints += removedGameElement.actionPointCost;
            Debug.Log("Added " + removedGameElement.actionPointCost + " points back to current points.");

            // Check if the removed element is "Move" and no image has "Action Icon Move"
            if (removedElement == "Move" &&
                !ImageHasActionIcon(actionEconomy.image1, "Action Icon Move") &&
                !ImageHasActionIcon(actionEconomy.image2, "Action Icon Move") &&
                !ImageHasActionIcon(actionEconomy.image3, "Action Icon Move"))
            {
                // Set the source image of freemoveImage to actionIconFreeMoveUnused
                actionEconomy.freeMoveImage.sprite = actionEconomy.actionIconFreeMoveUnused;
                isFirstMove = true;
            }
        }
        actionEconomy.SetHighestImage();

    }
    else
    {
        Debug.Log("No entries to remove.");
    }

    // Function to check if an image has a specific action icon
bool ImageHasActionIcon(Image image, string actionIconName)
{
    return image != null && image.sprite != null && image.sprite.name == actionIconName;
}
}

}
