using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace OverEarth
{
    public class EquipmentSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private EquipmentSlot _equipmentSlot;
        [SerializeField] private Sprite _emptySlotImage;

        private Image _image;

        private Color _emptySlotImageColor = Color.blue;
        private Color _filledSlotImageColor = Color.white;

        public EquipmentItem Equipment => _equipmentSlot.Equipment;

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
            _equipmentSlot.EquipmentChangedEvent += UpdateSlotUI;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            _equipmentSlot.EquipmentChangedEvent -= UpdateSlotUI;
        }

        private void SetEquipment(EquipmentItem equipmentItem)
        {
            _equipmentSlot.SetEquipment(equipmentItem);
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            DragAndDropContainer.SlotUnderCursor = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DragAndDropContainer.SlotUnderCursor = null;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_equipmentSlot.Equipment)
            {
                _image.color = _emptySlotImageColor;
                DragAndDropContainer.Instance.AddItemToContainer(_equipmentSlot.Equipment, transform);
                _equipmentSlot.RemoveEquipment();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragAndDropContainer.Instance.UpdateDragAndDropContainerPosition();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (DragAndDropContainer.SlotUnderCursor)
            {
                SetOrReplaceEquipment();
            }
            else
            {
                SetEquipment(DragAndDropContainer.Instance.GetItemInContainer());
            }
        }

        private void SetOrReplaceEquipment()
        {
            EquipmentSlotUI slotUnderCursor = DragAndDropContainer.SlotUnderCursor;

            if (slotUnderCursor == this)
            {
                SetEquipment(DragAndDropContainer.Instance.GetItemInContainer());
                return;
            }

            if (!slotUnderCursor.Equipment)
            {
                // Set equipment.
                slotUnderCursor.SetEquipment(DragAndDropContainer.Instance.GetItemInContainer());
            }
            else
            {
                // Replace equipment.
                EquipmentItem equipmentItem = Equipment;
                SetEquipment(slotUnderCursor.Equipment);
                slotUnderCursor.SetEquipment(equipmentItem);
            }
        }
    }
}
