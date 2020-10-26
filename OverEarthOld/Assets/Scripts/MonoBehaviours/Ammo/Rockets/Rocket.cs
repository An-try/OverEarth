using System.Collections.Generic;
using UnityEngine;

namespace OverEarth
{
    public abstract class Rocket : Ammo
    {
        [SerializeField] private RocketWarhead _rocketWarhead; // Rocket warhead component.
        [SerializeField] private RocketEngine _rocketEngine; // Rocket engine component.

        [SerializeField] private GameObject _explosionPrefab; // Explosion effect when rocket hit something
        [SerializeField] private GameObject _explosionSmokePrefab; // Smoke that takes off from a rocket

        private float _maxDurability;
        private float _currentDurability;
        private float _maxArmor;
        private float _currentArmor;
        private float _maxVelocity;
        private float _currentVelocity;

        private Transform _target; // The target that this rocket should pursue
        private Transform _targetPart; // The target part that this rocket should pursue

        private Rigidbody _rocketRigidbody;

        public List<string> TargetTags { private get; set; } // List that contains

        private float _defaultTimeToFindTarget = 1f;
        private float _currentTimeToFindTarget;

        private void Awake() // Awake is called when the script instance is being loaded
        {
            _rocketRigidbody = GetComponent<Rigidbody>();
        }

        private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
        {
            _maxDurability = _rocketWarhead.MaxDurability + _rocketEngine.MaxDurability;
            _currentDurability = _maxDurability;
            _maxArmor = _rocketWarhead.MaxArmor + _rocketEngine.MaxArmor;
            _currentArmor = _maxArmor;
            _maxVelocity = _rocketEngine.MaxVelocity;

            Destroy(gameObject, 60f); // Destroy the rocket after some time
        }

        private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
        {
            FindTarget();

            if (_currentDurability <= 0)
            {
                DestroyRocket();
            }

            // Add missile velocity to the limit
            if (_currentVelocity < _rocketEngine.MaxVelocity)
            {
                _currentVelocity++;
            }

            _rocketRigidbody.velocity = transform.forward * _currentVelocity; // Apply velocity to the missile

            ObstaclesAvoidance(); // Avoid obstacles and move to the target
        }

