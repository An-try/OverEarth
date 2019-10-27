using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace OverEarth
{
    public class EquipmentSlotUI : SlotUI
    {
        private EquipmentSlot _equipmentSlot;

        private protected override Color _emptySlotImageColor => Color.blue;

        public override Item Item => _equipmentSlot.Equipment;
        public override bool IsInventorySlot => false;

        private void SubscribeEvents()
        {
            _equipmentSlot.EquipmentChangedEvent += UpdateSlotUI;
        }

        private void UnsubscribeEvents()
        {
            _equipmentSlot.EquipmentChangedEvent -= UpdateSlotUI;
        }

        public void AssignEquipmentSlot(EquipmentSlot equipmentSlot)
        {
            _equipmentSlot = equipmentSlot;
            SubscribeEvents();
        }

        public override void SetItem(Item item)
        {
            _equipmentSlot.SetEquipment(item);
            UpdateSlotUI(item);
        }

        private protected override void RemoveItem()
        {
            if (Item.IsEquipment())
            {
                _equipmentSlot.RemoveEquipment();
            }
        }

        private void OnDestroy()
        {
            if (_equipmentSlot)
            {
                UnsubscribeEvents();
            }
        }
    }
}
