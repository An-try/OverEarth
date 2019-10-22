using UnityEngine;

namespace OverEarth
{
    public class SkinSlot : MonoBehaviour
    {
        public Material skin;

        private void OnMouseUp() // OnMouseUp is called when the user has released the mouse button
        {
            if (skin) // If skin exists
            {
                Manager.instance.PlayerShip.GetComponent<Ship>().MainHullTexture.material = skin; // Set this skin to player's ship main hull material
            }
        }
    }
}
