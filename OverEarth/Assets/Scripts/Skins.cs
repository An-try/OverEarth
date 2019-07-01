using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skins : MonoBehaviour
{
    public List<Material> materials= new List<Material>();

    void Start()
    {
        for(int counter = 0; counter < materials.Count; counter++)
        {
            transform.GetChild(counter).GetComponent<SkinSlot>().skin = materials[counter];
            transform.GetChild(counter).GetComponent<Image>().material = materials[counter];
        }
    }
}