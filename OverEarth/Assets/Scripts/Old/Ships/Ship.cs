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

        public List<Damageable> DamageableParts { get; private set; }

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

        

        public Transform ShipObservingPlaceForCamera => _shipObservingPlaceForCamera;
        
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

            //InvokeRepeating("SetShipBurningSimulationIfShipIsOnFire", 0f, 0.1f); // Start or stop ship burning simulation

            
            // FOR DEBUGGING.
            for (int i = 0; i < DamageableParts.Count; i++)
            {
                DamageableParts[i].SetDefaultParameters();
            }
        }

        //private void FixedUpdate()
        //{
        //    if (_currentDurability <= 0) // If health points are zero
        //    {
        //        DestroyShip(); // Destroy this ship
        //        return;
        //    }
        //}

        //private void SetShipBurningSimulationIfShipIsOnFire()
        //{
        //    for (int i = 0; i < ShipBurningParticles.transform.childCount; i++) // Pass all game objects that contains burning particles
        //    {
        //        // Get particle system on game object that contains particles
        //        ParticleSystem particleSystem = ShipBurningParticles.transform.GetChild(i).GetComponent<ParticleSystem>();
        //        var mainParticlesSettings = particleSystem.main; // Get main particle system settings
        //        Light burningLight = particleSystem.GetComponentInChildren<Light>(); // Get light that lights when ship is burning

        //        if (_shipFiresAmount > 0) // If there is any fire on the ship
        //        {
        //            mainParticlesSettings.loop = true; // Set looping to burning particles
        //            particleSystem.Play(); // Start a burning particle system
        //            burningLight.enabled = true; // Turn on the burning light
        //            burningLight.intensity = UnityEngine.Random.Range(250, 351); // Change the intensity of burning light to create a simple burning sensation
        //        }
        //        else
        //        {
        //            mainParticlesSettings.loop = false; // Set no looping to burning particles
        //            burningLight.enabled = false; // Turn off the burning light
        //        }
        //    }
        //}

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

        //private void DestroyShip()
        //{
        //    Destroy(gameObject); // Destroy this game object
        //}
    }
}
