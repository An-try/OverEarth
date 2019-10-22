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

        private List<IItem> _inventoryItems = new List<IItem>();

        private void Awake()
        {
            _damageable = GetComponent<Damageable>();
            _equipment = GetComponent<Equipment>();
        }

        public bool AddItem(IItem item)
        {
            // If the item that needs to be added to inventory is default item and there is free space in inventory
            if (_inventoryItems.Count < _inventoryMaximumSpace)
            {
                _inventoryItems.Add(item); // Add item to inventory
                onItemChanged?.Invoke();
                return true; // Item added
            }
            return false; // Item can't be added
        }

        public bool RemoveItem(IItem item)
        {
            if (_inventoryItems.Contains(item))
            {
                _inventoryItems.Remove(item); // Remove item from inventory
                onItemChanged?.Invoke();
                return true;
            }
            return false;
        }
    }
}
