using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class ItemsContainer : Singleton<ItemsContainer>
    {
        public List<Item> Items;
        public List<EquipmentItem> EquipmentItems;
    }
}
