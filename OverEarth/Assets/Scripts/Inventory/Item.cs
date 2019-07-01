using UnityEngine;

/// <summary>
/// Class for any item.
/// </summary>
[CreateAssetMenu(fileName = "New item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New item";
    public Sprite icon = null;
    public GameObject ItemPrefab = null;
    public bool isDefaultItem = false;

    public virtual void EquipItem() { } // Used in equipment

    public void RemoveItemFromInventory()
    {
        Inventory.instance.RemoveItem(this);
    }
}
