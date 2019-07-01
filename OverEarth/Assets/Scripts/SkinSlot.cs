using UnityEngine;

public class SkinSlot : MonoBehaviour
{
    public Material skin;

    private void OnMouseUp()
    {
        if (skin != null)
        {
            Manager.instance.PlayerShip.GetComponent<Ship>().MainHullTexture.material = skin;
        }
    }
}
