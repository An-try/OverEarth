using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class EquipmentSlot : MonoBehaviour
    {
        public event Action<EquipmentItem> EquipmentChangedEvent;

        public EquipmentItem Equipment { get; private set; }

        private GameObject _currentEquipmentObject;

        public void SetEquipment(EquipmentItem equipmentItem)
        {
            RemoveEquipment();

            Equipment = equipmentItem;

            InstantiateEquipment();

            EquipmentChangedEvent?.Invoke(Equipment);
        }

        public void RemoveEquipment()
        {
            Equipment = null;

            if (_currentEquipmentObject)
            {
                Destroy(_currentEquipmentObject);
            }

            EquipmentChangedEvent?.Invoke(Equipment);
        }

        private void InstantiateEquipment()
        {
            _currentEquipmentObject = Instantiate(Equipment.ObjectPrefab, transform.position, transform.rotation, transform);
        }
    }
}
