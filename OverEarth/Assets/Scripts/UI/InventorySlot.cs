using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for any inventory slot
/// </summary>
public class InventorySlot : MonoBehaviour
{
    public Image icon; // Icon to display what item is in this slot

    private Item item; // Item that stores in this slot

    private void Start()
    {
        icon = GetComponent<Image>(); // Get image in this game object
    }

    private void OnMouseUp() // When click and releasing left mouse button
    {
        EquipItem(); // Equip item that stores in this slot
    }

    public void AddItemToSlot(Item item)
    {
        this.item = item; // Set item to this slot
        icon.sprite = item.icon; // Set item icon in this slot
    }

    public void DeleteItem()
    {
        item = null; // Delete item in this slot
        icon.sprite = null; // Delete icon from this slot
    }

    public void EquipItem()
    {
        if (item) // If item in this slot exists
        {
            item.EquipItem(); // Equip this item
        }
    }
}
