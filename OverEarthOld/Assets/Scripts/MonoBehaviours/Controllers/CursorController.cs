using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class CursorController : MonoBehaviour
    {
        private AimingMethods _aimingMethod = AimingMethods.CameraCenter;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerController.AimingMethodChangedEvent += ChangeCursor;
            PlayerController.MenuButtonPressedEvent += ChangeCursor;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            PlayerController.AimingMethodChangedEvent -= ChangeCursor;
            PlayerController.MenuButtonPressedEvent -= ChangeCursor;
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void ChangeCursor(AimingMethods aimingMethod)
        {
            _aimingMethod = aimingMethod;
            if (_aimingMethod == AimingMethods.CameraCenter)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (_aimingMethod == AimingMethods.CursorPosition)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        private void ChangeCursor(bool value)
        {
            if (value)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (!value && _aimingMethod == AimingMethods.CameraCenter)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
