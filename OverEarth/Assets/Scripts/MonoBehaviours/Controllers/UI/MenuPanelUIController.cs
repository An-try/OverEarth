using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class MenuPanelUIController : Singleton<MenuPanelUIController>
    {
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private Transform _equipmentPanel;
        [SerializeField] private Equipment _equipment;

        public static event Action<bool> MenuInteractedEvent;

        public bool MenuOpened { get; private set; } = false;

        private List<EquipmentSlotUI> _equipmentSlotsUI;

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
        }

        private void Start()
        {
            SetEquipmentSlotsToUIElementsOnPanel();
        }

        private void SetEquipmentSlotsToUIElementsOnPanel()
        {
            for (int i = 0; i < _equipmentSlotsUI.Count; i++)
            {
                _equipmentSlotsUI[i].Slot = _equipment.EquipmentSlots[i];
            }
        }

        private void UpdateInventoryUI()
        {

        }

        private void InteractWithMenuPanel()
        {
            if (MenuOpened)
            {
                CloseInventory();
                MenuOpened = false;
            }
            else
            {
                OpenInventory();
                MenuOpened = true;
            }
        }

        private void OpenInventory()
        {
            _menuPanel.SetActive(true);

            MenuInteractedEvent?.Invoke(true);
        }

        private void CloseInventory()
        {
            _menuPanel.SetActive(false);

            MenuInteractedEvent?.Invoke(false);
        }
    }
}
