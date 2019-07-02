using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour, ITurret
{
    [SerializeField] private AudioClip ShootingSound;

    public Item item; // Item for this turret

    public List<GameObject> TargetsList; // Targets for this turret
    public GameObject NearestTargetWithParameter;

    public GameObject TurretBase; // Base platform of the turret that rotates horizontally
    public GameObject TurretCannons; // Cannons of the turret that totates vertically

    public GameObject ShootPlace;
    public GameObject ShootAnimationPrefab;
    //public GameObject HitHole;

    public Vector3 AimPoint; // The point that the turret should look at

    public float turnRate; // Turret turning speed
    public float turretRange;
    public float cooldown;
    public float currentCooldown;

    [Range(0.0f, 180.0f)] public float rightTraverse; // Maximum right turn in degrees
    [Range(0.0f, 180.0f)] public float leftTraverse; // Maximum left turn in degrees
    [Range(0.0f, 90.0f)] public float elevation; // Maximum turn up in degrees
    [Range(0.0f, 90.0f)] public float depression; // Maximum turn down in degrees

    public bool turretAI; // If the turret is controlled by AI

    public abstract void Shoot();

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        SetTurretParameters();
    }

    private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
    {
        if (turretAI) // If the turret is controlled by AI
        {
            AutomaticTurretControl(); 
        }
        else // If the turret is not controlled by AI
        {
            if (!Cursor.visible) // If cursoris not visible
            {
                ManualTurretControl();
            }
        }

        CooldownDecrease();
    }

    public virtual void SetTurretParameters()
    {
        // Check this turret tag
        switch (transform.tag)
        {
            case "Player":  // If this turret is on a player ship
                // Subscribe to the delegate that called when auto fire changes in the PlayerMovement script
                PlayerMovement.instance.onAutoFireChanged += ChangeTurretAutoFire;
                turretAI = PlayerMovement.instance.autoFire; // Set if the turret is controlled by AI
                TargetsList = Manager.Enemies; // Set enemies as targets for this turret
                break;
            case "Ally": // If this turret is on an ally ship
                turretAI = true; // Set the turret under AI control
                TargetsList = Manager.Enemies; // Set enemies as targets for this turret
                break;
            case "Enemy": // If this turret is on an enemy ship
                turretAI = true; // Set the turret under AI control
                TargetsList = Manager.Allies; // Set allies as targets for this turret
                break;
            default:
                break;
        }
    }

    // Set the turret AI. This method executes by delegate of PlayerMovement script
    public void ChangeTurretAutoFire(bool turretAutoFire)
    {
        turretAI = turretAutoFire;
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
        SearchTheNearestTargetWithParameter();

        if (NearestTargetWithParameter != null) // If there is any target
        {
            AimPoint = NearestTargetWithParameter.transform.position; // Set a target position as an aim point

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
        AimPoint = PlayerShipCameraController.instance.cameraLookingPoint; // Set the point on which camera is looking as an aim point

        RotateBase(); // Totate base of the turret to the aim point
        RotateCannons(); // Totate cannons of the turret to the aim point

        // If a player press left mouse button, turret cooldown is zero and it is not aimed at the owner
        if (Input.GetKey(KeyCode.Mouse0) && CooldownIsZero() && !AimedAtOwner())
        {
            Shoot();
        }
    }

    public void SearchTheNearestTargetWithParameter() // TODO: Transmit a parameter by which search a target
    {
        float distanceToNearestTarget = Mathf.Infinity; // Set distance to nearest target as infinity
        GameObject nearestTarget = null; // Set nearest target as null game object

        for (int targetIndex = 0; targetIndex < TargetsList.Count; targetIndex++) // Pass all targets in targets list
        {
            if (TargetsList[targetIndex] != null) // If target exists
            {
                // Get distance between this turret and target
                float distanceToTarget = Vector3.Distance(transform.position, TargetsList[targetIndex].transform.position);

                if (distanceToTarget < distanceToNearestTarget) // If this target is closer than previous nearest target
                {
                    distanceToNearestTarget = distanceToTarget; // Set new distance to the nearest target
                    nearestTarget = TargetsList[targetIndex]; // Set new nearest target
                }
            }
            else // If target does not exist
            {
                TargetsList.RemoveAt(targetIndex); // Remove target from targets list
            }
        }

        // Set the nearest target if it was found. Otherwise, the nearest target will be null
        NearestTargetWithParameter = nearestTarget;
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
            clampedLocalVector2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * rightTraverse, float.MaxValue);
        }
        else // If the aim point is located to the left of the turret
        {
            // Set new position to rotate with max left traverse
            clampedLocalVector2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * leftTraverse, float.MaxValue);
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
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * elevation, float.MaxValue);
        }
        else // If the aim point is located below the turret
        {
            // Set new position to rotate with max down traverse
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * depression, float.MaxValue);
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
        Ray aimingRay = new Ray(TurretCannons.transform.position, TurretCannons.transform.forward * turretRange);

        // If the turret is targeting an object except bullets and rockets (determined by layerMask)
        if (Physics.Raycast(aimingRay, out RaycastHit hit, turretRange, layerMask))
        {
            if (hit.collider.transform.root.gameObject == NearestTargetWithParameter) // If aiming on current nearest target
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
        Ray aimingRay = new Ray(TurretCannons.transform.position, TurretCannons.transform.forward * turretRange);

        if (Physics.Raycast(aimingRay, out RaycastHit hit, turretRange)) // If turret aiming at some object
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
