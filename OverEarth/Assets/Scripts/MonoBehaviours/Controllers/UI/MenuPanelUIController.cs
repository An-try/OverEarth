using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class MenuPanelUIController : PanelUI<MenuPanelUIController>
    {
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private Transform _equipmentPanel;
        [SerializeField] private Equipment _equipment;
        [SerializeField] private Transform _inventorySlotsContainer;

        public static event Action<bool> MenuInteractedEvent;

        public bool IsMenuOpened { get; private set; } = false;

        private List<EquipmentSlotUI> _equipmentSlotsUI;
        public List<InventorySlotUI> InventorySlotsUI { get; private set; }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerController.MenuButtonPressedEvent += InteractWithMenuPanel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            PlayerController.MenuButtonPressedEvent -= InteractWithMenuPanel;
        }

        protected override void Awake()
        {
            base.Awake();

            _equipmentSlotsUI = _equipmentPanel.GetComponentsInChildren<EquipmentSlotUI>().ToList();
            InventorySlotsUI = _inventorySlotsContainer.GetComponentsInChildren<InventorySlotUI>().ToList();
        }

        private void Start()
        {
            AssignEquipmentSlotsToEquipmentSlotsUI();
        }

        private void AssignEquipmentSlotsToEquipmentSlotsUI()
        {
            List<EquipmentSlot> equipmentSlots = PlayerController.Instance.Ship.Equipment.GetComponentsInChildren<EquipmentSlot>().ToList();

            for (int i = 0; i < equipmentSlots.Count; i++)
            {
                _equipmentSlotsUI[i].AssignEquipmentSlot(equipmentSlots[i]);
            }
        }

        private void UpdateInventoryUI()
        {

        }

        private void InteractWithMenuPanel()
        {
            if (IsMenuOpened)
            {
                CloseMenu();
                IsMenuOpened = false;
            }
            else
            {
                OpenMenu();
                IsMenuOpened = true;
            }
        }

        private void OpenMenu()
        {
            OpenPanel();
            MenuInteractedEvent?.Invoke(true);
        }

        private void CloseMenu()
        {
            ClosePanel();
            MenuInteractedEvent?.Invoke(false);
        }
    }
}
