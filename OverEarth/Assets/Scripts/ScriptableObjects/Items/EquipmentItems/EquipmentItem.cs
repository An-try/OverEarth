using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Class for any equipment.
    /// </summary>
    public abstract class EquipmentItem : Item
    {
        [SerializeField] private int _durabilityModifier = 0;
        [SerializeField] private int _durabilityRegen = 0;
        [SerializeField] private int _armorModifier = 0;
        [SerializeField] private int _armorRegen = 0;
        [SerializeField] private int _shieldModifier = 0;
        [SerializeField] private int _shieldRegen = 0;

        [SerializeField] private EquipmentTypes _equipmentType = EquipmentTypes.Upgrade;
        
        public int DurabilityModifier => _durabilityModifier;
        public int DurabilityRegen => _durabilityRegen;
        public int ArmorModifier => _armorModifier;
        public int ArmorRegen => _armorRegen;
        public int ShieldModifier => _shieldModifier;
        public int ShieldRegen => _shieldRegen;

        public override bool IsEquipment => true;

        public EquipmentTypes _thisEquipmentType => _equipmentType;
    }
}
