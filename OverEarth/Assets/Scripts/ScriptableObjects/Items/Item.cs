using System;
using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Class for any item.
    /// </summary>
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private string _name = "New item";
        [SerializeField] private Sprite _image = null;
        [SerializeField] private GameObject _objectPrefab = null;
        
        public string Name => _name;
        public Sprite Image => _image;
        public GameObject ObjectPrefab => _objectPrefab;
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
