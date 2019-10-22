using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace OverEarth
{
    public class EquipmentSlotUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Sprite _emptySlotImage;

        [HideInInspector] public EquipmentSlot Slot;

        private Image _image;

        private Color _emptySlotImageColor = Color.blue;
        private Color _filledSlotImageColor = Color.white;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _emptySlotImage = _image.sprite;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            Slot.EquipmentChangedEvent += UpdateSlotUI;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            Slot.EquipmentChangedEvent -= UpdateSlotUI;
        }

        private void UpdateSlotUI(EquipmentItem equipmentItem)
        {
            if (equipmentItem)
            {
                _image.sprite = equipmentItem.Image;
                _image.color = _filledSlotImageColor;
            }
            else
            {
                _image.sprite = _emptySlotImage;
                _image.color = _emptySlotImageColor;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _image.color = _emptySlotImageColor;
            DragAndDropContainer.Instance.AddItemToContainer(Slot.CurrentEquipmentItem, transform);
            Slot.RemoveEquipment();
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
}
