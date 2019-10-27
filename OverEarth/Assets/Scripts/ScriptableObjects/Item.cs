using System;
using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Class for any item.
    /// </summary>
    [CreateAssetMenu(fileName = "New item", menuName = "OverEarth/Inventory/Item")]
    public class Item : ScriptableObject
    {
        public string Name = "New item";
        public Sprite Image = null;
        public GameObject ObjectPrefab = null;

        private protected virtual EquipmentTypes _thisEquipmentType => (EquipmentTypes)999; // Get enum number of 999 (it is out of range, so this object is not equipment)

        public bool IsEquipment() // (out EquipmentTypes equipmentType)
        {
            //equipmentType = _thisEquipmentType;
            if (Enum.IsDefined(typeof(EquipmentTypes), _thisEquipmentType))
            {
                return true;
            }
            return false;
        }
    }
}
