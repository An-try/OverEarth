using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OverEarth
{
    //[RequireComponent(typeof(Damageable), typeof(Inventory))]
    public class Equipment : MonoBehaviour
    {
        // Delegate calls after replacing equipment
        public delegate void OnEquipmentChanged(Equipment newEquipment, Equipment oldEquipment);
        public OnEquipmentChanged onEquipmentChanged;

        //private Damageable _damageable;
        //private Inventory _inventory;

        public List<EquipmentSlot> EquipmentSlots { get; private set; }

        private void Awake() // Awake is called when the script instance is being loaded
        {
            //_damageable = GetComponent<Damageable>();
            //_inventory = GetComponent<Inventory>();

            EquipmentSlots = GetComponentsInChildren<EquipmentSlot>().ToList();
        }

        private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
        {
            Invoke("A", 1f);
        }

        void A()
        {
            EquipmentSlots[0].SetEquipment(ItemsContainer.Instance.EquipmentItems[0]);
            EquipmentSlots[1].SetEquipment(ItemsContainer.Instance.EquipmentItems[1]);
        }

        public void EquipItem(EquipmentItem equipmentItem, EquipmentSlot equipmentSlot)
        {
            //Equip(equipmentItem, equipmentSlot, equipmentSlotUI); // Equip this item
            //RemoveItemFromInventory(equipmentItem); // Remove this item from inventory
        }

        //private void SetInventory()
        //{
        //    inventory = Inventory.instance; // Get instance of inventory

        //    int slotsAmount = System.Enum.GetNames(typeof(EquipmentSlots)).Length; // Get equipment slots amount
        //    currentEquipment = new Equipment[slotsAmount]; // Create new equipment
        //}

        //private void Equip(EquipmentItem equipmentItem, EquipmentSlotUI equipmentSlotUI) // Put on equipment
        //{
        //    int slotIndex = (int)equipment.equipmentSlot; // Get slot index in current equipment based on new equipment type

        //    Equipment oldEquipment = null;

        //    if (currentEquipment[slotIndex] != null) // If there is any equipment in the equipment slot, where you need to equip a new item
        //    {
        //        oldEquipment = currentEquipment[slotIndex]; // Set old equipment that needs to be replaced
        //        inventory.AddItem(oldEquipment); // Add the equipment that needs to be changed to inventory
        //    }

        //    currentEquipment[slotIndex] = equipment; // Set the new equipment that needs to be equipped

        //    onEquipmentChanged?.Invoke(equipment, oldEquipment); // Call a delegate
        //}

        //public void UnequipEquipment(int slotIndex)
        //{
        //    if (currentEquipment[slotIndex] != null) // If equipment that needs to be removed exists
        //    {
        //        Equipment oldEquipment = currentEquipment[slotIndex];
        //        inventory.AddItem(oldEquipment); // Add this equipment to inventory
        //        currentEquipment[slotIndex] = null; // Remove this equipment

        //        onEquipmentChanged?.Invoke(null, oldEquipment); // Call a delegate
        //    }
        //}

        public void UnequipAllEquipment()
        {
            //for (int i = 0; i < currentEquipment.Length; i++) // Check all equipment
            //{
            //    //UnequipEquipment(i); // Unequip each equipment
            //}
        }
    }
}
