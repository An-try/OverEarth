using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace OverEarth
{
    public abstract class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Sprite _emptySlotImage;

        private Image _image;

        private Color _filledSlotImageColor => Color.white;
        private protected virtual Color _emptySlotImageColor { get { return Color.white; } }

        public abstract Item Item { get; }
        public abstract bool IsInventorySlot { get; }

        private void Awake()
        {
            _image = GetComponent<Image>();
            _emptySlotImage = _image.sprite;
        }

        private protected void UpdateSlotUI(Item item)
        {
            if (item)
            {
                _image.sprite = item.Image;
                _image.color = _filledSlotImageColor;
            }
            else
            {
                _image.sprite = _emptySlotImage;
                _image.color = _emptySlotImageColor;
            }
        }

        private protected abstract void RemoveItem();

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
            if (this.Item)
            {
                DragAndDropContainer.Instance.AddItemToContainer(this.Item, transform);
                RemoveItem();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragAndDropContainer.Instance.UpdateDragAndDropContainerPosition();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Item itemInContainer = DragAndDropContainer.Instance.GetItemInContainer();
            if (!itemInContainer)
            {
                return;
            }

            SlotUI slotUnderCursor = DragAndDropContainer.SlotUnderCursor;
            Item itemUnderCursor = null;
            if (slotUnderCursor && slotUnderCursor.Item)
            {
                itemUnderCursor = slotUnderCursor.Item;
            }

            if (!slotUnderCursor)
            {
                ReturnItem(this, itemInContainer);
                return;
            }

            if (!slotUnderCursor.IsInventorySlot && !itemInContainer.IsEquipment)
            {
                ReturnItem(this, itemInContainer);
                return;
            }

            if (!this.IsInventorySlot && itemUnderCursor && !itemUnderCursor.IsEquipment)
            {
                ReturnItem(this, itemInContainer);
                return;
            }

            ReplaceItems(slotUnderCursor, itemInContainer);
        }

        public abstract void SetItem(Item item);

        private void ReplaceItems(SlotUI slotUnderCursor, Item itemInContainer)
        {
            SetItem(slotUnderCursor.Item);
            slotUnderCursor.SetItem(itemInContainer);
        }

        private void ReturnItem(SlotUI slotUnderCursor, Item itemInContainer)
        {
            slotUnderCursor.SetItem(itemInContainer);
        }
    }
}
