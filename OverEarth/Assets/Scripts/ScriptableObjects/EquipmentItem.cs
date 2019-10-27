using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Class for any equipment.
    /// </summary>
    [CreateAssetMenu(fileName = "New equipment", menuName = "OverEarth/Inventory/Equipment")]
    public class EquipmentItem : Item
    {
        [SerializeField] private EquipmentTypes _equipmentType = EquipmentTypes.Upgrade;

        [SerializeField] private int HealthModifier = 0;
        [SerializeField] private int HealthRegen = 0;

        [SerializeField] private int ArmorModifier = 0;
        [SerializeField] private int ArmorRegen = 0;

        [SerializeField] private int ShieldModifier = 0;
        [SerializeField] private int ShieldRegen = 0;

        private protected override EquipmentTypes _thisEquipmentType => _equipmentType;
    }
}
