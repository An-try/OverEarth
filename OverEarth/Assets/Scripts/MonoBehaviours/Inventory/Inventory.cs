using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    [RequireComponent(typeof(Damageable), typeof(Equipment))]
    public class Inventory : MonoBehaviour
    {
        // Delegate calls when any item added or removed from the inventory
        public delegate void OnItemChanged();
        public OnItemChanged onItemChanged;

        private Damageable _damageable;
        private Equipment _equipment;

        private int _inventoryMaximumSpace = 50;

        private List<InventorySlotUI> _inventorySlotsUI;

        private void Awake()
        {
            _damageable = GetComponent<Damageable>();
            _equipment = GetComponent<Equipment>();
        }

        private void Start()
        {
            _inventorySlotsUI = MenuPanelUIController.Instance.InventorySlotsUI;

            _inventorySlotsUI[0].SetItem(ItemsContainer.Instance.Items[0]);
            _inventorySlotsUI[1].SetItem(ItemsContainer.Instance.Items[0]);
            _inventorySlotsUI[2].SetItem(ItemsContainer.Instance.Items[0]);

            _inventorySlotsUI[3].SetItem(ItemsContainer.Instance.Items[1]);
            _inventorySlotsUI[4].SetItem(ItemsContainer.Instance.Items[1]);
            _inventorySlotsUI[5].SetItem(ItemsContainer.Instance.Items[1]);
            _inventorySlotsUI[6].SetItem(ItemsContainer.Instance.Items[1]);

            _inventorySlotsUI[7].SetItem(ItemsContainer.Instance.Items[2]);
            _inventorySlotsUI[8].SetItem(ItemsContainer.Instance.Items[2]);
            _inventorySlotsUI[9].SetItem(ItemsContainer.Instance.Items[2]);
            _inventorySlotsUI[10].SetItem(ItemsContainer.Instance.Items[2]);
        }

        //public bool AddItem(Item item)
        //{
        //    // If the item that needs to be added to inventory is default item and there is free space in inventory
        //    if (_inventorySlots.Count < _inventoryMaximumSpace)
        //    {
        //        _inventorySlots.Add(item); // Add item to inventory
        //        onItemChanged?.Invoke();
        //        return true; // Item added
        //    }
        //    return false; // Item can't be added
        //}

        //public bool RemoveItem(Item item)
        //{
        //    if (_inventorySlots.Contains(item))
        //    {
        //        _inventorySlots.Remove(item); // Remove item from inventory
        //        onItemChanged?.Invoke();
        //        return true;
        //    }
        //    return false;
        //}
    }
}
