using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addgun : MonoBehaviour
{
    public int id;

    public List<Item> items = new List<Item>();

    void OnMouseUp()
    {
        if (id == 0)
        {
            //if (!Inventory.instance.items.Contains(items[0]))
                Inventory.instance.AddItem(items[0]);
            //else
                //Inventory.instance.RemoveItem(items[0]);
        }

        if (id == 1)
        {
            //if (!Inventory.instance.items.Contains(items[1]))
                Inventory.instance.AddItem(items[1]);
            //else
                //Inventory.instance.RemoveItem(items[1]);
        }
    }
}
