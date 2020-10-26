using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Class for any item.
    /// </summary>
    public abstract class Item : ScriptableObject
    {
        [SerializeField] private string _name = "New item";
        [SerializeField] private string _description = "Item description";
        [SerializeField] private Sprite _image = null;
        [SerializeField] private GameObject _objectPrefab = null;
        
        public string Name => _name;
        public Sprite Image => _image;
        public GameObject ObjectPrefab => _objectPrefab;

        public virtual bool IsTurret => false;
        public virtual bool IsEquipment => false;
    }
}
