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

        private void UpdateInventoryUI()
        {

        }

        private void InteractWithMenuPanel()
        {
            if (MenuOpened)
            {
                CloseMenu();
                MenuOpened = false;
            }
            else
            {
                OpenMenu();
                MenuOpened = true;
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
