using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance; // Singleton for this script

    // Delegate called when player ship spawned
    public delegate void OnAutoFireChanged(bool turretAutoFire);
    public OnAutoFireChanged onAutoFireChanged;

    public bool autoFire = false; // If guns mode setting to auto fire on player ship

    public Rigidbody playerShipRigidbody;

    public Ship shipScript;

    private void Awake() // Awake is called when the script instance is being loaded
    {
        if (instance == null) // If instance not exist
        {
            instance = this; // Set up instance as this script
        }
        else //If instance already exists
        {
            Destroy(this); // Destroy this script
        }
    }

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        shipScript = GetComponent<Ship>(); // Get ship script
        playerShipRigidbody = transform.GetComponent<Rigidbody>(); // Get ship rigidbody
        
        playerShipRigidbody.maxAngularVelocity = shipScript.maxRotationSpeed; // Set max rotation speed to ship's rigidbody max angular velocity
    }

    private void Update() // Update is called every frame
    {
        // If player press any plus button on the keyboard
        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals))
        {
            shipScript.transmission++; // Add a transmission to the ship
        }

        // If player press any minus button on the keyboard
        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
        {
            shipScript.transmission--; // Subtract a transmission from the ship
        }

        // If player press the F button on the keyboard
        if (Input.GetKeyDown(KeyCode.F))
        {
            autoFire = !autoFire; // Change auto fire
            onAutoFireChanged?.Invoke(autoFire);
        }

        shipScript.transmission = Mathf.Clamp(shipScript.transmission, -4, 4); // Clamp ship transmission from -4 to 4
    }

    private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
    {
        MoveShip();
    }

    private void MoveShip()
    {
        MoveForwardOrBack(shipScript.transmission);

        // Call the move methods. Transfer a parameters of input system as moving directions: positive or negative multiplying by forces
        RotateRightOrLeft(Input.GetAxis("RotateRightLeft") * shipScript.rotationForce);
        RotateUpOrDown(Input.GetAxis("RotateUpDown") * shipScript.rotationForce);
        RotateSide(Input.GetAxis("RotateSide") * shipScript.rotationForce);
        StrafeSide(Input.GetAxis("StrafeSide") * shipScript.strafeForce);
        StrafeUpOrDown(Input.GetAxis("StrafeUpDown") * shipScript.strafeForce);

        // If the player’s ship has no orders to move
        if (shipScript.transmission == 0 && Input.GetAxis("StrafeSide") == 0 && Input.GetAxis("StrafeUpDown") == 0)
        {
            playerShipRigidbody.drag = shipScript.autoStopForce; // Apply stop force to the ship
        }
        else // If the player ordered the ship to move in any direction
        {
            playerShipRigidbody.drag = 0; // Do not stop the ship
        }

        // If the player’s ship has no orders to rotate
        if (Input.GetAxis("RotateRightLeft") == 0 && Input.GetAxis("RotateUpDown") == 0 && Input.GetAxis("RotateSide") == 0)
        {
            playerShipRigidbody.angularDrag = 1; // Apply stop rotation force to the ship
        }
        else // If the player ordered the ship to rotate in any direction
        {
            playerShipRigidbody.angularDrag = 0; // Do not stop rotation of the ship
        }

        if (Input.GetKey(KeyCode.Space)) // If player holds space button
        {
            playerShipRigidbody.drag = shipScript.stopForce; // Apply stop force to the ship
            playerShipRigidbody.angularDrag = shipScript.stopForce; // Apply stop rotation force to the ship
        }

        ClampShipVelocity(); // Clamp ship speed
    }

    private void ClampShipVelocity()
    {
        if (playerShipRigidbody.velocity.magnitude > shipScript.maxSpeed) // If the current speed of the ship is higher than the maximum speed of the ship
        {
            // Normalyze rigidbody velocity to maximum speed of the ship
            playerShipRigidbody.velocity = playerShipRigidbody.velocity.normalized * shipScript.maxSpeed;
        }
    }

    public void MoveForwardOrBack(float transmission)
    {
        // Add force to player ship rigidbody to move forward or backward
        playerShipRigidbody.AddForce(transform.forward * shipScript.thrustForce * transmission);
    }

    public void RotateRightOrLeft(float direction)
    {
        // Add torque to player ship rigidbody to rotate right or left
        playerShipRigidbody.AddTorque(transform.up * direction);
    }

    public void RotateUpOrDown(float direction)
    {
        // Add torque to player ship rigidbody to rotate up or down
        playerShipRigidbody.AddTorque(transform.right * direction);
    }

    public void RotateSide(float direction)
    {
        // Add torque to player ship rigidbody to rotate around z axis
        playerShipRigidbody.AddTorque(transform.forward * direction);
    }

    public void StrafeSide(float direction)
    {
        // Add force to player ship rigidbody to move right or left
        playerShipRigidbody.AddForce(transform.right * direction * shipScript.strafeForce);
    }

    public void StrafeUpOrDown(float direction)
    {
        // Add force to player ship rigidbody to move up or down
        playerShipRigidbody.AddForce(transform.up * direction * shipScript.strafeForce);
    }
}
