using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class EquipmentSlot : MonoBehaviour
    {
        public event Action<EquipmentItem> EquipmentChangedEvent;

        public EquipmentItem CurrentEquipmentItem { get; private set; }

        private GameObject _currentEquipmentObject;

        public void SetEquipment(EquipmentItem equipmentItem)
        {
            RemoveEquipment();

            CurrentEquipmentItem = equipmentItem;

            InstantiateEquipment();
        }

        public void RemoveEquipment()
        {
            CurrentEquipmentItem = null;

            if (_currentEquipmentObject)
            {
                Destroy(_currentEquipmentObject);
            }

            EquipmentChangedEvent?.Invoke(CurrentEquipmentItem);
        }

        private void InstantiateEquipment()
        {
            _currentEquipmentObject = Instantiate(CurrentEquipmentItem.ObjectPrefab, transform.position, transform.rotation, transform);
        }
    }
}
