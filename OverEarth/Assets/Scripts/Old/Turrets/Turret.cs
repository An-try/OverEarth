using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public abstract class Turret : MonoBehaviour
    {
        [SerializeField] private AudioClip ShootingSound;

        public Item item; // Item for this turret

        public List<string> TargetTags; // Targets for this turret
        private protected Transform _target;

        public GameObject TurretBase; // Base platform of the turret that rotates horizontally
        public GameObject TurretCannons; // Cannons of the turret that totates vertically

        public GameObject ShootPlace;
        public GameObject ShootAnimationPrefab;
        //public GameObject HitHole;

        public Vector3 AimPoint; // The point that the turret should look at

        public float turnRate; // Turret turning speed
        private protected float _turretRange;
        public float cooldown;
        public float currentCooldown;

        [Range(0.0f, 180.0f)] public float _rightTraverse; // Maximum right turn in degrees
        [Range(0.0f, 180.0f)] public float _leftTraverse; // Maximum left turn in degrees
        [Range(0.0f, 90.0f)] public float _elevation; // Maximum turn up in degrees
        [Range(0.0f, 90.0f)] public float _depression; // Maximum turn down in degrees

        public bool turretAI; // If the turret is controlled by AI

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
            if (PlayerController.Instance.IsMenuOpened)
            {
                return;
            }

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
            // Check this turret tag
            switch (transform.root.tag)
            {
                case "Player":  // If this turret is on a player ship
                    turretAI = PlayerController.Instance.IsAIEnabled; // Set if the turret is controlled by AI
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
            if (currentCooldown >= 0)
            {
                currentCooldown -= Time.deltaTime;
            }
        }

        public bool CooldownIsZero()
        {
            if (currentCooldown <= 0)
            {
                return true;
            }
            return false;
        }

        // Method executes when turret AI is enabled
        public void AutomaticTurretControl()
        {
            _target = Methods.SearchNearestTarget(transform, TargetTags, EntityParameters.None, MinMaxValues.MaxValue);

            if (_target != null) // If there is any target
            {
                AimPoint = _target.position; // Set a target position as an aim point

                RotateBase(); // Totate base of the turret to the aim point
                RotateCannons(); // Totate cannons of the turret to the aim point
            }
            else // If there is no target
            {
                RotateToDefault(); // Rotate turret to default
            }

            // If the turret is aimed at the enemy, its cooldown is zero and it is not aimed at the owner
            if (AimedAtEnemy() && CooldownIsZero() && !AimedAtOwner())
            {
                Shoot();
            }
        }

        // Method executes when turret AI is disabled
        public void ManualTurretControl()
        {
            //AimPoint = CameraController.Instance.MouseAimRay.direction * _turretRange; // Set the point on which camera is looking as an aim point
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

            if (localTargetPos.x >= 0f) // If the aim point is located to the right of the turret
            {
                // Set new position to rotate with max right traverse
                clampedLocalVector2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * _rightTraverse, float.MaxValue);
            }
            else // If the aim point is located to the left of the turret
            {
                // Set new position to rotate with max left traverse
                clampedLocalVector2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * _leftTraverse, float.MaxValue);
            }

            Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVector2Target); // Create a new rotation that looking at new point
                                                                                          // Rotates current turret to the new quaternion
            Quaternion newRotation = Quaternion.RotateTowards(TurretBase.transform.localRotation, rotationGoal, turnRate * Time.deltaTime);

            TurretBase.transform.localRotation = newRotation; // Apply intermediate rotation to the turret
        }

        public void RotateCannons()
        {
            // Get local position of aim point in relative to this turret
            Vector3 localTargetPos = TurretBase.transform.InverseTransformPoint(AimPoint);
            localTargetPos.x = 0f; // Put the aiming point at the same vertical with this tower

            Vector3 clampedLocalVec2Target = localTargetPos; // New point to rotate with clamped rotate traverses

            if (localTargetPos.y >= 0f) // If the aim point is located above the turret
            {
                // Set new position to rotate with max up traverse
                clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * _elevation, float.MaxValue);
            }
            else // If the aim point is located below the turret
            {
                // Set new position to rotate with max down traverse
                clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * _depression, float.MaxValue);
            }

            Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target); // Create a new rotation that looking at new point
                                                                                       // Rotates current turret to the new quaternion
            Quaternion newRotation = Quaternion.RotateTowards(TurretCannons.transform.localRotation, rotationGoal, 2 * turnRate * Time.deltaTime);

            TurretCannons.transform.localRotation = newRotation; // Apply intermediate rotation to the turret
        }

        public void RotateToDefault()
        {
            // Set new intermediate rotation of base and cannons to default rotation
            Quaternion newBaseRotation = Quaternion.RotateTowards(TurretBase.transform.localRotation, Quaternion.identity, turnRate * Time.deltaTime);
            Quaternion newCannonRotation = Quaternion.RotateTowards(TurretCannons.transform.localRotation, Quaternion.identity, 2.0f * turnRate * Time.deltaTime);

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
                else // If not aiming on current nearest target
                {
                    return false; // Not aimed at the enemy
                }
            }
            else // If turret is not aimed at anything
            {
                return false; // Not aimed at the enemy
            }
        }

        // If the turret aimed at the ship on which it is attached
        public bool AimedAtOwner()
        {
            // Create an outgoing ray from cannons with turret range lenght
            Ray aimingRay = new Ray(TurretCannons.transform.position, TurretCannons.transform.forward * _turretRange);

            if (Physics.Raycast(aimingRay, out RaycastHit hit, _turretRange)) // If turret aiming at some object
            {
                if (hit.collider.transform.root == gameObject.transform.root) // If this object is the current ship on which turret is attached
                {
                    return true; // Aimed at the owner
                }
                else
                {
                    return false; // Not aimed at the owner
                }
            }
            return false; // Not aimed at the owner
        }
    }
}
