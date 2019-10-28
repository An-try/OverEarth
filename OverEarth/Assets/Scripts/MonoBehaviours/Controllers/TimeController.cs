using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class TimeController : MonoBehaviour
    {
        private static float _timeScale
        {
            get { return Time.timeScale; }
            set { Time.timeScale = value; }
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerController.MenuButtonPressedEvent += SetTimeScale;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            PlayerController.MenuButtonPressedEvent -= SetTimeScale;
        }

        public static void SetTimeScale(float newTimeScale)
        {
            _timeScale = newTimeScale;
        }

        public static void SetTimeScale(bool stopTime)
        {
            //_timeScale = stopTime ? 0 : 1;
        }

        public static void ReverseTimeScale()
        {
            _timeScale = _timeScale == 1 ? 0 : 1;
        }

        public static void ResumeGame()
        {
            _timeScale = 1;
        }

        public static void PauseGame()
        {
            _timeScale = 0;
        }
    }
}
