using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class InventorySlotUI : SlotUI
    {
        private Item _item;
        
        public override Item Item => _item;
        public override bool IsInventorySlot => true;

        public override void SetItem(Item item)
        {
            _item = item;
            UpdateSlotUI(item);
        }

        private protected override void RemoveItem()
        {
            _item = null;
            UpdateSlotUI(_item);
        }
    }
}
