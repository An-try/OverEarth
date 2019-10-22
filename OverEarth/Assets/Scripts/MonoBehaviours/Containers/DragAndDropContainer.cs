using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class DragAndDropContainer : Singleton<DragAndDropContainer>
    {
        [SerializeField] private SpriteRenderer _dragAndDropItem;

        [SerializeField] private EquipmentItem _itemInContainer;

        private Vector3 _senderPosition;

        private void Update()
        {
            if (_itemInContainer != null)
            {
                _dragAndDropItem.transform.position =
                    CameraController.Instance.AimRay.direction * Vector3.Distance(CameraController.Instance.transform.position, _senderPosition);
            }
        }

        public void AddItemToContainer(EquipmentItem item, Transform sender)
        {
            if (_itemInContainer != null)
            {
                Debug.LogError("There is already an item in Drag And Drop Container!");
            }

            _itemInContainer = item;
            _dragAndDropItem.sprite = item.GetImage();

            _senderPosition = sender.position;
        }

        public void RemoveItemFromContainer()
        {
            _itemInContainer = null;
            _dragAndDropItem.sprite = null;
        }
    }
}
