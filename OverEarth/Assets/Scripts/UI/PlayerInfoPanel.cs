using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    public static PlayerInfoPanel instance; // Singleton for this script

    public Text PlayerInfoText;

    private Ship playerShipScript;

    // Text colors that will be displayed on the player info panel
    private string mainStringColor = "white";
    private string positiveStringColor = "lime";
    private string negativeStringColor = "red";

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
        // Delegate called when player ship is assigned
        Manager.instance.onPlayerShipAssigned += StartUpdateShipInfo;
        playerShipScript = Manager.instance.PlayerShip.GetComponent<Ship>();
    }

    private void StartUpdateShipInfo()
    {
        InvokeRepeating("UpdateShipInfo", 0, 0.1f);
    }

    private void UpdateShipInfo()
    {
        // If player ship exists display the ship's info
        if (Manager.instance.PlayerShip)
        {
            PlayerInfoText.text =
/* Health points     */ $"<color={mainStringColor}>Health: </color>" + OutputCurrentHP() +
/* Defence           */ $"\n<color={mainStringColor}>Defence:</color>" + OutputCurrentDefence() +
/* Speed             */ $"\n<color={mainStringColor}>Speed: </color>" + OutputCurrentSpeed() +
/* Transmission      */ $"\n<color={mainStringColor}>Transmission: </color>" + OutputCurrentTransmission() +
/* Stop force        */ $"\n<color={mainStringColor}>Stop force: </color>" + OutputCurrentStopForce() +
/* Turret auto fire  */ $"\n<color={mainStringColor}>Turret auto fire: </color>" + OutputIfAutoFire();
        }
        else // If player ship does not exist inform player that it was destroyed
        {
            PlayerInfoText.text = $"<color={negativeStringColor}>Player ship has been destroyed</color>";
        }
    }

    private string OutputCurrentHP()
    {
        int playerShipHP = (int)playerShipScript.HP; // Get player ship script and HP
        return $"<color={positiveStringColor}>" + playerShipHP + "</color>"; // Return player ship HP
    }

    private string OutputCurrentDefence()
    {
        return $"<color={positiveStringColor}>\n Kinetic:     " + playerShipScript.kineticDefence + "%" +
                                             "\n Explosion: " + playerShipScript.explosionDefence + "%" +
                                             "\n Laser:       " + playerShipScript.laserDefence + "%" +
                                             "\n Flame:       " + playerShipScript.flameDefence + "%" +
                                             "\n Fragment:  " + playerShipScript.fragmentDefence + "%</color>";
    }

    private string OutputCurrentSpeed()
    {
        int PlayerSpeed = (int)PlayerMovement.instance.PlayerShipRigidbody.velocity.magnitude;

        if (PlayerSpeed > 0)
        {
            return $"<color={positiveStringColor}>" + PlayerSpeed + "</color>";
        }
        return $"<color={negativeStringColor}>" + PlayerSpeed + "</color>";
    }

    private string OutputCurrentTransmission()
    {
        int PlayerTransmission = playerShipScript.transmission;

        if (PlayerTransmission > 0)
        {
            return $"<color={positiveStringColor}>" + PlayerTransmission + "</color>";
        }
        return $"<color={negativeStringColor}>" + PlayerTransmission + "</color>";
    }

    private string OutputCurrentStopForce()
    {
        int StopForce = (int)PlayerMovement.instance.PlayerShipRigidbody.drag;

        if (StopForce > 0)
        {
            return $"<color={positiveStringColor}>" + StopForce + "</color>";
        }
        return $"<color={negativeStringColor}>" + StopForce + "</color>";
    }

    private string OutputIfAutoFire()
    {
        if (PlayerMovement.instance.autoFire)
        {
            return $"<color={positiveStringColor}>Yes</color>";
        }
        return $"<color={negativeStringColor}>No</color>";
    }
}
