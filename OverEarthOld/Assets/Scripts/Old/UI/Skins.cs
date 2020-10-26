using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OverEarth
{
    public class Skins : MonoBehaviour
    {
        public GameObject SkinSlotPrefab;

        public List<Material> materials = new List<Material>(); // Materials for ship skin. User can set any material to the ship

        private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
        {
            // Instantiate skin slots in the player skins UI
            for (int i = 0; i < materials.Count; i++)
            {
                // Create an inventory slot and parent it to this game object
                GameObject inventorySlot = Instantiate(SkinSlotPrefab, gameObject.transform);
                inventorySlot.GetComponent<SkinSlot>().skin = materials[i]; // Set a material to skin slot script
                inventorySlot.GetComponent<Image>().material = materials[i]; // Set a material to skin slot image component
            }
        }
    }
}
