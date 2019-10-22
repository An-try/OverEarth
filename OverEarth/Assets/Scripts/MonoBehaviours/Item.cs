using UnityEngine;

namespace OverEarth
{
    /// <summary>
    /// Class for any item.
    /// </summary>
    [CreateAssetMenu(fileName = "New item", menuName = "OverEarth/Inventory/Item")]
    public class Item : ScriptableObject, IItem
    {
        public string Name = "New item";
        public Sprite Image = null;
        public GameObject ObjectPrefab = null;

        public Sprite GetImage()
        {
            return Image;
        }
    }
}
