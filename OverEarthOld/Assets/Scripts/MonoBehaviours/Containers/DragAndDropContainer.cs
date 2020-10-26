using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OverEarth
{
    public class DragAndDropContainer : Singleton<DragAndDropContainer>
    {
        public static SlotUI SlotUnderCursor;

        [SerializeField] private Image _dragAndDropItem;

        private Item _itemInContainer;
        private Vector3 _senderPosition;

        public void UpdateDragAndDropContainerPosition()
        {
            if (_itemInContainer != null)
            {
                _dragAndDropItem.transform.position = Input.mousePosition;
            }
        }

        public void AddItemToContainer(Item item, Transform sender)
        {
            if (_itemInContainer != null)
            {
                Debug.LogError("There is already an item in Drag And Drop Container!");
            }

            _itemInContainer = item;
            _dragAndDropItem.sprite = item.Image;
            _dragAndDropItem.enabled = true;

            _senderPosition = sender.position;
        }

        public void RemoveItemFromContainer()
        {
            _itemInContainer = null;
            _dragAndDropItem.enabled = false;
            _dragAndDropItem.sprite = null;
        }

        public Item GetItemInContainer()
        {
            Item itemInContainer = _itemInContainer;
            RemoveItemFromContainer();
            return itemInContainer;
        }
    }
}
