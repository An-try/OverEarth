using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    // TODO: Equipment

    public static EquipmentManager instance; // Singleton for this script

    // Delegate calls after replacing equipment
    public delegate void OnEquipmentChanged(Equipment newEquipment, Equipment oldEquipment);
    public OnEquipmentChanged onEquipmentChanged;

    private Inventory inventory;

    private Equipment[] currentEquipment;

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
        // Delegate called when player ship spawned
        Manager.instance.onPlayerShipAssigned += SetInventory;
    }

    private void SetInventory()
    {
        inventory = Inventory.instance; // Get instance of inventory

        int slotsAmount = System.Enum.GetNames(typeof(EquipmentSlot)).Length; // Get equipment slots amount
        currentEquipment = new Equipment[slotsAmount]; // Create new equipment
    }

    public void Equip(Equipment equipment) // Put on equipment
    {
        int slotIndex = (int)equipment.equipmentSlot; // Get slot index in current equipment based on new equipment type

        Equipment oldEquipment = null;

        if (currentEquipment[slotIndex] != null) // If there is any equipment in the equipment slot, where you need to equip a new item
        {
            oldEquipment = currentEquipment[slotIndex]; // Set old equipment that needs to be replaced
            inventory.AddItem(oldEquipment); // Add the equipment that needs to be changed to inventory
        }

        currentEquipment[slotIndex] = equipment; // Set the new equipment that needs to be equipped

        onEquipmentChanged?.Invoke(equipment, oldEquipment); // Call a delegate
    }

    public void UnequipEquipment(int slotIndex)
    {
        if(currentEquipment[slotIndex] != null) // If equipment that needs to be removed exists
        {
            Equipment oldEquipment = currentEquipment[slotIndex];
            inventory.AddItem(oldEquipment); // Add this equipment to inventory
            currentEquipment[slotIndex] = null; // Remove this equipment

            onEquipmentChanged?.Invoke(null, oldEquipment); // Call a delegate
        }
    }

    public void UnequipAllEquipment()
    {
        for (int i = 0; i < currentEquipment.Length; i++) // Check all equipment
        {
            UnequipEquipment(i); // Unequip each equipment
        }
    }
}
