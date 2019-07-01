using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance; // Singleton for this script

    // Delegate calls when any item added or removed from the inventory
    public delegate void OnItemChanged();
    public OnItemChanged onItemChanged;

    public int inventoryMaximumSpace = 16;

    public List<Item> inventoryItems = new List<Item>();

    private Manager manager;

    private void Awake() // Awake is called when the script instance is being loaded
    {
        if (instance == null) // If instance not exist
        {
            instance = this; // Set up instance as this script
        }
        else //If instance already exists
        {
            Destroy(this); // Destroy this script
        }
    }

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        manager = Manager.instance;
    }

    public void InventoryPanelExecute()
    {
        manager.ActivateDeactivateUIPanel(manager.InventoryPanel); // Activate or deactivate inventory panel

        Cursor.visible = manager.InventoryPanel.activeSelf; // Enable or disable cursor based on inventory panel

        // Enable or disable current target info camera based on inventory panel
        manager.CurrentTargetInfoCamera.SetActive(!manager.InventoryPanel.activeSelf);
    }

    public bool AddItem(Item item)
    {
        // If the item that needs to be added to inventory is default item and there is free space in inventory
        if (!item.isDefaultItem && inventoryItems.Count < inventoryMaximumSpace)
        {
            inventoryItems.Add(item); // Add item to inventory
            onItemChanged?.Invoke();
            return true; // Item added
        }
        return false; // Item can't be added
    }

    public void RemoveItem(Item item)
    {
        inventoryItems.Remove(item); // Remove item from inventory
        onItemChanged?.Invoke();
    }
}
