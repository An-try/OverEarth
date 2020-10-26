using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    [RequireComponent(typeof(Inventory), typeof(Equipment))]
    public abstract class Ship : MonoBehaviour
    {
        [SerializeField] private Transform _shipObservingPlaceForCamera;

        public List<Damageable> DamageableParts { get; private set; } = new List<Damageable>();

        public int Transmission = 0; // Transmission can be positive and negative. This defines the ship moving direction - forward or backward

        private float _maxDurability = 0;
        private float _maxArmor = 0;
        private float _maxSpeed = 100;
        
        private float _thrustForce = 1f; // Forward or backward speed
        private float _strafeForce = 200f; // Strafe speed. Directions: up, down, right, left
        private float _rotationForce = 500f;
        private float _maxRotationSpeed = 0.1f;

        private float _stopForce = 2f;
        private float _autoStopForce = 1f;
        private float _currentStopForce = 0f;

        public float RadarRange = 0f;

        private int _shipFiresAmount = 0;

        public Inventory Inventory { get; private set; }
        public Equipment Equipment { get; private set; }

        public GameObject MainHull;
        public MeshRenderer MainHullTexture;

        public GameObject TargetCameraRotatesAround; // Player's ship game object around which the camera rotates

        // TODO: Game objects that enemies of this ship will be aiming at
        // These markers have already been added to Star Destroyer but the turrets do not know how to aim at them
        public GameObject SelfTargetMarkers;

        public GameObject ShipBurningParticles;

        private WarpAnimation _warpAnimation;
        private bool _isWarpEnded = false;


        
        public Transform ShipObservingPlaceForCamera => _shipObservingPlaceForCamera;
        public bool IsPrepareForWarpAnimationCompleted => _warpAnimation.IsPrepareForWarpAnimationCompleted;
        public bool CanWarp => _warpAnimation.CanWarp;
        public bool IsWarpEnded => _isWarpEnded;

        public float MaxSpeed => _maxSpeed;
        public float ThrustForce => _thrustForce;
        public float StrafeForce => _strafeForce;
        public float RotationForce => _rotationForce;
        public float MaxRotationSpeed => _maxRotationSpeed;
        public float StopForce => _stopForce;
        public float AutoStopForce => _autoStopForce;

        private void Awake()
        {
            DamageableParts = GetComponentsInChildren<Damageable>().ToList();

            for (int i = 0; i < DamageableParts.Count; i++)
            {
                _maxDurability += DamageableParts[i].MaxDurability;
                _maxArmor += DamageableParts[i].MaxArmor;
            }

            SubscribeEvents();

            Inventory = GetComponent<Inventory>();
            Equipment = GetComponent<Equipment>();

            _warpAnimation = GetComponentInChildren<WarpAnimation>();
        }

        private void SubscribeEvents()
        {
            for (int i = 0; i < DamageableParts.Count; i++)
            {
                DamageableParts[i].DestroyedEvent += RemovePartFromShip;
            }
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            for (int i = 0; i < DamageableParts.Count; i++)
            {
                DamageableParts[i].DestroyedEvent -= RemovePartFromShip;
            }
        }

        private void RemovePartFromShip(Damageable damageable)
        {
            for (int i = 0; i < DamageableParts.Count; i++)
            {
                if (DamageableParts[i] == damageable)
                {
                    DamageableParts[i].DestroyedEvent -= RemovePartFromShip;
                    DamageableParts.Remove(damageable);
                    return;
                }
            }
        }

        private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
        {
            MainHullTexture = MainHull.GetComponent<MeshRenderer>(); // Get mesh renderer of this ship main hull

            _thrustForce *= GetComponent<Rigidbody>().mass; // Multiply forward speed by the mass of the ship
            _rotationForce *= GetComponent<Rigidbody>().mass; // Multiply rotation speed by the mass of the ship

            
            // FOR DEBUGGING.
            for (int i = 0; i < DamageableParts.Count; i++)
            {
                DamageableParts[i].SetDefaultParameters();
            }
        }

        public float GetParameterValue(EntityParameters entityParameter)
        {
            switch (entityParameter)
            {
                case EntityParameters.MaxDurability:
                    return _maxDurability;
                case EntityParameters.MaxArmor:
                    return _maxArmor;
                case EntityParameters.MaxSpeed:
                    return _maxSpeed;
                case EntityParameters.None:
                    Debug.LogError("Enum \"SearchParameter\": \"" + entityParameter + "\" is not correct!"); // TODO: check this output
                    return 0;
                default:
                    Debug.LogError("Enum \"SearchParameter\": \"" + entityParameter + "\" is not correct!"); // TODO: check this output
                    return 0;
            }
        }

        public void PrepareToWarp()
        {
            _warpAnimation.PrepareToWarp();
        }

        public void DoWarp()
        {
            _warpAnimation.DoWarp();
            StartCoroutine(ShipPreWarping());
        }

        private IEnumerator ShipPreWarping()
        {
            PlayerController.Instance.CameraDefaultFieldOfView = PlayerController.Instance.Camera.fieldOfView;
            float time = 5f;

            while (time > 0)
            {
                PlayerController.Instance.Camera.fieldOfView *= 0.999f;

                transform.localScale *= 0.999f;
                transform.position += transform.forward * 1.01f;
                _warpAnimation.transform.position -= transform.forward * 1.01f;

                time -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            yield break;
        }

        public void AnimateShipWarping()
        {
            StartCoroutine(ShipWarping());
        }

        private IEnumerator ShipWarping()
        {
            float time = 0.8f;

            while (time > 0)
            {
                transform.localScale *= 0.9f;
                transform.position += transform.forward * 100;
                _warpAnimation.transform.position -= transform.forward * 100;

                time -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            _isWarpEnded = true;

            StartCoroutine(ExitFromWarp());

            yield break;
        }

        private IEnumerator ExitFromWarp()
        {
            _warpAnimation.transform.position += transform.forward * 100;

            while (transform.localScale.x < 1)
            {
                PlayerController.Instance.Camera.fieldOfView *= 1.005f;
                transform.localScale *= 1.1f;
                transform.position += transform.forward * 100;
                _warpAnimation.transform.position -= transform.forward * 100;
                
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            PlayerController.Instance.Camera.fieldOfView = PlayerController.Instance.CameraDefaultFieldOfView;
            transform.localScale = Vector3.one;

            _isWarpEnded = false;

            StopAllCoroutines();

            yield break;
        }
    }
}