        private void ObstaclesAvoidance() // TODO: Improve the obstacles avoidance algorithm
        {
            float rayLength = 400f;
            float rocketSizeToAvoid = 3f;

            //#region DEBUG RAYS
            //Debug.DrawRay(transform.position + new Vector3(rocketSizeToAvoid, rocketSizeToAvoid, 0f), transform.forward * rayLength / 2);
            //Debug.DrawRay(transform.position + new Vector3(rocketSizeToAvoid, -rocketSizeToAvoid, 0f), transform.forward * rayLength / 2);
            //Debug.DrawRay(transform.position + new Vector3(-rocketSizeToAvoid, rocketSizeToAvoid, 0f), transform.forward * rayLength / 2);
            //Debug.DrawRay(transform.position + new Vector3(-rocketSizeToAvoid, -rocketSizeToAvoid, 0f), transform.forward * rayLength / 2);

            //Debug.DrawRay(transform.position, (transform.forward + Vector3.up) * rayLength);
            //Debug.DrawRay(transform.position, (transform.forward + Vector3.up + Vector3.right) * rayLength);
            //Debug.DrawRay(transform.position, (transform.forward + Vector3.right) * rayLength);
            //Debug.DrawRay(transform.position, (transform.forward + Vector3.right + Vector3.down) * rayLength);
            //Debug.DrawRay(transform.position, (transform.forward + Vector3.down) * rayLength);
            //Debug.DrawRay(transform.position, (transform.forward + Vector3.down + Vector3.left) * rayLength);
            //Debug.DrawRay(transform.position, (transform.forward + Vector3.left) * rayLength);
            //Debug.DrawRay(transform.position, (transform.forward + Vector3.left + Vector3.up) * rayLength);
            //#endregion

            // Rays that check whether the path is free in front of the rocket
            Ray rayForwardFirst = new Ray(transform.position + new Vector3(rocketSizeToAvoid, rocketSizeToAvoid, 0f), transform.forward);
            Ray rayForwardSecond = new Ray(transform.position + new Vector3(rocketSizeToAvoid, -rocketSizeToAvoid, 0f), transform.forward);
            Ray rayForwardThird = new Ray(transform.position + new Vector3(-rocketSizeToAvoid, rocketSizeToAvoid, 0f), transform.forward);
            Ray rayForwardFourth = new Ray(transform.position + new Vector3(-rocketSizeToAvoid, -rocketSizeToAvoid, 0f), transform.forward);

            // Rays that check whether the path is clear in different directions in front of the rocket
            Ray rayUp = new Ray(transform.position, transform.forward + transform.up);
            Ray rayUpRight = new Ray(transform.position, transform.forward + transform.up + transform.right);
            Ray rayRight = new Ray(transform.position, transform.forward + transform.right);
            Ray rayRightDown = new Ray(transform.position, transform.forward + transform.right + -transform.up);
            Ray rayDown = new Ray(transform.position, transform.forward + -transform.up);
            Ray rayDownLeft = new Ray(transform.position, transform.forward + -transform.up + -transform.right);
            Ray rayLeft = new Ray(transform.position, transform.forward + -transform.right);
            Ray rayLeftUp = new Ray(transform.position, transform.forward + -transform.right + transform.up);

            RaycastHit hit = new RaycastHit();

            // If there is any obstacle on the rocket way
            if (Physics.Raycast(rayForwardFirst, out hit, rayLength / 2) || Physics.Raycast(rayForwardSecond, out hit, rayLength / 2) ||
                Physics.Raycast(rayForwardThird, out hit, rayLength / 2) || Physics.Raycast(rayForwardFourth, out hit, rayLength / 2))
            {
                // If forward rays of this missile does not cross current target of this missile avoid this object
                if (hit.collider.transform.root != _target)
                {
                    // Try to avoid by rotating to the new direction that have no obstacles
                    if (!Physics.Raycast(rayUp, out hit, rayLength)) // If the upper ray hits nothing
                    {
                        RotateToTarget(false, rayUp.GetPoint(rayLength)); // Move to the new point up
                        return;
                    }
                    if (!Physics.Raycast(rayUpRight, out hit, rayLength)) // If the upper right ray hits nothing
                    {
                        RotateToTarget(false, rayUpRight.GetPoint(rayLength)); // Move to the new point up right
                        return;
                    }
                    if (!Physics.Raycast(rayRight, out hit, rayLength)) // If the right ray hits nothing
                    {
                        RotateToTarget(false, rayRight.GetPoint(rayLength)); // Move to the new point right
                        return;
                    }
                    if (!Physics.Raycast(rayRightDown, out hit, rayLength)) // If the lower right ray hits nothing
                    {
                        RotateToTarget(false, rayRightDown.GetPoint(rayLength)); // Move to the new point right down
                        return;
                    }
                    if (!Physics.Raycast(rayDown, out hit, rayLength)) // If the lower ray hits nothing
                    {
                        RotateToTarget(false, rayDown.GetPoint(rayLength)); // Move to the new point down
                        return;
                    }
                    if (!Physics.Raycast(rayDownLeft, out hit, rayLength)) // If the lower left ray hits nothing
                    {
                        RotateToTarget(false, rayDownLeft.GetPoint(rayLength)); // Move to the new point down left
                        return;
                    }
                    if (!Physics.Raycast(rayLeft, out hit, rayLength)) // If the left ray hits nothing
                    {
                        RotateToTarget(false, rayLeft.GetPoint(rayLength)); // Move to the new point left
                        return;
                    }
                    if (!Physics.Raycast(rayLeftUp, out hit, rayLength)) // If the upper left ray hits nothing
                    {
                        RotateToTarget(false, rayLeftUp.GetPoint(rayLength)); // Move to the new point left up
                        return;
                    }
                }
            }
            else // If there isn't obstacle on the rocket way
            {
                RotateToTarget(true, transform.position + transform.forward); // Move rocket to target. If there is no target move forward
            }
        }

        private void RotateToTarget(bool moveToTarget, Vector3 newAvoidingPosition) // Move to the new position
        {
            if (_targetPart != null && moveToTarget) // If target exists and moving to target is allow
            {
                Quaternion targetRotation = Quaternion.LookRotation(_targetPart.position - transform.position); // Set new look rotation based on target
                _rocketRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, _rocketEngine.TurnRate)); // Apply new rotation
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(newAvoidingPosition - transform.position); // Set new look rotation based on avoiding
                _rocketRigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, _rocketEngine.TurnRate)); // Apply new rotation
            }
        }

        private void OnCollisionEnter(Collision collision) // Called when this collider/rigidbody has begun touching another rigidbody/collider
        {
            //DamageManager.instance.DealRocketDamage(this, collision); // Call a method of dealing damage by this rocket to the hitted ship
            DoDamage(_damage, collision.collider);
            DestroyRocket(); // Call destroy rocket method
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

        private void DestroyRocket()
        {
            // Instantiate an explosion effects
            //GameObject spark = Instantiate(_hitSparksPrefab, transform.position, Quaternion.identity);
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            GameObject smoke = Instantiate(_explosionSmokePrefab, transform.position, Quaternion.identity);

            // Destroy this objects after some time
            //Destroy(spark.gameObject, 2f);
            Destroy(explosion.gameObject, 2f);
            Destroy(smoke.gameObject, 2f);

            Destroy(gameObject); // Destroy rocket
        }
    }
}
