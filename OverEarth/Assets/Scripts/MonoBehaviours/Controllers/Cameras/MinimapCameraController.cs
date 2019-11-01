using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class MinimapCameraController : Singleton<MinimapCameraController>
    {
        [SerializeField] private Camera _minimapCamera;
        [SerializeField] private Transform _cameraConeParent;
        [SerializeField] private Transform _targetCameraFollow;

        private float _radarRange => PlayerController.Instance.Ship.RadarRange;

        private void Update()
        {
            if (_minimapCamera && _targetCameraFollow)
            {
                //_minimapCamera.farClipPlane = _radarRange * 2;
                _minimapCamera.orthographicSize = _radarRange;

                Vector3 newPosition = new Vector3(_targetCameraFollow.position.x, _radarRange, _targetCameraFollow.position.z);
                //_minimapCamera.transform.position = newPosition;

                Vector3 newRotation = new Vector3(90, _targetCameraFollow.eulerAngles.x, _targetCameraFollow.eulerAngles.z);
                _minimapCamera.transform.eulerAngles = newRotation;

                _cameraConeParent.transform.eulerAngles = new Vector3(0, _cameraConeParent.transform.eulerAngles.y, 0);
            }
        }
    }
}
