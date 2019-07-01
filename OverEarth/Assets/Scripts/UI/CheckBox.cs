using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBox : MonoBehaviour
{
    public Sprite checkMark;

    private void Start()
    {
        SetCheckMark();
        GetComponent<Image>().sprite = checkMark;
    }

    private void OnMouseUp()
    {
        SetCheckMark();
    }

    private void SetCheckMark()
    {
        if (!GetComponent<Toggle>().isOn)
        {
            GetComponent<Image>().sprite = checkMark;
        }
        else
        {
            GetComponent<Image>().sprite = null;
        }
    }
}