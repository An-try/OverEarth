using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class MinimapCameraController : Singleton<MinimapCameraController>
    {
        [SerializeField] private Camera _minimapCamera;
        [SerializeField] private Transform _targetCameraFollow;

        private float _radarRange => PlayerController.Instance.Ship.RadarRange;

        private void Update()
        {
            if (_minimapCamera && _targetCameraFollow)
            {
                _minimapCamera.farClipPlane = _radarRange * 2;

                Vector3 newPosition = new Vector3(_targetCameraFollow.position.x, _radarRange, _targetCameraFollow.position.z);
                _minimapCamera.transform.position = newPosition;

                Vector3 newRotation = new Vector3(90, _targetCameraFollow.eulerAngles.x, _targetCameraFollow.eulerAngles.z);
                _minimapCamera.transform.eulerAngles = newRotation;
            }
        }
    }
}
