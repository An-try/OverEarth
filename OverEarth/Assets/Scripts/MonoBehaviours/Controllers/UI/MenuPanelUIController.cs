using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class MenuPanelUIController : PanelUI<MenuPanelUIController>
    {
        [SerializeField] private Transform _equipmentPanel;
        [SerializeField] private Equipment _equipment;
        [SerializeField] private Transform _inventorySlotsContainer;

        private List<EquipmentSlotUI> _equipmentSlotsUI;
        public List<InventorySlotUI> InventorySlotsUI { get; private set; }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerController.MenuButtonPressedEvent += InteractWithPanel;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            PlayerController.MenuButtonPressedEvent -= InteractWithPanel;
        }

        private void InteractWithPanel(bool isMenuOpened)
        {
            if (!isMenuOpened)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();
            }
        }

        private protected override void Awake()
        {
            base.Awake();

            _equipmentSlotsUI = _equipmentPanel.GetComponentsInChildren<EquipmentSlotUI>().ToList();
            InventorySlotsUI = _inventorySlotsContainer.GetComponentsInChildren<InventorySlotUI>().ToList();
        }

        private protected override void Init()
        {
            _panelGroup.alpha = 0;
            _panelGroup.interactable = false;
            _panelGroup.blocksRaycasts = false;
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
    }
}
