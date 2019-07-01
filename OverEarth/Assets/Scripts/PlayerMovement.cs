using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance; // Instance for PlayerMovement script

    public delegate void AutoFireChanged(bool turretAutoFire);
    public AutoFireChanged autoFireChanged;
    public bool autoFire = false; // If guns mode setting to auto fire on player ship

    public static Rigidbody PlayerShipRigidbody;

    public Ship ShipScript;

    private void Awake()
    {
        // If instance not exist, set up instance as this
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //If instance already exists and it's not this -> destroy this game object
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        ShipScript = GetComponent<Ship>();
        PlayerShipRigidbody = transform.GetComponent<Rigidbody>();

        ShipScript.maxSpeed += 0.1f;
        PlayerShipRigidbody.maxAngularVelocity = ShipScript.maxRotationSpeed;

        ShipScript.thrustForce *= PlayerShipRigidbody.mass;
        ShipScript.strafeForce *= 100;
        ShipScript.rotationForce *= PlayerShipRigidbody.mass;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals))
        {
            ShipScript.transmission++;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
        {
            ShipScript.transmission--;
        }
        ShipScript.transmission = Mathf.Clamp(ShipScript.transmission, -4, 4);

        if (Input.GetKeyDown(KeyCode.F))
        {
            autoFire = !autoFire;
            autoFireChanged?.Invoke(autoFire);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        MoveForwardOrBack(ShipScript.transmission);

        RotateRightOrLeft(Input.GetAxis("RotateRightLeft") * ShipScript.rotationForce * 100f);
        RotateUpOrDown(Input.GetAxis("RotateUpDown") * ShipScript.rotationForce * 100f);
        RotateSide(Input.GetAxis("RotateSide") * ShipScript.rotationForce * 100f);
        StrafeSide(Input.GetAxis("StrafeSide") * ShipScript.strafeForce * 100f);
        StrafeUpOrDown(Input.GetAxis("StrafeUpDown") * ShipScript.strafeForce * Time.deltaTime);

        if (ShipScript.transmission == 0 && Input.GetAxis("StrafeSide") == 0 && Input.GetAxis("StrafeUpDown") == 0)
        {
            PlayerShipRigidbody.drag = ShipScript.autoStopForce;
        }
        else
        {
            PlayerShipRigidbody.drag = 0;
        }

        if (Input.GetAxis("RotateRightLeft") != 0 || Input.GetAxis("RotateUpDown") != 0 || Input.GetAxis("RotateSide") != 0)
        {
            PlayerShipRigidbody.angularDrag = 0;
        }
        else
        {
            PlayerShipRigidbody.angularDrag = 1;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            PlayerShipRigidbody.drag = ShipScript.stopForce;
            PlayerShipRigidbody.angularDrag = ShipScript.stopForce;
        }
        ClampVelocity();
    }

    private void ClampVelocity()
    {
        if (PlayerShipRigidbody.velocity.magnitude > ShipScript.maxSpeed)
        {
            PlayerShipRigidbody.velocity = PlayerShipRigidbody.velocity.normalized * ShipScript.maxSpeed;
        }
    }

    public void MoveForwardOrBack(float transmission)
    {
        PlayerShipRigidbody.AddForce(transform.forward * ShipScript.thrustForce * transmission);
    }

    public void RotateRightOrLeft(float direction)
    {
        PlayerShipRigidbody.AddTorque(transform.up * direction * 10000f * Time.deltaTime);
    }

    public void RotateUpOrDown(float direction)
    {
        PlayerShipRigidbody.AddTorque(transform.right * direction * 10000f * Time.deltaTime);
    }

    public void RotateSide(float direction)
    {
        PlayerShipRigidbody.AddTorque(transform.forward * direction * 10000f * Time.deltaTime);
    }

    public void StrafeSide(float direction)
    {
        PlayerShipRigidbody.AddForce(transform.right * direction * ShipScript.strafeForce);
    }

    public void StrafeUpOrDown(float direction)
    {
        PlayerShipRigidbody.AddForce(transform.up * direction * ShipScript.strafeForce * 1000f);
    }
}