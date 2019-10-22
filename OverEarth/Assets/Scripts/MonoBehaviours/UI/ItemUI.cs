using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OverEarth
{
    public class ItemUI : MonoBehaviour
    {
        private Item _item;
        private Image _image;

        private void Start()
        {
            if (_item)
            {
                _image.sprite = _item.Image;
            }
        }
    }
}
