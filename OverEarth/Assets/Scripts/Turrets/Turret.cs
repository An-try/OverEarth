using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour, ITurret
{
    [SerializeField] private AudioClip ShootingSound;

    public Item item; // Item for this turret

    public List<GameObject> TargetsList; // Targets for this turret
    public GameObject NearestTargetWithParameter;

    public GameObject TurretBase; // Base platform of the turret that rotates horizontally
    public GameObject TurretCannon; // Cannons of the turret that totates vertically

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

    public RaycastHit hit;

    public abstract void Shoot();

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        SetTurretParameters();

        if (transform.CompareTag("Player")) // If this turret is on player ship
        {
            // Subscribe to the delegate that called when auto fire changes in the PlayerMovement script
            PlayerMovement.instance.autoFireChanged += ChangeTurretAutoFire;
        }
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
        switch (transform.tag)
        {
            case "Player":
                turretAI = PlayerMovement.instance.autoFire;
                TargetsList = Manager.Enemies;
                break;
            case "Ally":
                turretAI = true;
                TargetsList = Manager.Enemies;
                break;
            case "Enemy":
                turretAI = true;
                TargetsList = Manager.Allies;
                break;
            default:
                break;
        }
    }

    // Set the turret AI
    public void ChangeTurretAutoFire(bool turretAutoFire)
    {
        turretAI = turretAutoFire;
    }

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

    public void AutomaticTurretControl()
    {
        SearchTheNearestTargetWithParameter();

        if (NearestTargetWithParameter != null)
        {
            SetAimPoint();
            RotateBase();
            RotateCannon();
        }
        else
        {
            RotateToIdle();
        }

        if (AimedOnEnemy() && CooldownIsZero() && !AimedOnYourself())
        {
            Shoot();
        }
    }

    public void ManualTurretControl()
    {
        SetAimPoint();
        RotateBase();
        RotateCannon();

        if (Input.GetKey(KeyCode.Mouse0) && CooldownIsZero() && !AimedOnYourself())
        {
            Shoot();
        }
    }

    public void SearchTheNearestTargetWithParameter() // TODO: Transmit a parameter by which search a target
    {
        float distanceToNearestTarget = Mathf.Infinity;
        GameObject nearestTarget = null;

        for (int targetIndex = 0; targetIndex < TargetsList.Count; targetIndex++)
        {
            if (TargetsList[targetIndex] != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, TargetsList[targetIndex].transform.position);
                if (distanceToTarget < distanceToNearestTarget)
                {
                    distanceToNearestTarget = distanceToTarget;
                    nearestTarget = TargetsList[targetIndex];
                }
            }
            else
            {
                TargetsList.RemoveAt(targetIndex);
            }
        }
        NearestTargetWithParameter = nearestTarget;
    }

    public void SetAimPoint()
    {
        if (turretAI)
        {
            AimPoint = NearestTargetWithParameter.transform.position;
        }
        else // Если турель в ручном управлении, поворачивать башню на воображаемый объект вдали от центра камеры
        {
            AimPoint = PlayerShipCameraController.instance.cameraLookingPoint;
        }
    }

    public void RotateBase()
    {
        Vector3 localTargetPos = transform.InverseTransformPoint(AimPoint);
        localTargetPos.y = 0f;

        Vector3 clampedLocalVector2Target = localTargetPos;

        if (localTargetPos.x >= 0f)
        {
            clampedLocalVector2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * rightTraverse, float.MaxValue);
        }
        else
        {
            clampedLocalVector2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * leftTraverse, float.MaxValue);
        }

        Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVector2Target);
        Quaternion newRotation = Quaternion.RotateTowards(TurretBase.transform.localRotation, rotationGoal, turnRate * Time.deltaTime);

        TurretBase.transform.localRotation = newRotation;
    }

    public void RotateCannon()
    {
        Vector3 localTargetPos = TurretBase.transform.InverseTransformPoint(AimPoint);
        localTargetPos.x = 0f;

        Vector3 clampedLocalVec2Target = localTargetPos;

        if (localTargetPos.y >= 0f)
        {
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * elevation, float.MaxValue);
        }
        else
        {
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * depression, float.MaxValue);
        }

        Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
        Quaternion newRotation = Quaternion.RotateTowards(TurretCannon.transform.localRotation, rotationGoal, 2 * turnRate * Time.deltaTime);

        TurretCannon.transform.localRotation = newRotation;
    }

    public void RotateToIdle()
    {
        Quaternion newBaseRotation = Quaternion.RotateTowards(TurretBase.transform.localRotation, Quaternion.identity, turnRate * Time.deltaTime);
        Quaternion newCannonRotation = Quaternion.RotateTowards(TurretCannon.transform.localRotation, Quaternion.identity, 2.0f * turnRate * Time.deltaTime);

        TurretBase.transform.localRotation = newBaseRotation;
        TurretCannon.transform.localRotation = newCannonRotation;
    }

    public virtual bool AimedOnEnemy()
    {
        int layerMask = (1 << 8) | (1 << 9); // Выбрать определённые слои
        layerMask = ~layerMask; // Инвертировать эти слои

        Ray ray = new Ray(TurretCannon.transform.position, TurretCannon.transform.forward * turretRange);
        hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, turretRange, layerMask)) // Если навелись на какой-то объект кроме пуль и ракет (определяется layerMask)
        {
            if (hit.collider.transform.root.gameObject == NearestTargetWithParameter) // Если навелся на своего противника
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else // Если не_навелись ни на какой объект
        {
            return false;
        }
    }

    // If the turret aimed at the ship on which it is attached
    public bool AimedOnYourself()
    {
        Ray ray = new Ray(TurretCannon.transform.position, TurretCannon.transform.forward * turretRange);
        hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, turretRange))
        {
            if (hit.collider.transform.root == gameObject.transform.root)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
