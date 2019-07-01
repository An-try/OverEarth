using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private InventorySlot[] inventorySlots;

    private Inventory inventory;

    private void Start()
    {
        Manager.instance.onPlayerShipAssigned += SetInventory;
    }

    private void SetInventory()
    {
        inventory = Inventory.instance;
        inventory.onItemChanged += UpdateUI; // When any item changed in inventory

        inventorySlots = GetComponentsInChildren<InventorySlot>(); // Get all children game objects in this game object as slots
    }

    private void UpdateUI()
    {
        for(int i = 0; i < inventorySlots.Length; i++) // Check all inventory slots
        {
            if(i < inventory.inventoryItems.Count)
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