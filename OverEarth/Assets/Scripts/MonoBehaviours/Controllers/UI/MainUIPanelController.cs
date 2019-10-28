using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class MainUIPanelController : PanelUI<MainUIPanelController>
    {
        [SerializeField] private Transform _mainUIPanel;
        private List<DamageableUI> _damageablesUI;

        private protected override void Awake()
        {
            base.Awake();

            _damageablesUI = _mainUIPanel.GetComponentsInChildren<DamageableUI>().ToList();
        }

        private void Start()
        {
            AssignDamageableParts();
        }

        private void AssignDamageableParts()
        {
            for (int i = 0; i < _damageablesUI.Count; i++)
            {
                _damageablesUI[i].AssignDamageablePart(PlayerController.Instance.Ship);
            }
        }

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

        private protected override void Init()
        {
            _panelGroup.alpha = 1;
            _panelGroup.interactable = true;
            _panelGroup.blocksRaycasts = true;
        }

        private void InteractWithPanel(bool isMenuOpened)
        {
            if (isMenuOpened)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();
            }
        }
    }
}
