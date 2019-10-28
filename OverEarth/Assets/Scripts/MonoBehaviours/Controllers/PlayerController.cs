using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class PlayerController : Singleton<PlayerController>
    {
        public Ship Ship;
        public Station Station;

        public static event Action<bool> AIStateChangedEvent;
        public static event Action<bool> MenuButtonPressedEvent;
        public static event Action<AimingMethods> AimingMethodChangedEvent;

        private AimingMethods _aimingMethod = AimingMethods.CameraCenter;

        public bool InStation { get; private set; }
        public bool IsAIEnabled { get; private set; }
        public bool IsMenuOpened { get; private set; } = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeAIstate();
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                ChangeAimingMethod();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                MenuButtonPressed();
            }
        }

        private void ChangeAIstate()
        {
            IsAIEnabled = !IsAIEnabled; // Change AI state.
            AIStateChangedEvent?.Invoke(IsAIEnabled);
        }

        private void ChangeAimingMethod()
        {
            if (IsMenuOpened)
            {
                return;
            }

            int aimingMethodIndex = (int)_aimingMethod;
            aimingMethodIndex++;
            if (aimingMethodIndex >= Enum.GetNames(typeof(AimingMethods)).Length)
            {
                aimingMethodIndex = 0;
            }
            _aimingMethod = (AimingMethods)aimingMethodIndex;

            AimingMethodChangedEvent?.Invoke(_aimingMethod);
        }

        private void MenuButtonPressed()
        {
            IsMenuOpened = !IsMenuOpened;
            MenuButtonPressedEvent?.Invoke(IsMenuOpened);
        }
    }
}
