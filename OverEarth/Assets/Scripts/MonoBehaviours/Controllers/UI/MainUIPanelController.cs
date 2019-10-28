using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class MainUIPanelController : PanelUI<MainUIPanelController>
    {
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
