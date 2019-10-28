using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public class PlayerShipController : Singleton<PlayerShipController>
    {
        [SerializeField] private Ship _shipScript;
        [SerializeField] private Rigidbody _playerShipRigidbody;

        public Transform ShipObservingPlace => _shipScript.ShipObservingPlaceForCamera;
        public float ShipVelocityMagnitude => _playerShipRigidbody.velocity.magnitude;
        public float ShipDrag => _playerShipRigidbody.drag;
        public float ShipAngularDrag => _playerShipRigidbody.angularDrag;

        private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
        {
            _playerShipRigidbody.maxAngularVelocity = _shipScript.MaxRotationSpeed; // Set max rotation speed to ship's rigidbody max angular velocity
        }

        private void Update() // Update is called every frame
        {
            UpdateTransmissionByInputs();
            ClampShipTransmission();
        }

        private void UpdateTransmissionByInputs()
        {
            // If player press any plus button on the keyboard
            if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals))
            {
                _shipScript.Transmission++; // Add a transmission to the ship
            }

            // If player press any minus button on the keyboard
            if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
            {
                _shipScript.Transmission--; // Subtract a transmission from the ship
            }
        }

        public void UpdateTransmissionByValue(int value)
        {
            _shipScript.Transmission = value;
        }

        private void ClampShipTransmission()
        {
            _shipScript.Transmission = Mathf.Clamp(_shipScript.Transmission, -4, 4); // Clamp ship transmission from -4 to 4
        }

        private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
        {
            MoveShip();
        }

        private void MoveShip()
        {
            MoveForwardOrBack(_shipScript.Transmission);

            // Call the move methods. Transfer a parameters of input system as moving directions: positive or negative multiplying by forces
            RotateRightOrLeft(Input.GetAxis("RotateRightLeft") * _shipScript.RotationForce);
            RotateUpOrDown(Input.GetAxis("RotateUpDown") * _shipScript.RotationForce);
            RotateSide(Input.GetAxis("RotateSide") * _shipScript.RotationForce);
            StrafeSide(Input.GetAxis("StrafeSide") * _shipScript.StrafeForce);
            StrafeUpOrDown(Input.GetAxis("StrafeUpDown") * _shipScript.StrafeForce);

            // If the player’s ship has no orders to move
            if (_shipScript.Transmission == 0 && Input.GetAxis("StrafeSide") == 0 && Input.GetAxis("StrafeUpDown") == 0)
            {
                _playerShipRigidbody.drag = _shipScript.AutoStopForce; // Apply stop force to the ship
            }
            else // If the player ordered the ship to move in any direction
            {
                _playerShipRigidbody.drag = 0; // Do not stop the ship
            }

            // If the player’s ship has no orders to rotate
            if (Input.GetAxis("RotateRightLeft") == 0 && Input.GetAxis("RotateUpDown") == 0 && Input.GetAxis("RotateSide") == 0)
            {
                _playerShipRigidbody.angularDrag = 1; // Apply stop rotation force to the ship
            }
            else // If the player ordered the ship to rotate in any direction
            {
                _playerShipRigidbody.angularDrag = 0; // Do not stop rotation of the ship
            }

            if (Input.GetKey(KeyCode.Space)) // If player holds space button
            {
                _playerShipRigidbody.drag = _shipScript.StopForce; // Apply stop force to the ship
                _playerShipRigidbody.angularDrag = _shipScript.StopForce; // Apply stop rotation force to the ship
            }

            ClampShipVelocity(); // Clamp ship speed
        }

        private void ClampShipVelocity()
        {
            if (_playerShipRigidbody.velocity.magnitude > _shipScript.MaxSpeed) // If the current speed of the ship is higher than the maximum speed of the ship
            {
                // Normalyze rigidbody velocity to maximum speed of the ship
                _playerShipRigidbody.velocity = _playerShipRigidbody.velocity.normalized * _shipScript.MaxSpeed;
            }
        }

        public void MoveForwardOrBack(float transmission)
        {
            // Add force to player ship rigidbody to move forward or backward
            _playerShipRigidbody.AddForce(_playerShipRigidbody.transform.forward * _shipScript.ThrustForce * transmission);
        }

        public void RotateRightOrLeft(float direction)
        {
            // Add torque to player ship rigidbody to rotate right or left
            _playerShipRigidbody.AddTorque(_playerShipRigidbody.transform.up * direction);
        }

        public void RotateUpOrDown(float direction)
        {
            // Add torque to player ship rigidbody to rotate up or down
            _playerShipRigidbody.AddTorque(_playerShipRigidbody.transform.right * direction);
        }

        public void RotateSide(float direction)
        {
            // Add torque to player ship rigidbody to rotate around z axis
            _playerShipRigidbody.AddTorque(_playerShipRigidbody.transform.forward * direction);
        }

        public void StrafeSide(float direction)
        {
            // Add force to player ship rigidbody to move right or left
            _playerShipRigidbody.AddForce(_playerShipRigidbody.transform.right * direction * _shipScript.StrafeForce);
        }

        public void StrafeUpOrDown(float direction)
        {
            // Add force to player ship rigidbody to move up or down
            _playerShipRigidbody.AddForce(_playerShipRigidbody.transform.up * direction * _shipScript.StrafeForce);
        }
    }
}
