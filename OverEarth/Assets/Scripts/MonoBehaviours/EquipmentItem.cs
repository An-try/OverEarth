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
        public EquipmentSlots equipmentSlot;

        public int HealthModifier;
        public int HealthRegen;

        public int ArmorModifier;
        public int ArmorRegen;

        public int ShieldModifier;
        public int ShieldRegen;
    }
}
