using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _targetCameraRotatesAround; // Player's ship game object around which the camera rotates

        [SerializeField] private float _scrollWheelSensitivity = 100f;
        [SerializeField] private float _scrollMax = 1000f;
        [SerializeField] private float _scrollMin = 0f;
        [SerializeField] private float _defaultMouseRotateSensitivity = 3f;
        [SerializeField] private float _zoomedCameraFieldOfView = 10f; // Field of view when zooming

        [SerializeField] private bool _canMovingCamera = true;

        [SerializeField] private AimingMethods _aimingMethod = AimingMethods.CameraCenter;

        private Vector3 _cameraOffset = Vector3.zero;
        private Vector3 _cameraLastPosition = Vector3.zero;
        private Vector3 _cameraLastEulerAngles = Vector3.zero;

        private float _cameraAimRayLength; // Ray length that based on camera far clip plane
        private float _cameraDefaultFieldOfView;
        private float _curentMouseRotateSensitivity;
        private float _limitRotateY = 90f; // Rotation limit by Y
        private float _cameraRotateAngleX;
        private float _cameraRotateAngleY;

        public Vector3 AimPoint
        {
            get
            {
                if (_aimingMethod == AimingMethods.CameraCenter)
                {
                    return CameraCenterLookingPoint();
                }
                else if (_aimingMethod == AimingMethods.CursorPosition)
                {
                    return CursorRayHitPoint();
                }

                Debug.LogError("AimingMethods is invalid!");
                return Vector3.zero;
            }
        }

        public Ray AimRay
        {
            get
            {
                if (_aimingMethod == AimingMethods.CameraCenter)
                {
                    return new Ray(_camera.transform.position, _camera.transform.forward);
                }
                else if (_aimingMethod == AimingMethods.CursorPosition)
                {
                    return _camera.ScreenPointToRay(Input.mousePosition);
                }

                Debug.LogError("AimingMethods is invalid!");
                return new Ray();
            }
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerController.AimingMethodChangedEvent += SetAimingMethod;
            MenuPanelUIController.MenuInteractedEvent += ChangeCameraObservingPoint;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            PlayerController.AimingMethodChangedEvent -= SetAimingMethod;
            MenuPanelUIController.MenuInteractedEvent -= ChangeCameraObservingPoint;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _cameraDefaultFieldOfView = _camera.fieldOfView; // Set the current camera field of view as default
            _cameraAimRayLength = _camera.farClipPlane; // Set ray length
            _curentMouseRotateSensitivity = _defaultMouseRotateSensitivity; // Set current mouse rotate sensitivity

            _limitRotateY = Mathf.Abs(_limitRotateY); // Get the absolute value of limit rotate Y
            _limitRotateY = Mathf.Clamp(_limitRotateY, -Mathf.Infinity, 90); // Clamp limit rotate Y up to 90

            _cameraOffset = new Vector3(_cameraOffset.x, _cameraOffset.y, -Mathf.Abs(_scrollMax) / 2); // Set camera offset for start camera position

            _camera.transform.position = _targetCameraRotatesAround.position + _cameraOffset; // Set start camera position
        }

        private void Update() // Update is called every frame
        {
            if (_canMovingCamera && _aimingMethod == AimingMethods.CameraCenter)
            {
                RotateCameraTowardsObject();
                MoveCameraTowardsObject();
            }

            // Camera zooming
            if (Input.GetKey(KeyCode.Mouse1)) // If right mouse button is pressed
            {
                _camera.fieldOfView = _zoomedCameraFieldOfView; // Set new camera field of view
                _curentMouseRotateSensitivity = _defaultMouseRotateSensitivity / 3; // Decrease mouse rotate sensitivity
            }
            else // If right mouse button is not pressed
            {
                _camera.fieldOfView = _cameraDefaultFieldOfView; // Set default camera field of view
                _curentMouseRotateSensitivity = _defaultMouseRotateSensitivity; // Set default mouse rotate sensitivity
            }

            // Camera scrolling
            _cameraOffset.z += Input.GetAxis("Mouse ScrollWheel") * _scrollWheelSensitivity;
        }

        private void MoveCameraTowardsObject()
        {
            _camera.transform.position = _camera.transform.localRotation * _cameraOffset + _targetCameraRotatesAround.position;
        }

        private void RotateCameraTowardsObject()
        {
            _cameraOffset.z = Mathf.Clamp(_cameraOffset.z, -Mathf.Abs(_scrollMax), -Mathf.Abs(_scrollMin)); // Clamp camera offset

            _cameraRotateAngleX = _camera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _curentMouseRotateSensitivity; // Set new camera rotate angle by X
            _cameraRotateAngleY += Input.GetAxis("Mouse Y") * _curentMouseRotateSensitivity; // Set new camera rotate angle by Y
            _cameraRotateAngleY = Mathf.Clamp(_cameraRotateAngleY, -_limitRotateY, _limitRotateY); // Clamp rotation by Y

            _camera.transform.eulerAngles = new Vector3(-_cameraRotateAngleY, _cameraRotateAngleX, 0); // Set new camera rotation.
        }

        private Vector3 CameraCenterLookingPoint()
        {
            // Ray that comes out of the camera and determining contact point if it hit something
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, _cameraAimRayLength)) // If this ray hits something
            {
                return new Vector3(hit.point.x, hit.point.y, hit.point.z); // Set hit coordinates
            }
            else // If ray doesn't hit anything
            {
                // Set the point that is at the end of the ray coming out of the camera
                return new Vector3(_camera.transform.position.x + _camera.transform.forward.x * _cameraAimRayLength,
                                   _camera.transform.position.y + _camera.transform.forward.y * _cameraAimRayLength,
                                   _camera.transform.position.z + _camera.transform.forward.z * _cameraAimRayLength);
            }
        }

        private Vector3 CursorRayHitPoint()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _cameraAimRayLength)) // If this ray hits something
            {
                return new Vector3(hit.point.x, hit.point.y, hit.point.z); // Set hit coordinates
            }
            else // If ray doesn't hit anything
            {
                return ray.direction * _cameraAimRayLength;
            }
        }

        private void ChangeCameraObservingPoint(bool inventoryIsOpened)
        {
            if (inventoryIsOpened)
            {
                _canMovingCamera = false;
                _cameraLastPosition = _camera.transform.position;
                _cameraLastEulerAngles = _camera.transform.eulerAngles;
                _camera.transform.position = PlayerShipController.Instance.ShipObservingPlace.position;
                _camera.transform.rotation = PlayerShipController.Instance.ShipObservingPlace.rotation;
            }
            else
            {
                _canMovingCamera = true;
                _camera.transform.position = _cameraLastPosition;
                _camera.transform.eulerAngles = _cameraLastEulerAngles;
            }
        }

        private void SetAimingMethod(AimingMethods aimingMethod)
        {
            _aimingMethod = aimingMethod;
        }
    }
}
