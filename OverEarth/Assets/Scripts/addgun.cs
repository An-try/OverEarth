using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a temporary script that allows to add laser beam turret or plasma turret to the inventory.
/// </summary>
public class addgun : MonoBehaviour
{
    public int id;

    public List<Item> items = new List<Item>();

    void OnMouseUp()
    {
        if (id == 0)
        {
            Inventory.instance.AddItem(items[0]);
        }

        if (id == 1)
        {
            Inventory.instance.AddItem(items[1]);
        }
    }
}
