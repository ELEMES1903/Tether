using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public class InventorySlot
{
    public string name;
    public Button button;
    public string itemName;
    public Image itemImage;

}

    public class InventoryManager : MonoBehaviour
    {
        public Gameplay gameplay; // Reference to the Gameplay script
        public ActionEconomy actionEconomy;
        public InventorySlot[] inventorySlots;

        // Method to update UI buttons in the Gameplay script
        public void UpdateGameplayUIButtons()
        {
            foreach (var slot in inventorySlots)
            {
                // Find the corresponding element in the Gameplay script
                var element = Array.Find(gameplay.elements, e => e.name == slot.itemName);
                if (element != null)
                {
                    if(slot.name == "1")
                    {
                        element.uiButtons[0] = slot.button;
                    } 
                    else if (slot.name == "2")
                    {
                        element.uiButtons[1] = slot.button;
                    } 
                    else
                    {
                        element.uiButtons[2] = slot.button;
                    }
                }
            }
            gameplay.AddListenerToButton();
        }


    // Update is called when a collision occurs
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            // Find the earliest empty inventory slot
            InventorySlot emptySlot = null;
            foreach (InventorySlot slot in inventorySlots)
            {
                if (string.IsNullOrEmpty(slot.itemName))
                {
                    emptySlot = slot;
                    break;
                }
            }

            if (emptySlot != null)
            {
                // Set the item name and image of the empty slot based on the collided item
                Sprite itemSprite = other.GetComponent<SpriteRenderer>().sprite;
                emptySlot.itemName = itemSprite.name;
                emptySlot.itemImage.sprite = itemSprite;

                // Disable the collided item to prevent it from being picked up again
                other.gameObject.SetActive(false);

                Debug.Log("Item picked up: " + itemSprite.name);

                // After picking up an item, call the method to update UI buttons
                UpdateGameplayUIButtons();
            }
            else
            {
                Debug.LogWarning("Inventory is full!");
            }
        }
    }
}
