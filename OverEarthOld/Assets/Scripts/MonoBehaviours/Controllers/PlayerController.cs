using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class PlayerController : Singleton<PlayerController>
    {
        public Camera Camera;
        public Ship Ship;
        //public Station Station;

        public static event Action<bool> AIStateChangedEvent;
        public static event Action<bool> MenuButtonPressedEvent;
        public static event Action<AimingMethods> AimingMethodChangedEvent;

        private AimingMethods _aimingMethod = AimingMethods.CameraCenter;

        public float CameraDefaultFieldOfView;

        //public static bool IsInStation { get; private set; }
        public static bool IsAIEnabled { get; private set; }
        public static bool IsMenuOpened { get; private set; } = false;
        public static bool IsInsideControlCenter { get; private set; }

        private void Start()
        {
            Camera = Camera.main;
            CameraDefaultFieldOfView = Camera.fieldOfView;
        }

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

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ChangeCamera();
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

        private void ChangeCamera()
        {
            Camera camera = Ship.GetComponentInChildren<Camera>();

            camera.enabled = !camera.enabled;

            if (camera.enabled)
            {
                Camera = camera;
                MainCameraUIPanelController.Instance.ClosePanel();
                IsInsideControlCenter = true;
            }
            else
            {
                Camera = Camera.main;
                MainCameraUIPanelController.Instance.OpenPanel();
                IsInsideControlCenter = false;
            }
        }
    }
}
