using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Class for any equipment in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "New equipment", menuName = "OverEarth/Inventory/EquipmentItem")]
    public class EquipmentItem : Item
    {
        public EquipmentSlots EquipmentSlot = EquipmentSlots.Upgrade;

        public int HealthModifier = 0;
        public int HealthRegen = 0;

        public int ArmorModifier = 0;
        public int ArmorRegen = 0;

        public int ShieldModifier = 0;
        public int ShieldRegen = 0;
    }
}
