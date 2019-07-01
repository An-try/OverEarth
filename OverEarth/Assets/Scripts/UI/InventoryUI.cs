using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private InventorySlot[] inventorySlots;

    private Inventory inventory;

    private void Start()
    {
        // Delegate calls when player ship is assigned
        Manager.instance.onPlayerShipAssigned += SetInventory;
    }

    private void SetInventory()
    {
        inventory = Inventory.instance; // Get instance of the inventory

        inventory.onItemChanged += UpdateUI; // When any item changed in inventory

        inventorySlots = GetComponentsInChildren<InventorySlot>(); // Get all children game objects in this game object as slots
    }

    private void UpdateUI()
    {
        // Move all the items in the inventory to the beginning
        for (int i = 0; i < inventorySlots.Length; i++) // Check all inventory slots
        {
            // Each item in inventory add to slots from the beginning and delete from slots removed item
            // For example: player has 8 slots and 5 items in the inventory.
            // Inventory -> 4 6 8 1 7
            // Slots     -> 4 6 8 1 7 - - -
            // Then he deletes the third item fron the inventory(number 8)
            // Inventory -> 4 6 1 7
            // Slots     -> 4 6 - 1 7 - - -
            // Then each item in the inventory readded again to the slots. And the last item deletes from the slots
            // Inventory -> 4 6 1 7
            //              ↓ ↓ ↓ ↓
            // Slots     -> 4 6 1 7 - - - -
            if (i < inventory.inventoryItems.Count)
            {
                inventorySlots[i].AddItemToSlot(inventory.inventoryItems[i]);
            }
            else
            {
                inventorySlots[i].DeleteItem();
            }
        }
    }
}
