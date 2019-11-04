using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public abstract class Turret : MonoBehaviour
    {
        [SerializeField] private AudioClip ShootingSound;

        [SerializeField] private TurretEquipment _turretEquipment; // Item for this turret

        public List<string> TargetTags; // Targets for this turret
        private protected Transform _target;
        private protected Transform _targetPart;

        public GameObject TurretBase; // Base platform of the turret that rotates horizontally
        public GameObject TurretCannons; // Cannons of the turret that totates vertically

        public GameObject ShootPlace;
        public GameObject ShootAnimationPrefab;
        //public GameObject HitHole;

        public Vector3 AimPoint; // The point that the turret should look at

        private float _turnRate; // Turret turning speed
        private protected float _turretRange;
        private protected float _maxCooldown;
        private protected float _currentCooldown;

        [Range(0.0f, 180.0f)] private float _rightTraverse; // Maximum right turn in degrees
        [Range(0.0f, 180.0f)] private float _leftTraverse; // Maximum left turn in degrees
        [Range(0.0f, 90.0f)] private float _elevation; // Maximum turn up in degrees
        [Range(0.0f, 90.0f)] private float _depression; // Maximum turn down in degrees

        public bool turretAI; // If the turret is controlled by AI

        private float _defaultTimeToFindTarget = 1f;
        private float _currentTimeToFindTarget;

        public float Range => _turretRange;
        public float RightTraverse => _rightTraverse;
        public float LeftTraverse => _leftTraverse;
        public float Elevation => _elevation;
        public float Depression => _depression;

        public abstract void Shoot();

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerController.AIStateChangedEvent += ChangeTurretAIState;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            PlayerController.AIStateChangedEvent -= ChangeTurretAIState;
        }

        private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
        {
            SetTurretParameters();
        }

        private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
        {
            if (PlayerController.IsMenuOpened)
            {
                return;
            }

            FindTarget();

            if (turretAI) // If the turret is controlled by AI
            {
                AutomaticTurretControl();
            }
            else // If the turret is not controlled by AI
            {
                ManualTurretControl();
            }

            CooldownDecrease();
        }

        public virtual void SetTurretParameters()
        {
            _turnRate = _turretEquipment.TurnRate;
            _turretRange = _turretEquipment.Range;
            _maxCooldown = _turretEquipment.MaxCooldown;
            _currentCooldown = _maxCooldown;

            _rightTraverse = _turretEquipment.RightTraverse;
            _leftTraverse = _turretEquipment.LeftTraverse;
            _elevation = _turretEquipment.Elevation;
            _depression = _turretEquipment.Depression;

            // Check this turret tag
            switch (transform.root.tag)
            {
                case "Player":  // If this turret is on a player ship
                    turretAI = PlayerController.IsAIEnabled; // Set if the turret is controlled by AI
                    TargetTags = new List<string> { "Enemy" }; // Set enemies as targets for this turret
                    break;
                case "Ally": // If this turret is on an ally ship
                    turretAI = true; // Set the turret under AI control
                    TargetTags = new List<string> { "Enemy" }; // Set enemies as targets for this turret
                    break;
                case "Enemy": // If this turret is on an enemy ship
                    turretAI = true; // Set the turret under AI control
                    TargetTags = new List<string> { "Player", "Ally" }; // Set allies as targets for this turret
                    break;
                default:
                    break;
            }
        }

        // Set the turret AI. This method executes by delegate of PlayerMovement script
        public void ChangeTurretAIState(bool AIState)
        {
            if (transform.root.CompareTag("Player"))
            {
                turretAI = AIState;
            }
        }

        // Decrease turret cooldown each fixed update
        public void CooldownDecrease()
        {
            if (_currentCooldown >= 0)
            {
                _currentCooldown -= Time.deltaTime;
            }
        }

        public bool CooldownIsZero()
        {
            if (_currentCooldown <= 0)
            {
                return true;
            }
            return false;
        }

        // Method executes when turret AI is enabled
        public void AutomaticTurretControl()
        {
            if (_targetPart != null) // If there is any target
            {
                AimPoint = _targetPart.position; // Set a target position as an aim point

                RotateBase(); // Totate base of the turret to the aim point
                RotateCannons(); // Totate cannons of the turret to the aim point
            }
            else // If there is no target
            {
                RotateToDefault(); // Rotate turret to default
            }

            // If the turret is aimed at the enemy, its cooldown is zero and it is not aimed at the owner
            if (CooldownIsZero() && AimedAtEnemy() && !AimedAtOwner())
            {
                Shoot();
            }
        }

        // Method executes when turret AI is disabled
        public void ManualTurretControl()
        {
            AimPoint = CameraController.Instance.AimPoint; // Set the point on which camera is looking as an aim point

            RotateBase(); // Totate base of the turret to the aim point
            RotateCannons(); // Totate cannons of the turret to the aim point

            // If a player press left mouse button, turret cooldown is zero and it is not aimed at the owner
            if (Input.GetKey(KeyCode.Mouse0) && CooldownIsZero() && !AimedAtOwner())
            {
                Shoot();
            }
        }

        public void RotateBase()
        {
            // Get local position of aim point in relative to this turret
            Vector3 localTargetPos = transform.InverseTransformPoint(AimPoint);
            localTargetPos.y = 0f; // Put the aiming point at the same height with this tower

            Vector3 clampedLocalVector2Target = localTargetPos; // New point to rotate with clamped rotate traverses

            float traverse = localTargetPos.x >= 0 ? _rightTraverse : _leftTraverse;
            clampedLocalVector2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * traverse, float.MaxValue);

            Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVector2Target); // Create a new rotation that looking at new point
                                                                                          // Rotates current turret to the new quaternion
            Quaternion newRotation = Quaternion.RotateTowards(TurretBase.transform.localRotation, rotationGoal, _turnRate * Time.deltaTime);

            TurretBase.transform.localRotation = newRotation; // Apply intermediate rotation to the turret
        }

        public void RotateCannons()
        {
            // Get local position of aim point in relative to this turret
            Vector3 localTargetPos = TurretBase.transform.InverseTransformPoint(AimPoint);
            localTargetPos.x = 0f; // Put the aiming point at the same vertical with this tower

            Vector3 clampedLocalVec2Target = localTargetPos; // New point to rotate with clamped rotate traverses

            float traverse = localTargetPos.y >= 0 ? _elevation : _depression;
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * traverse, float.MaxValue);

            Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target); // Create a new rotation that looking at new point
                                                                                       // Rotates current turret to the new quaternion
            Quaternion newRotation = Quaternion.RotateTowards(TurretCannons.transform.localRotation, rotationGoal, 2 * _turnRate * Time.deltaTime);

            TurretCannons.transform.localRotation = newRotation; // Apply intermediate rotation to the turret
        }

        public void RotateToDefault()
        {
            // Set new intermediate rotation of base and cannons to default rotation
            Quaternion newBaseRotation = Quaternion.RotateTowards(TurretBase.transform.localRotation, Quaternion.identity, _turnRate * Time.deltaTime);
            Quaternion newCannonRotation = Quaternion.RotateTowards(TurretCannons.transform.localRotation, Quaternion.identity, 2.0f * _turnRate * Time.deltaTime);

            // Apply intermediate rotation
            TurretBase.transform.localRotation = newBaseRotation;
            TurretCannons.transform.localRotation = newCannonRotation;
        }

        public virtual bool AimedAtEnemy()
        {
            // Select specific layers by shifting the bits. These layers will be ignored by the turret raycast
            // Layer 8 is a bullet and 9 is a missile
            int layerMask = (1 << 8) | (1 << 9);
            layerMask = ~layerMask; // Invert these layers. So raycast will ignore bullets and missiles

            // Create an outgoing ray from cannons with turret range lenght
            Ray aimingRay = new Ray(TurretCannons.transform.position, TurretCannons.transform.forward * _turretRange);

            // If the turret is targeting an object except bullets and rockets (determined by layerMask)
            if (Physics.Raycast(aimingRay, out RaycastHit hit, _turretRange, layerMask))
            {
                if (hit.collider.transform.root == _target) // If aiming on current nearest target
                {
                    return true; // Aimed at the enemy
                }
            }
            return false; // Not aimed at the enemy
        }

        // If the turret aimed at the ship on which it is attached
        public bool AimedAtOwner()
        {
            // Create an outgoing ray from cannons with turret range lenght
            Ray aimingRay = new Ray(TurretCannons.transform.position, TurretCannons.transform.forward * _turretRange);

            if (Physics.Raycast(aimingRay, out RaycastHit hit, _turretRange)) // If turret aiming at some object
            {
                if (hit.transform.root == transform.root) // If this object is the current ship on which turret is attached
                {
                    return true; // Aimed at the owner
                }
            }
            return false; // Not aimed at the owner
        }

        private void FindTarget()
        {
            _currentTimeToFindTarget -= Time.fixedDeltaTime;

            if (_currentTimeToFindTarget <= 0)
            {
                _targetPart = Methods.SearchNearestTarget(transform, TargetTags, out Transform target, EntityParameters.None, MinMaxValues.MaxValue);
                _target = target;
                _currentTimeToFindTarget = _defaultTimeToFindTarget;
            }
        }
    }
}
